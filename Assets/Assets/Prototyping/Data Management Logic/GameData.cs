using UnityEngine;
using System.Collections.Generic;


public class GameData : MonoBehaviour
{
    public static GameData Instance { get; private set; }

    public string selectedScene; // Scene to load
    public string nextCampaignScene; //Set the next campaign scene to load (mainly here for cutscenes and to exit the shop)
    public int activeSaveSlot = -1; // Currently active save slot
    public int currentCoins; // Player's current coins
    public float timePlayed;
    public int numVehiclesUnlocked; // Number of unlocked vehicles
    public int numCharactersUnlocked;
    public bool isCampaign = false; // Is player currently playing the campaign
    public bool isArcade = false;   // Is player currently playing the arcade mode

    //track data
    public string currentTrack; // e.g., "TutorialTrack", etc.
    public Dictionary<string, LevelData> trackData = new Dictionary<string, LevelData>();

    //vehicle and character data
    public List<string> vehiclesUnlocked = new List<string>();
    public List<string> charactersUnlocked = new List<string>();

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // Persist across scenes

            if (DataHandler.Instance == null || DataHandler.Instance.IsSaveSlotEmpty(activeSaveSlot))
            {
                InitializeDefaults();
            }

            Cursor.visible = false;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void InitializeDefaults()
    {
        // Add default character IDs
        charactersUnlocked = new List<string>
        {
            "Neo",  // Add new IDs for other default characters (Max, Ally, etc.)
            "Ember",
            "Max",
            "Chase",
            "Ally",
            "Indigo"
        };

        // Add default vehicle IDs
        vehiclesUnlocked = new List<string>
        {
            "Neo",  // Add new IDs for other default vehicles (Max, Ally, etc.)
            "Ember",
            "Max",
            "Chase",
            "Ally",
            "Indigo"
        };

        //change values once we know how many characters are unlocked by default
        numCharactersUnlocked = charactersUnlocked.Count;
        numVehiclesUnlocked = vehiclesUnlocked.Count;

        // Ensure total values match the unlocked counts
        UpdateTotalCounts();
    }

    public void SynchronizeDataWithSave()
    {
        if (activeSaveSlot < 0 || DataHandler.Instance == null || DataHandler.Instance.playerData[activeSaveSlot] == null)
        {
            Debug.LogWarning("No valid save slot or DataHandler instance to synchronize with.");
            return;
        }

        // Fetch the PlayerData for the active save slot
        PlayerData saveData = DataHandler.Instance.playerData[activeSaveSlot];

        // Synchronize characters
        foreach (string characterId in saveData.charactersUnlocked)
        {
            if (!charactersUnlocked.Contains(characterId))
            {
                charactersUnlocked.Add(characterId);
            }
        }
        foreach (string characterId in charactersUnlocked)
        {
            if (!saveData.charactersUnlocked.Contains(characterId))
            {
                saveData.charactersUnlocked.Add(characterId);
            }
        }

        // Update counts
        numVehiclesUnlocked = vehiclesUnlocked.Count;
        numCharactersUnlocked = charactersUnlocked.Count;

        UpdateTotalCounts();

        Debug.Log("GameData synchronized with save data.");
    }


    // Explicitly update GameData values
    public void UpdateDataFromSave()
    {
        Debug.Log($"Updating GameData: Coins={currentCoins}, Time Played={timePlayed}, Vehicles Unlocked={vehiclesUnlocked}");
    }

    public void UnlockVehicle(string vehicleId)
    {
        if (!vehiclesUnlocked.Contains(vehicleId))
        {
            vehiclesUnlocked.Add(vehicleId);
            numVehiclesUnlocked++;
            UpdateTotalCounts();
        }
    }

    public void UnlockCharacter(string characterId) {  
        if (!charactersUnlocked.Contains(characterId)) 
        { 
            charactersUnlocked.Add(characterId);
            numCharactersUnlocked++;
            UpdateTotalCounts();
        } 
    }

    private void UpdateTotalCounts()
    {
        if (activeSaveSlot < 0 || DataHandler.Instance == null || DataHandler.Instance.playerData[activeSaveSlot] == null)
        {
            Debug.LogWarning("No valid save slot or PlayerData instance to update totals.");
            return;
        }

        PlayerData saveData = DataHandler.Instance.playerData[activeSaveSlot];
    }
}
