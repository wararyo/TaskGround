using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.Networking;

namespace wararyo.TaskGround {

	public class ViewerWindow : EditorWindow {

		private Task m_currentTask;
		Task currentTask {
			get {
				return m_currentTask;
			}
			set {
				m_currentTask = value;
				OnTaskChanged ();
			}
		}

		private bool trelloIsAuthorizing = false;

		private int tab = 0;

		private string trelloUsername = "";
		private string trelloToken = "";

		private const string APIKEY = "2794682bb58abe778a8b2db5e1c6647d";//もっとスマートな保管方法ないですかね
		private const string APIURL = "https://api.trello.com/1/authorize?key={0}&name=TaskGround&expiration=never&response_type=token&scope=read,write";

		const string KEY_USERNAME = "TaskGround.TrelloUserName";
		const string KEY_TOKEN = "TaskGround.TrelloToken";

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

		}

		void OnEnable()
		{

		}

		void OnGUI()
		{
			tab = GUILayout.Toolbar(tab, new string[] { "Information", "Settings" }, EditorStyles.toolbarButton, null);

			if (tab == 0) {//Information Tab
				
				if (currentTask == null) {
					EditorGUILayout.LabelField ("There is no task to show.");
				} else {

				}

			} else {//Setting Tab

				//Trello
				trelloUsername = EditorUserSettings.GetConfigValue(KEY_USERNAME);
				trelloToken = EditorUserSettings.GetConfigValue (KEY_TOKEN);

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
				}

			}
		}

		void OpenTrelloAuth(){
			Application.OpenURL (string.Format(APIURL,APIKEY));
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
		}

		IEnumerator AuthTrelloCoroutine(string token){
			WWW www = new WWW(string.Format("https://api.trello.com/1/members/me?key={0}&token={1}&fields=fullName",APIKEY,token));
			yield return www;
			if (!string.IsNullOrEmpty (www.error)) {
				Debug.Log (www.error);
				trelloIsAuthorizing = false;
			} else {
				Debug.Log (www.text);
				TrelloUser tu = JsonUtility.FromJson<TrelloUser> (www.text);
				trelloUsername = tu.fullName;
				trelloToken = token;
				EditorUserSettings.SetConfigValue (KEY_TOKEN, trelloToken);
				EditorUserSettings.SetConfigValue (KEY_USERNAME, trelloUsername);
				trelloIsAuthorizing = false;
			}
			www.Dispose ();
		}
	}

	[System.Serializable]
	public class TrelloUser {
		public string fullName;
	}

}