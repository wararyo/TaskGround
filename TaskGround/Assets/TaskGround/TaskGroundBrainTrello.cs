using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace wararyo.TaskGround
{

    public class TaskGroundBrainTrello : TaskGroundBrainBase
    {

        // Use this for initialization
        new void Start()
        {
            base.Start();
        }

        // Update is called once per frame
        new void Update()
        {
            base.Update();
        }

        protected override void Push(Change c)
        {

        }

        protected override List<Task> Pull()
        {
            return new List<Task>();
        }
    }

}
