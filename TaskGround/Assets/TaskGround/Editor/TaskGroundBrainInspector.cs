using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEditor;

namespace wararyo.TaskGround
{

	//[CustomEditor (typeof(TaskGroundBrainBase))]
	public class TaskGroundBrainInspector : Editor
	{
		public override void OnInspectorGUI ()
		{
			TaskGroundBrainBase t = (TaskGroundBrainBase)target;
			if (GUILayout.Button ("Sync", null)) {
				t.Sync ();
			}
			EditorGUILayout.HelpBox ("Last Synced: " + t.getLastSynced().ToString() , MessageType.Info);
			EditorGUILayout.Space ();
			base.OnInspectorGUI ();
		}
	}

}