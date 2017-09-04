using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MiniJSON;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace wararyo.TaskGround
{

    public class TaskGroundBrainTrello : TaskGroundBrainBase
    {

		private const string APIURL_LISTS = "https://api.trello.com/1/boards/{0}/lists?key={1}&token={2}&fields=name&filter=open";
		private const string APIURL_CARDS = "https://api.trello.com/1/lists/{0}/cards?key={1}&token={2}&fields=name,desc,shortUrl";

		[SerializeField, HideInInspector]
		private string boardId = "";

		[SerializeField, HideInInspector]
		private string listId = "";

		private string trelloToken = "";
		private string trelloBoardId = "";

		[HideInInspector]
		public List<string> listIds;
		[HideInInspector]
		public List<string> listNames;
		private int m_listIndex = 0;
		public int listIndex {
			get {
				return m_listIndex;
			}
			set {
				m_listIndex = value;
				listId = listIds [value];
			}
		}


        new void Start()
        {
            base.Start();
			#if UNITY_EDITOR
			trelloToken = EditorUserSettings.GetConfigValue (Trello.KEY_TOKEN);
			trelloBoardId = EditorUserSettings.GetConfigValue (Trello.KEY_BOARD);
			#endif

			if(listIds == null) listIds = new List<string> ();
			if(listNames == null) listNames = new List<string> ();
			listIndex = listIds.IndexOf (listId);

        }

        new void Update()
        {
            base.Update();
        }

		//Called in Sync()
		protected override IEnumerator OnSyncStarted ()
		{
			yield return StartCoroutine (UpdateList ());
		}

		//Called in Sync()
		protected override IEnumerator Push(Change c)
        {
			yield return null;
        }

		//Called in Sync()
		protected override IEnumerator Pull(Action<List<Task>> callback)
		{
			#if UNITY_EDITOR
			trelloToken = EditorUserSettings.GetConfigValue (Trello.KEY_TOKEN);
			trelloBoardId = EditorUserSettings.GetConfigValue (Trello.KEY_BOARD);
			#endif

			WWW www = new WWW(string.Format(APIURL_CARDS,listId,Trello.APIKEY,trelloToken));
			yield return www;
			if (!string.IsNullOrEmpty (www.error)) {
				Debug.Log (www.error);
				callback (null);
			} else if (!string.IsNullOrEmpty (www.text)) {
				Debug.Log (www.text);
				IList json = (IList)MiniJSON.Json.Deserialize (www.text);

				List<Task> l = new List<Task> ();

				foreach (IDictionary b in json) {
					Task t = Trello.TrelloCardToTask (b);
					l.Add (t);
				}

				//l.Add (new Task ("hogehoge", new Vector3 (1, 0, 1), "ちくわ大明神", "ちくわ", "https://wararyo.com"));
				//l.Add (new Task ("piyopiyo", new Vector3 (10, 0, 10), "私たちはここにいます\nここには夢がちゃんとある", "わーい", "https://wararyo.com"));
				callback (l);
			}
        }

		IEnumerator UpdateList(){
			WWW www = new WWW(string.Format(APIURL_LISTS,trelloBoardId,Trello.APIKEY,trelloToken));
			yield return www;
			if (!string.IsNullOrEmpty (www.error)) {
				Debug.Log (www.error);
			} else if (!string.IsNullOrEmpty (www.text)) {
				IList json = (IList) MiniJSON.Json.Deserialize (www.text);
				listIds.Clear ();
				listNames.Clear ();
				foreach (IDictionary b in json) {
					listIds.Add ((string)b ["id"]);
					listNames.Add ((string)b ["name"]);
				}
			}
		}
    }

}
