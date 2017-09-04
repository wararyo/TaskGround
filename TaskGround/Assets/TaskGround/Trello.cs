using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace wararyo.TaskGround {

	public static class Trello {
		public const string APIKEY = "2794682bb58abe778a8b2db5e1c6647d";//もっとスマートな保管方法ないですかね

		public const string KEY_USERNAME = "TaskGround.TrelloUserName";
		public const string KEY_TOKEN = "TaskGround.TrelloToken";
		public const string KEY_BOARD = "TaskGround.TrelloBoard";

		public static Task TrelloCardToTask(IDictionary card){
			string desc = (string)card ["desc"];
			int index = desc.IndexOf ("TaskGround (");
			string sPos = desc.Substring(index + "TaskGround ".Length);
			desc = desc.Substring (0, index);
			Debug.Log (sPos);
			return new Task ((string)card ["id"], TaskUtil.Vector3Parse (sPos), (string)card ["name"], desc, (string)card ["shortUrl"]);
		}

		/*static IDictionary TaskToTrelloCard(Task task){
			IDictionary dic = new IDictionary();
			return dic;
		}*/
	}

	[System.Serializable]
	public class TrelloUser {
		public string fullName;
	}
}