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
        public string title;
        public string countryName = "Brussels";
        public InstitutionType institution;
        public string electionDate;
        [TextArea] public string description;
        public string president;
        [SerializeField] public List<Party> parties;

        [System.Serializable]
        public class Party
        {
            public string partyName;
            public int seats;
        }

        public enum InstitutionType
        {
            Parliament = 1,
            Comission = 2,
            EuropeanCouncil = 3,
            CouncilOfEU = 4
        }
    }
}
