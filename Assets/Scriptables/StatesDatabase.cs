using UnityEngine;
using System;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "StatesDatabase", menuName = "Scriptable Objects/StatesDatabase")]
public class StatesDatabase : ScriptableObject
{
    [SerializeField] public List<State> states;

    [System.Serializable]
    public class State
    {
        public string stateName;
        public int loyalty;
        public string joinDate;
    }
}
