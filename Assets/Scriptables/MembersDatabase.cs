using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "MembersDatabase", menuName = "Scriptable Objects/MembersDatabase")]
public class MembersDatabase : ScriptableObject
{
    [SerializeField] public List<MemberEvent> memberEvents;

    [System.Serializable]
    public class MemberEvent {
        public string date;
        public string title;
        public string description;
        public string countryName;

        public Sprite newMap;
    }
}
