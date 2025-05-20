using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

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
        if (electionData.parties.Count > 0)
        {
            UpdatePartyUI();
        }
    }

    public void Load(SaveData saveData)
    {
        electionData = EventsManager.Instance.ElectionsDatabase.elections[saveData.selectedElectionIndex];

        UpdateElectionUI();
        if (electionData.parties.Count > 0)
        {
            UpdatePartyUI();
        }
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
        UpdatePartyUI();
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

    private void UpdatePartyUI()
    {
        foreach (Transform child in partyListContainer)
        {
            Destroy(child.gameObject);
        }

        foreach (var party in electionData.parties)
        {
            GameObject obj = Instantiate(partyItemPrefab, partyListContainer);
            TextMeshProUGUI label = obj.GetComponentInChildren<TextMeshProUGUI>();

            if (label != null)
            {
                label.text = $"{party.partyName} - {party.seats} seats";
            }
        }
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
