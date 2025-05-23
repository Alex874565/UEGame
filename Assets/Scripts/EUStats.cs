using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

public class EUStats : MonoBehaviour
{
    public static EUStats Instance;

    [Header("Stats UI")]
    [SerializeField] private GameObject statsPanel;

    [Header("Map UI")]
    [SerializeField] private Image mapImage;

    [Header("Election Info UI")]
    [SerializeField] private TMP_Text comissionPresident;
    [SerializeField] private TMP_Text parliamentPresident;
    [SerializeField] private TMP_Text councilPresident;

    [Header("Party Stats UI")]
    [SerializeField] private Transform partyListContainer;
    [SerializeField] private GameObject partyItemPrefab;

    private ElectionsDatabase.Election electionData = new();

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    private void Start()
    {
        UpdateElectionUI();
        UpdatePartyUI(electionData.parties);
    }

    public void Load(SaveData saveData)
    {
        electionData = EventsManager.Instance.ElectionsDatabase.elections[saveData.selectedElectionIndex];

        UpdateElectionUI();
        UpdatePartyUI(saveData.savedParties);
    }

    public void ChangeParty(ElectionsDatabase.Election newParty)
    {
        electionData = newParty;
        if (newParty == null)
        {
            Debug.LogWarning("ChangeParty called with empty or null list.");
            return;
        }

        UpdateElectionUI();
        UpdatePartyUI(newParty.parties);
    }

    public void UpdateMap(Sprite newMap)
    {
        mapImage.sprite = newMap;
    }
    
    private void UpdateElectionUI()
    {
        if (electionData == null) return;

        if (electionData.institution == ElectionsDatabase.Election.InstitutionType.Comission)
        {
            comissionPresident.text = electionData.president;
        }
        else if (electionData.institution == ElectionsDatabase.Election.InstitutionType.EuropeanCouncil)
        {
            councilPresident.text = electionData.president;
        }
        else if (electionData.institution == ElectionsDatabase.Election.InstitutionType.Parliament)
        {
            if (electionData.president != "")
            {
                parliamentPresident.text = electionData.president;
            }
        }
    }

    private void UpdatePartyUI(List<ElectionsDatabase.Party> partiesList)
    {
        if(partiesList == null || partiesList.Count == 0)
        {
            Debug.LogWarning("UpdatePartyUI called with empty or null list.");
            return;
        }
        foreach (Transform child in partyListContainer)
        {
            Destroy(child.gameObject);
        }

        foreach (var party in partiesList)
        {
            GameObject obj = Instantiate(partyItemPrefab, partyListContainer);
            TextMeshProUGUI label = obj.GetComponentInChildren<TextMeshProUGUI>();

            if (label != null)
            {
                label.text = $"{party.partyName} - {party.seats} seats";
            }
        }
    }

    public ElectionsDatabase.Election GetPreviousElection()
    {
        if (electionData == null)
        {
            Debug.LogWarning("Previous election called with empty or null list.");
            return null;
        }
        int currentIndex = EventsManager.Instance.ElectionsDatabase.elections.IndexOf(electionData);
        if (currentIndex > 0)
        {
            electionData = EventsManager.Instance.ElectionsDatabase.elections[currentIndex - 1];
        }
        else
        {
            electionData = EventsManager.Instance.ElectionsDatabase.elections[0];
        }
        return electionData;
    }

    public ElectionsDatabase.Election GetCurrentElection()
    {
        return electionData;
    }

    public void OpenStatsPanel()
    {
        TimeManager.Instance.PauseTime();
        statsPanel.SetActive(true);
    }
}
