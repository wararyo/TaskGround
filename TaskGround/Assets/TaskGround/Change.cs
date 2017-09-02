using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace wararyo.TaskGround {

    public enum ChangeType
    {
        Create,
        Modify,
        Delete,
    }

    [System.Serializable]
    public class Change {

        public string ID;
        public ChangeType type;
        public Vector3 position;
        public string title;
        public string description;

        public Change(string ID,ChangeType type,Vector3 position,string title,string description)
        {
            this.ID = ID;
            this.type = type;
            this.position = position;
            this.title = title;
            this.description = description;
        }
    }

}
