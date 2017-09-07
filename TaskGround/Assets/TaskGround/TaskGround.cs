using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace wararyo.TaskGround {

public static class TaskGround {

		private static Task m_selectingTask;
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

		public static System.Action OnSelectingTaskChanged;

}

}