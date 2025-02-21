using UnityEngine;
using System.IO;
using System.Collections;
using System.Collections.Generic;

public class DataHandler : MonoBehaviour
{
    public static DataHandler Instance { get; private set; }

    public PlayerData[] playerData = new PlayerData[4];
    private string saveDirectory;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);

            // Set up the save directory
            saveDirectory = Path.Combine(Application.persistentDataPath, "Saves");
            Directory.CreateDirectory(saveDirectory); // Create the directory if it doesn't exist

            Debug.Log($"DataHandler initialized. Save directory: {saveDirectory}");

            // Initialize player data array
            for (int i = 0; i < playerData.Length; i++)
            {
                if (playerData[i] == null)
                {
                    playerData[i] = new PlayerData();
                    playerData[i].slotNumber = i;
                }
            }
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        // Try to load existing save data
        LoadAllSaves();
    }

    private string GetSaveFilePath(int slotNumber)
    {
        return Path.Combine(saveDirectory, $"player_data_{slotNumber}.json");
    }

    public void UpdateAndSavePlayerData(int slotNumber, GameData gameData)
    {
        if (slotNumber >= 0 && slotNumber < playerData.Length)
        {
            if (playerData[slotNumber] == null)
            {
                playerData[slotNumber] = new PlayerData();
                playerData[slotNumber].slotNumber = slotNumber;
            }

            // Map GameData values to PlayerData
            playerData[slotNumber].bits = gameData.currentCoins;
            playerData[slotNumber].timePlayed += Time.timeSinceLevelLoad;
            playerData[slotNumber].numCharactersUnlocked = gameData.numCharactersUnlocked;
            playerData[slotNumber].charactersUnlocked = new List<string>(gameData.charactersUnlocked);

            // Save track data
            foreach (var track in gameData.trackData)
            {
                // Update track data in the player data
                playerData[slotNumber].UpdateTrackData(track.Key, track.Value);
            }

            SaveGame(slotNumber);
        }
        else
        {
            Debug.LogError($"Invalid slot number: {slotNumber}");
        }
    }

    public void SaveGame(int slotNumber)
    {
        if (slotNumber >= 0 && slotNumber < playerData.Length && playerData[slotNumber] != null)
        {
            string savePath = GetSaveFilePath(slotNumber);
            string jsonData = JsonUtility.ToJson(playerData[slotNumber], true); // true for pretty print

            try
            {
                File.WriteAllText(savePath, jsonData);
                Debug.Log($"Game saved successfully to slot {slotNumber} at: {savePath}");

                // Save track data for the current slot
                foreach (var track in playerData[slotNumber].trackData)
                {
                    // Update track data in GameData only for the active slot
                    GameData.Instance.trackData[track.Key] = track.Value;
                }
            }
            catch (System.Exception e)
            {
                Debug.LogError($"Failed to save game to slot {slotNumber}: {e.Message}");
            }
        }
    }

    public void DeleteGame(int slotNumber)
    {
        // Check if the slot number is valid
        if (slotNumber < 0 || slotNumber >= playerData.Length)
        {
            Debug.LogError($"Invalid slot number: {slotNumber}");
            return;
        }

        // Check if the save slot is empty
        if (IsSaveSlotEmpty(slotNumber))
        {
            Debug.Log($"Save slot {slotNumber} is already empty.");
            return;
        }

        // Delete the save file
        string savePath = GetSaveFilePath(slotNumber);
        try
        {
            File.Delete(savePath);
            Debug.Log($"Save file for slot {slotNumber} deleted successfully.");

            // Reset the in-memory data for the slot
            playerData[slotNumber] = new PlayerData { slotNumber = slotNumber };
        }
        catch (System.Exception e)
        {
            Debug.LogError($"Failed to delete save file for slot {slotNumber}: {e.Message}");
        }
    }

    public void LoadGame(int slotNumber)
    {
        string savePath = GetSaveFilePath(slotNumber);

        if (File.Exists(savePath))
        {
            try
            {
                string jsonData = File.ReadAllText(savePath);
                playerData[slotNumber] = JsonUtility.FromJson<PlayerData>(jsonData);

                if (GameData.Instance != null)
                {
                    GameData.Instance.activeSaveSlot = slotNumber;
                    GameData.Instance.currentCoins = playerData[slotNumber].bits;
                    GameData.Instance.timePlayed = playerData[slotNumber].timePlayed;

                    // Add default characters and vehicles if missing
                    foreach (var characterId in GameData.Instance.charactersUnlocked)
                    {
                        if (!playerData[slotNumber].charactersUnlocked.Contains(characterId))
                        {
                            playerData[slotNumber].charactersUnlocked.Add(characterId);
                        }
                    }

                    GameData.Instance.trackData.Clear();
                    foreach (var track in playerData[slotNumber].trackData)
                    {
                        GameData.Instance.trackData[track.Key] = track.Value;
                    }

                    // Synchronize unlocked vehicles and characters
                    GameData.Instance.SynchronizeDataWithSave();

                    // Sync the managers
                    CharacterManager.Instance.SyncUnlockedCharacters();

                    Debug.Log($"Successfully loaded save data from slot {slotNumber}");
                }
                else
                {
                    Debug.LogWarning("GameData.Instance is null. Unable to update game state.");
                }
            }
            catch (System.Exception e)
            {
                Debug.LogError($"Failed to load game from slot {slotNumber}: {e.Message}");
            }
        }
        else
        {
            Debug.Log($"No save file found for slot {slotNumber}.");
            if(playerData[slotNumber] != null)
            {
                playerData[slotNumber] = new PlayerData();
                playerData[slotNumber].slotNumber = slotNumber;

                // Add default data if no save exists
                GameData.Instance?.InitializeDefaults();
                playerData[slotNumber].charactersUnlocked = new List<string>(GameData.Instance.charactersUnlocked);
                playerData[slotNumber].numCharactersUnlocked = GameData.Instance.numCharactersUnlocked;

                // Ensure trackData is cleared for the new slot
                playerData[slotNumber].trackData.Clear();
            }
        }
    }

    public void LoadAllSaves()
    {
        for (int i = 0; i < playerData.Length; i++)
        {
            LoadGame(i);
        }
    }

    public bool IsSaveSlotEmpty(int slotNumber)
    {
        return !File.Exists(GetSaveFilePath(slotNumber));
    }
}