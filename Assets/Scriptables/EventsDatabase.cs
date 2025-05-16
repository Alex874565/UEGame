using NUnit.Framework;
using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using Unity.VisualScripting;
using System;

[CreateAssetMenu(fileName = "EventsDatabase", menuName = "Scriptable Objects/EventsDatabase")]
public class EventsDatabase : ScriptableObject
{
    [SerializeField] public List<Event> events;

    [System.Serializable]
    public class Event
    {
        public string eventDate;
        public string title;
        public string countryName;
        [TextArea] public string description;
        public GameObject location;
        public string question;
        public string EuChoice;
        [TextArea] public string EuChoiceDescription;

        public Sprite eventIcon;

        [SerializeField] public List<Choice> choices;

        [System.Serializable]
        public class Choice
        {
            public string title;
            [TextArea] public string description;
            [TextArea] public string consequences;
            public float euroscepticismModifier;
            public float foreignAffairsModifier;
            public long moneyModifier;
            public Sprite eventIcon;
            public Dictionary<string, string> partyApprovalChances;
        }
    }
}