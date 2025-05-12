using NUnit.Framework;
using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "PartiesDatabase", menuName = "Scriptable Objects/PartiesDatabase")]
public class PartiesDatabase : ScriptableObject
{
    [SerializeField] public List<Party> parties;

    [System.Serializable]
    public class Party
    {
        public string partyName;
        public int seats;
        public string joinDate;
    }
}
