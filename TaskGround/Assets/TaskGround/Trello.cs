using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace wararyo.TaskGround {

	public class Trello {
		public const string APIKEY = "2794682bb58abe778a8b2db5e1c6647d";//もっとスマートな保管方法ないですかね
	}

	[System.Serializable]
	public class TrelloUser {
		public string fullName;
	}

	[System.Serializable]
	public class TrelloBoard{
		public string name;
		public string id;
	}

	[System.Serializable]
	public class TrelloList{
		public string name;
		public string id;
	}

	[System.Serializable]
	public class TrelloCard{

	}

}