using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

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
		private const string APIURL = "https://trello.com/1/authorize?key={0}&name=TaskGround&expiration=never&response_type=token&scope=read,write";

		const string KEY_USERNAME = "TaskGround.TrelloUserName";
		const string KEY_TOKEN = "TaskGround.TrelloToken";

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
			tab = GUILayout.Toolbar(tab, new string[] { "Infomation", "Settings" }, EditorStyles.toolbarButton, null);

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
						string token = EditorGUILayout.TextField ("Token","");
						if (GUILayout.Button ("Continue"))
							AuthTrello (token);
					} else {
						if (GUILayout.Button ("Start Authorize"))
							OpenTrelloAuth ();
					}
				} else {//logged in
					EditorGUILayout.LabelField("Logged in as " + trelloUsername);

				}

			}
		}

		void OpenTrelloAuth(){
			Application.OpenURL (string.Format(APIURL,APIKEY));
			trelloIsAuthorizing = true;
		}

		void AuthTrello(string token){
			if (token == "")
				trelloIsAuthorizing = false;
			
			//Succeed.

			//trelloToken = token;

		}
	}

}