using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace wararyo.TaskGround
{
	[SelectionBase]
    public class TaskPin : MonoBehaviour
    {
        public Transform player;
        public float size = 1;
        public float scaleFactor = 0.1f;
        public float minSize = 0.2f;

        public TextMesh titleText;
		[SerializeField, HideInInspector]
        private Task m_task;
		public Task task{
			get{
				return m_task;
			}
			set{
				m_task = value;
				titleText.text = task.title;
			}
		}

        // Use this for initialization
        void Start()
        {
			
        }

        // Update is called once per frame
        void Update()
        {
            //ずっとこっち向く
            Vector3 target = Camera.main.transform.position;
            target.y = this.transform.position.y;
            this.transform.LookAt(target);

            //遠くに行くほど大きくなるか小さくなる
            float distance = Vector3.Distance(player.position, transform.position);
            float scale = Mathf.Max(minSize,size + (scaleFactor * distance));
            transform.localScale = new Vector3(scale, scale, scale);
        }

        public static void Instantiate(Transform parent, Task task, Transform player, float size, float scaleFactor, float minSize)
        {
#if UNITY_EDITOR
            const string PrefabGUID = "a0a7bc9fda6e3194ea8fa6d188e3f802";
			string prefabPath = AssetDatabase.GUIDToAssetPath(PrefabGUID);
			Object prefab = AssetDatabase.LoadAssetAtPath<Object>(prefabPath);

			GameObject go = Instantiate<GameObject>((GameObject)prefab,parent);
			go.transform.position = task.position;
			TaskPin tp = go.GetComponent<TaskPin>();
			tp.task = task;
			tp.player = player;
			tp.size = size;
			tp.scaleFactor = scaleFactor;
			tp.minSize = minSize;
#endif
        }
    }

}