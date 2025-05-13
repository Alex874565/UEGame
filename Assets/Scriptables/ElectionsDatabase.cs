using NUnit.Framework;
using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "ElectionsDatabase", menuName = "Scriptable Objects/ElectionsDatabase")]
public class ElectionsDatabase : ScriptableObject
{
    public List<Election> elections;

    [System.Serializable]
    public class Election
    {
        public string electionDate;
        [SerializeField] public List<Party> parties;

        [System.Serializable]
        public class Party
        {
            public string partyName;
            public int seats;
        }
    }
}
