using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EUStats : MonoBehaviour
{
    public static EUStats Instance;

    [SerializeField] private Image mapImage;

    private void Awake()
    {
        if(Instance == null)
        Instance = this;
    }
    
    public void ChangeParty(List<ElectionsDatabase.Election.Party> newParty)
    {

    }

    public void ChangeMap(Sprite newMap)
    {
        mapImage.sprite = newMap;
    }
}
