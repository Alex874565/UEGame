using System.Collections.Generic;
using UnityEngine;

public class EUStats : MonoBehaviour
{
    public static EUStats Instance;

    private void Awake()
    {
        if(Instance == null)
        Instance = this;
    }
    
    public void ChangeParty(List<ElectionsDatabase.Election.Party> newParty)
    {

    }
}
