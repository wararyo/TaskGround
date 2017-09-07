using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.Networking;
using MiniJSON;

namespace wararyo.TaskGround {

	public class ViewerWindow : EditorWindow {

		private Task currentTask;

		private bool trelloIsAuthorizing = false;

		private int tab = 0;

		private string trelloUsername = "";
		private string trelloToken = "";
		private string trelloBoardId = "";

		private const string APIURL_USERNAME = "https://api.trello.com/1/authorize?key={0}&name=TaskGround&expiration=never&response_type=token&scope=read,write";
		private const string APIURL_BOARDS = "https://api.trello.com/1/members/me/boards?key={0}&token={1}&fields=name";

		private List<string> trelloBoardNames;
		private List<string> trelloBoardIDs;
		private int m_trelloBoardIndex;
		private int trelloBoardIndex{
			get{
				return m_trelloBoardIndex;
			}
			set{
				m_trelloBoardIndex = value;
				trelloBoardId = trelloBoardIDs[value];
				EditorUserSettings.SetConfigValue (Trello.KEY_BOARD, trelloBoardId);
			}
		}

		private string tokenField_text = "";

		[MenuItem("Window/TaskGround")]
		static void Open()
		{
			var window = EditorWindow.GetWindow<ViewerWindow>("TaskGround");
			//string iconPath = AssetDatabase.GUIDToAssetPath ("40470457539b24899825bad08fdb5ed1");
			//Debug.Log (iconPath);
			//var icon = AssetDatabase.LoadAssetAtPath<Texture> (iconPath);

			//window.titleContent = new GUIContent ("TaskGround", icon);

			window.minSize = new Vector2(256, 256);
		}

		void OnTaskChanged()
		{
			currentTask = TaskGround.SelectingTask;
			if (tab == 0)
				Repaint ();
		}

		//TaskPinかその子供を選んでいたら、その詳細を表示する(一旦TaskGroundクラスを経由してまたViewerWindowに帰ってくる)
		//Playingの時にはTaskGroundBrain#Updateにて
		void OnSelectionChange(){
			if (EditorApplication.isPlaying)
				return;
			if (Selection.activeGameObject == null) {
				TaskGround.SelectingTask = null;
				return;
			}
			
			TaskPin tp = Selection.activeGameObject.GetComponent<TaskPin> ();
			if (tp != null) {
				TaskGround.SelectingTask = tp.task;
			} else {
				tp = Selection.activeGameObject.GetComponentInParent<TaskPin> ();
				if (tp != null)
					TaskGround.SelectingTask = tp.task;
				else
					TaskGround.SelectingTask = null;
			}
		}

		void OnEnable()
		{
			trelloBoardIDs = new List<string>();
			trelloBoardNames = new List<string>();

			//Trello
			trelloUsername = EditorUserSettings.GetConfigValue(Trello.KEY_USERNAME);
			trelloToken = EditorUserSettings.GetConfigValue (Trello.KEY_TOKEN);
			trelloBoardId = EditorUserSettings.GetConfigValue (Trello.KEY_BOARD);

			TaskGround.OnSelectingTaskChanged += OnTaskChanged ;
		}

		void OnGUI()
		{
			tab = GUILayout.Toolbar(tab, new string[] { "Information", "Settings" }, EditorStyles.toolbarButton, null);

			if (tab == 0) {//Information Tab
				
				if (currentTask == null) {
					EditorGUILayout.LabelField ("There is no task to show.");
				} else {
					EditorGUILayout.LabelField(currentTask.title, EditorStyles.boldLabel);
					EditorGUILayout.Space ();
					EditorGUILayout.TextArea (currentTask.description, EditorStyles.whiteLargeLabel);
				}

			} else {//Setting Tab

				EditorGUILayout.LabelField("Trello Settings", EditorStyles.boldLabel);
				if (trelloUsername == null || trelloUsername == "") {//Not logged in
					EditorGUILayout.LabelField("Not logged in");
					if (trelloIsAuthorizing) {
						tokenField_text = EditorGUILayout.TextField ("Token",tokenField_text);
						if (GUILayout.Button ("Continue"))
							AuthTrello (tokenField_text);
					} else {
						if (GUILayout.Button ("Start Authorize"))
							OpenTrelloAuth ();
					}
				} else {//logged in
					EditorGUILayout.LabelField("Logged in as " + trelloUsername);
					if (GUILayout.Button ("Logout"))
						LogoutTrello ();
					if (trelloBoardIDs.Count == 0)
						EditorCoroutine.Start(ReloadTrelloBoards ());
					else trelloBoardIndex = EditorGUILayout.Popup ("Board" , trelloBoardIndex, trelloBoardNames.ToArray(),null);
				}

			}
		}

		void OpenTrelloAuth(){
			Application.OpenURL (string.Format(APIURL_USERNAME,Trello.APIKEY));
			tokenField_text = "";
			trelloIsAuthorizing = true;
		}

		void AuthTrello(string token){
			Debug.Log ("Token:" + token);
			if (token == "") {
				trelloIsAuthorizing = false;
				return;
			}
			EditorCoroutine.Start (AuthTrelloCoroutine (token));

		}

		void LogoutTrello(){
			trelloToken = "";
			trelloUsername = "";
			SetUserSettings ();
			Repaint ();
		}

		IEnumerator AuthTrelloCoroutine(string token){
			WWW www = new WWW(string.Format("https://api.trello.com/1/members/me?key={0}&token={1}&fields=fullName",Trello.APIKEY,token));
			yield return www;
			if (!string.IsNullOrEmpty (www.error)) {
				Debug.Log (www.error);
				trelloIsAuthorizing = false;
			} else {
				//Debug.Log (www.text);
				TrelloUser tu = JsonUtility.FromJson<TrelloUser> (www.text);
				trelloUsername = tu.fullName;
				trelloToken = token;
				SetUserSettings ();
				trelloIsAuthorizing = false;
			}
			Repaint ();
			www.Dispose ();
		}

		IEnumerator ReloadTrelloBoards(){
			WWW www = new WWW(string.Format(APIURL_BOARDS,Trello.APIKEY,trelloToken));
			yield return www;
			if (!string.IsNullOrEmpty (www.error)) {
				Debug.Log (www.error);
			} else if(!string.IsNullOrEmpty(www.text)) {
				IList json = (IList) MiniJSON.Json.Deserialize (www.text);
				trelloBoardIDs.Clear ();
				trelloBoardNames.Clear ();
				foreach (IDictionary b in json) {
					trelloBoardIDs.Add ((string)b ["id"]);
					trelloBoardNames.Add ((string)b ["name"]);
				}
				trelloBoardIndex = trelloBoardIDs.IndexOf (trelloBoardId);
			}
		}

		void SetUserSettings(){
			EditorUserSettings.SetConfigValue (Trello.KEY_TOKEN, trelloToken);
			EditorUserSettings.SetConfigValue (Trello.KEY_USERNAME, trelloUsername);
			EditorUserSettings.SetConfigValue (Trello.KEY_BOARD, trelloBoardId);
		}
	}

}