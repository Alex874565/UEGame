using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using UnityEngine;
using UnityEngine.Playables;

[Serializable]
public class SaveData
{
    public int selectedElectionIndex;
    public List<ElectionsDatabase.Party> savedParties = new();

    public bool finishedTutorial;

    // Events manager variables
    public int eventIndex;
    public int budgetIndex;
    public int quizIndex;
    public int electionIndex;
    public int membersIndex;

    // Time variables
    public int day;
    public int month;
    public int year;
    public int hour;
    public int minute;

    // Resource manager variables
    public int quizzesFailed;
    public float foreignAffair;
    public float euroscepticism;
    public long budget;
}

public class SaveManager : MonoBehaviour
{
    public static SaveManager Instance;
    public SaveData currentData;

    private string SavePath;

    private static readonly byte[] Key = Encoding.UTF8.GetBytes("ThisIsASecretKey1234567890123456");
    private static readonly byte[] IV = Encoding.UTF8.GetBytes("ThisIsAnIV123456");

    [SerializeField] private GameObject tutorialGO;
    [SerializeField] private TimeManager timeManager;

    private bool lostGame = false;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;

        SavePath = Path.Combine(Application.persistentDataPath, "save.dat");
    }

    private void Start()
    {
        LoadGame();
    }

    public void DeleteSave()
    {
        if (File.Exists(SavePath))
        {
            File.Delete(SavePath);
            Debug.Log("Save file deleted.");
        }
        else
        {
            Debug.Log("No save file to delete.");
        }
    }
    
    public void SaveGame()
    {
        if (lostGame) return;

        // Time
        currentData.day = TimeManager.Instance.GetDay();
        currentData.month = TimeManager.Instance.GetMonth();
        currentData.year = TimeManager.Instance.GetYear();
        currentData.hour = TimeManager.Instance.GetHour();
        currentData.minute = TimeManager.Instance.GetMinute();

        // Resources
        currentData.budget = ResourceManager.Instance.GetCurrentBudget();
        currentData.quizzesFailed = ResourceManager.Instance.GetCurrentQuizFails();
        currentData.foreignAffair = ResourceManager.Instance.GetCurrentForeignAffairs();
        currentData.euroscepticism = ResourceManager.Instance.GetCurrentEurosceptisism();

        // Events
        currentData.eventIndex = EventsManager.Instance.EventIndex;
        currentData.budgetIndex = EventsManager.Instance.BudgetIndex;
        currentData.quizIndex = EventsManager.Instance.QuizIndex;
        currentData.electionIndex = EventsManager.Instance.ElectionIndex;
        currentData.membersIndex = EventsManager.Instance.MembersIndex;


        var election = EUStats.Instance.GetCurrentElection();
        // EU Stats
        if (currentData.savedParties == null)
        {
            currentData.savedParties = new List<ElectionsDatabase.Party>();
        }
        else if (election.parties.Count != 0)
        {
            currentData.savedParties.Clear();
            foreach (var party in election.parties)
            {
                currentData.savedParties.Add(new ElectionsDatabase.Party
                {
                    partyName = party.partyName,
                    seats = party.seats
                });
            }
        }
        
        currentData.selectedElectionIndex = EventsManager.Instance.ElectionIndex;

        string json = JsonUtility.ToJson(currentData);
        string encrypted = EncryptString(json);
        File.WriteAllText(SavePath, encrypted);
        Debug.Log("Game Saved");
    }

    public void LoadGame()
    {
        if (File.Exists(SavePath))
        {
            try
            {
                string encrypted = File.ReadAllText(SavePath);
                string json = DecryptString(encrypted);
                currentData = JsonUtility.FromJson<SaveData>(json);

                // If we have a load file, then we want to skip the tutorial
                // and acitivate scritp sof interet
                if (currentData.finishedTutorial)
                {
                    tutorialGO.SetActive(false);
                }

                timeManager.gameObject.SetActive(true);
                timeManager.Load(currentData);

                ResourceManager.Instance.Load(currentData);
                EUStats.Instance.Load(currentData);
            }
            catch (Exception ex)
            {
                Debug.LogError("Failed to load save: " + ex.Message);
                currentData = CreateNewData();
                TimeManager.Instance.gameObject.SetActive(false);
            }
        }
        else
        {
            currentData = CreateNewData();
            timeManager.gameObject.SetActive(false);
        }

        // Can be loaded at any time
        EventsManager.Instance.Load(currentData);
    }

    public void LostGame()
    {
        lostGame = true;
    }

    private SaveData CreateNewData()
    {
        return new SaveData
        {
            finishedTutorial = false,
            day = 1,
            month = 1,
            year = 2003,
            hour = 0,
            minute = 0,
            budget = 0,
            quizzesFailed = 0,
            foreignAffair = 0f,
            euroscepticism = 0f,
        };
    }

    private string EncryptString(string plainText)
    {
        using Aes aes = Aes.Create();
        aes.Key = Key;
        aes.IV = IV;

        ICryptoTransform encryptor = aes.CreateEncryptor(aes.Key, aes.IV);

        using MemoryStream ms = new MemoryStream();
        using CryptoStream cs = new CryptoStream(ms, encryptor, CryptoStreamMode.Write);
        using (StreamWriter sw = new StreamWriter(cs))
        {
            sw.Write(plainText);
        }

        return Convert.ToBase64String(ms.ToArray());
    }

    private string DecryptString(string cipherText)
    {
        using Aes aes = Aes.Create();
        aes.Key = Key;
        aes.IV = IV;

        ICryptoTransform decryptor = aes.CreateDecryptor(aes.Key, aes.IV);

        byte[] buffer = Convert.FromBase64String(cipherText);
        using MemoryStream ms = new MemoryStream(buffer);
        using CryptoStream cs = new CryptoStream(ms, decryptor, CryptoStreamMode.Read);
        using StreamReader sr = new StreamReader(cs);

        return sr.ReadToEnd();
    }
}
