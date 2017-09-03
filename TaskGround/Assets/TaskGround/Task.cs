using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace wararyo.TaskGround
{

    public class Task
    {
        public string ID;
        public Vector3 position;
		public string title;
		public string description;
		public string url;

		public Task(string ID,Vector3 position,string title,string description,string url){
			this.ID = ID;
			this.position = position;
			this.title = title;
			this.description = description;
			this.url = url;
		}
    }

}
