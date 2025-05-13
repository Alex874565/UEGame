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
        public string description;
        public GameObject location;
        public string question;
        public Sprite eventIcon;
        public Choice EuChoice;

        [SerializeField] public List<Choice> choices;


        [System.Serializable]
        public class Choice
        {
            public string title;
            public string description;
            public string euroscepticismModifier;
            public string moneyModifier;
            public string foreignAffairsModifier;
            public Sprite eventIcon;
            public Dictionary<string, string> partyApprovalChances;
        }
    }
}