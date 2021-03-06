﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace wararyo.TaskGround {

public static class TaskGround {

		private static Task m_selectingTask = null;
		public static Task SelectingTask{
			get{
				return m_selectingTask;
			}
			set{
				if (m_selectingTask != value) {
					m_selectingTask = value;
					OnSelectingTaskChanged ();
				}
			}
		}

		public static void AddChange(Change c){
			OnChange (c);
		}

		public static System.Action OnSelectingTaskChanged;
		public static System.Action<Change> OnChange = delegate {};

}

}