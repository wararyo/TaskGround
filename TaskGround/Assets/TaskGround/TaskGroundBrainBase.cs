using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
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

		public Transform player;
		public float size;
		public float scaleFactor;
		public float minSize;

		[SerializeField, HideInInspector]
		private string lastSynced;

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

		public DateTime getLastSynced()
		{
			try { return DateTime.Parse(lastSynced);}
			catch {
				return DateTime.MinValue;
			}
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

		public void StartSync(){
			StartCoroutine (SyncWork ());
		}

		public IEnumerator SyncWork()
        {
			yield return StartCoroutine (OnSyncStarted ());
            //送信
            foreach(Change c in changes)
            {
				yield return StartCoroutine(Push(c));
            }

            //受信
			List<Task> response = new List<Task>();
			yield return StartCoroutine(Pull(r => response = r));

			if (response == null) yield break;//受信失敗
			Transform pins = transform.Find("Pins");
			//一旦全部消す
			for( int i = pins.childCount - 1; i >= 0; --i ){
				GameObject.DestroyImmediate( pins.GetChild( i ).gameObject );
			}
			//それから追加していく
            foreach(Task t in response)
            {
				TaskPin.Instantiate(pins,t,player,size,scaleFactor,minSize);
            }

			lastSynced = DateTime.Now.ToString ();
        }

		protected abstract IEnumerator OnSyncStarted ();

		protected abstract IEnumerator Push(Change c);

		protected abstract IEnumerator Pull(Action<List<Task>> callback);


    }

}