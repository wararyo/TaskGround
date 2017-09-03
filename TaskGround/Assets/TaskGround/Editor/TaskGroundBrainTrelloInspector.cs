using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace wararyo.TaskGround
{

	[CustomEditor (typeof(TaskGroundBrainTrello))]
	public class TaskGroundBrainTrelloInspector : TaskGroundBrainInspector
	{
		public override void OnInspectorGUI ()
		{
			base.OnInspectorGUI ();
		}
	}

}