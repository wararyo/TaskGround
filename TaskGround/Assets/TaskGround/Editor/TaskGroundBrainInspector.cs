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
				t.StartSync ();
			}
			DateTime lastSynced = t.getLastSynced ();
			EditorGUILayout.HelpBox (lastSynced == DateTime.MinValue ? "Not synced yet." : 
				"Last Synced: " + lastSynced.ToString() + 
				(t.changes.Count == 0 ? "" : "\n" + t.changes.Count + " changes in queue.") , MessageType.Info);
			if (GUILayout.Button ("Discard Changes"))
				t.DiscardChanges ();
			EditorGUILayout.Space ();
			base.OnInspectorGUI ();
		}
	}

}