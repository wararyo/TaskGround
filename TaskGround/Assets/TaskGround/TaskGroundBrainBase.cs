using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace wararyo.TaskGround
{
    [ExecuteInEditMode]
    public abstract class TaskGroundBrainBase : MonoBehaviour
    {

        [SerializeField, HideInInspector]
        List<Change> changes;//変更のキュー

        // Use this for initialization
        public void Start()
        {
            if (!Application.isEditor) Destroy(gameObject); //エディターじゃなかったら自殺
            changes = new List<Change>();
        }

        // Update is called once per frame
        public void Update()
        {

        }

        public void DeleteTask(string ID)
        {
            AddChange(ID, ChangeType.Delete, Vector3.zero, null, null);
        }

        public void MoveTask(string ID, Vector3 position)
        {
            AddChange(ID, ChangeType.Modify, position,null,null);
        }

        public void AddChange(string ID,ChangeType type,Vector3 position,string title,string description)
        {

        }

        public bool Sync()
        {
            //送信
            foreach(Change c in changes)
            {
                Push(c);
            }

            //受信
            List<Task> response = Pull();
            if (response == null) return false;//受信失敗
            foreach(Task t in response)
            {
                //Instantiate();
            }
            return true;
        }

        protected abstract void Push(Change c);

        protected abstract List<Task> Pull();


    }

}
