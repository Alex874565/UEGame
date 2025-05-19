using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class EUStats : MonoBehaviour
{
    public static EUStats Instance;

    [Header("Map UI")]
    [SerializeField] private Image mapImage;

    [Header("Election Info UI")]
    [SerializeField] private TMP_Text titleText;
    [SerializeField] private TMP_Text countryText;
    [SerializeField] private TMP_Text institutionText;
    [SerializeField] private TMP_Text dateText;
    [SerializeField] private TMP_Text presidentText;
    [SerializeField] private TMP_Text descriptionText;

    [Header("Party Stats UI")]
    [SerializeField] private Transform partyListContainer;
    [SerializeField] private GameObject partyItemPrefab;

    private ElectionsDatabase.Election currentElection;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    public void Load(SaveData saveData)
    {
        currentElection = EventsManager.Instance.ElectionsDatabase.elections[saveData.selectedElectionIndex];

        UpdateElectionUI();
        UpdatePartyUI();
    }

    public void ChangeParty(ElectionsDatabase.Election newParty)
    {
        currentElection = newParty;
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
        if (currentElection == null) return;

        titleText.text = currentElection.title;
        countryText.text = currentElection.countryName;
        institutionText.text = currentElection.institution.ToString();
        dateText.text = currentElection.electionDate;
        presidentText.text = currentElection.president;
        descriptionText.text = currentElection.description;
    }

    private void UpdatePartyUI()
    {
        foreach (Transform child in partyListContainer)
        {
            Destroy(child.gameObject);
        }

        foreach (var party in currentElection.parties)
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
        return currentElection;
    }
}
