using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace wararyo.TaskGround
{

    public class TaskGroundBrainTrello : TaskGroundBrainBase
    {

        new void Start()
        {
            base.Start();
        }

        new void Update()
        {
            base.Update();
        }

		//Called in Sync()
        protected override void Push(Change c)
        {

        }

		//Called in Sync()
        protected override List<Task> Pull()
		{
			List<Task> l = new List<Task>();
			l.Add(new Task("hogehoge",new Vector3(1,0,1),"ちくわ大明神","ちくわ","https://wararyo.com"));
			l.Add (new Task ("piyopiyo", new Vector3 (10, 0, 10), "私たちはここにいます\nここには夢がちゃんとある", "わーい", "https://wararyo.com"));
			return l;
        }
    }

}
