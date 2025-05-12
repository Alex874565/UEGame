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
        [SerializeField] public List<Choice> choices;

        public string date;
        public string title;
        public string description;
        public GameObject location;
        public string question;
        public Sprite eventIcon;
        public int EUChoiceIndex;

        [System.Serializable]
        public class Choice
        {
            public string title;
            public string description;
            public Dictionary<string, string> loyaltyModifiers;
            public Dictionary<string, string> partyApprovalChances;
            public Sprite eventIcon;
        }
    }
}