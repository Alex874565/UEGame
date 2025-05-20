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
        public string title = "European Parliament Elections";
        public string countryName = "Brussels";
        public InstitutionType institution = InstitutionType.Parliament;
        public string electionDate = "13/06/1999";
        [TextArea] public string description;
        public string president = string.Empty;
        [SerializeField]
        public List<Party> parties = new List<Party>
        {
            new Party { partyName = "EPP-ED", seats = 223 },
            new Party { partyName = "PES", seats = 175 },
            new Party { partyName = "ELDR", seats = 52 },
            new Party { partyName = "Greens/EFA", seats = 45 },
            new Party { partyName = "GUE/NGL", seats = 42 },
            new Party { partyName = "UEN", seats = 23 },
            new Party { partyName = "EDD", seats = 18 },
            new Party { partyName = "NI", seats = 38 }
        };


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
