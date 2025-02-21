using UnityEngine;
using TMPro;

public class SaveSlot : MonoBehaviour
{
    public int slotNumber;
    public TMP_InputField nameInputField;
    public TextMeshProUGUI saveStatusText;
    public TextMeshProUGUI saveDetailsText;
    public GameObject onScreenKeyboard;
    public MainMenu mainMenu;
    public OnscreenKeyboard keyboardScript;

    private void Awake()
    {
        //Debug.Log($"SaveSlot {slotNumber} Awake - Starting component verification");

        // Verify all required components
        ValidateComponents();

        // Find MainMenu if not assigned
        if (mainMenu == null)
        {
            mainMenu = FindAnyObjectByType<MainMenu>();
            //Debug.Log($"SaveSlot {slotNumber} - MainMenu {(mainMenu != null ? "found" : "not found")}");
        }

        // Set up keyboard reference dynamically if onScreenKeyboard is the parent object
        if (onScreenKeyboard != null)
        {
            // Find the OnscreenKeyboard component in the children of the passed GameObject
            keyboardScript = onScreenKeyboard.GetComponentInChildren<OnscreenKeyboard>();
            if (keyboardScript != null)
            {
                keyboardScript.currentSaveSlot = this;
                //Debug.Log($"SaveSlot {slotNumber} - Keyboard script setup successful");
            }
            else
            {
                Debug.LogError($"SaveSlot {slotNumber} - OnscreenKeyboard component missing from keyboard GameObject or its children");
            }
        }
        else
        {
            Debug.LogError($"SaveSlot {slotNumber} - onScreenKeyboard reference is null");
        }
    }

    private void ValidateComponents()
    {
        if (nameInputField == null)
            Debug.LogError($"SaveSlot {slotNumber} - nameInputField is not assigned");
        if (saveStatusText == null)
            Debug.LogError($"SaveSlot {slotNumber} - saveStatusText is not assigned");
        if (saveDetailsText == null)
            Debug.LogError($"SaveSlot {slotNumber} - saveDetailsText is not assigned");
        if (onScreenKeyboard == null)
            Debug.LogError($"SaveSlot {slotNumber} - onScreenKeyboard is not assigned");
    }

    private void Start()
    {
        //Debug.Log($"SaveSlot {slotNumber} Start - Refreshing slot details");
        if (DataHandler.Instance == null)
        {
            Debug.LogError($"SaveSlot {slotNumber} - DataHandler.Instance is null in Start");
        }
        RefreshSlotDetails();
    }

    public void OnClickSaveSlot()
    {
        //Debug.Log($"SaveSlot {slotNumber} clicked - Starting save slot interaction");
        keyboardScript.currentSaveSlot = this;

        if (DataHandler.Instance == null)
        {
            //Debug.LogError($"SaveSlot {slotNumber} - DataHandler.Instance is null in OnClickSaveSlot");
            ShowSaveStatus("Error: Save system not initialized");
            return;
        }

        try
        {
            bool isEmpty = DataHandler.Instance.IsSaveSlotEmpty(slotNumber);
            //Debug.Log($"SaveSlot {slotNumber} - Slot is {(isEmpty ? "empty" : "occupied")}");

            if (isEmpty)
            {
                // Show new save creation UI
                if (nameInputField != null)
                {
                    nameInputField.gameObject.SetActive(true);
                    nameInputField.text = "";
                }
                else
                {
                    //Debug.LogError($"SaveSlot {slotNumber} - nameInputField is null");
                    return;
                }

                if (saveStatusText != null)
                {
                    saveStatusText.gameObject.SetActive(true);
                    saveStatusText.text = "Enter a name for this save slot";
                }
                else
                {
                    //Debug.LogError($"SaveSlot {slotNumber} - saveStatusText is null");
                    return;
                }

                if (onScreenKeyboard != null && keyboardScript != null)
                {
                    onScreenKeyboard.SetActive(true);
                    keyboardScript.inputField = nameInputField;
                    //Debug.Log($"SaveSlot {slotNumber} - Keyboard activated");
                }
                else
                {
                    Debug.LogError($"SaveSlot {slotNumber} - Keyboard setup incomplete. onScreenKeyboard: {onScreenKeyboard != null}, keyboardScript: {keyboardScript != null}");
                }
            }
            else
            {
                // Load existing save
                DataHandler.Instance.LoadGame(slotNumber);

                if (GameData.Instance != null)
                {
                    GameData.Instance.UpdateDataFromSave();
                    //Debug.Log($"SaveSlot {slotNumber} - Existing save loaded successfully");
                }
                else
                {
                    Debug.LogError($"SaveSlot {slotNumber} - GameData.Instance is null");
                }

                ShowSaveStatus("Loading save data...");
                TriggerMainMenuTransition();
            }
        }
        catch (System.Exception e)
        {
            Debug.LogError($"SaveSlot {slotNumber} - Exception in OnClickSaveSlot: {e}");
            ShowSaveStatus("Error occurred while processing save");
        }
    }

    private void ShowSaveStatus(string message)
    {
        if (saveStatusText != null)
        {
            saveStatusText.text = message;
            saveStatusText.gameObject.SetActive(true);
            //Debug.Log($"SaveSlot {slotNumber} - Status message: {message}");
            Invoke(nameof(ClearSaveStatus), 2f);
        }
        else
        {
            Debug.LogError($"SaveSlot {slotNumber} - Cannot show status: saveStatusText is null");
        }
    }

    private void ClearSaveStatus()
    {
        if (saveStatusText != null)
        {
            saveStatusText.text = "";
            saveStatusText.gameObject.SetActive(false);
        }
    }

    public void RefreshSlotDetails()
    {
        //Debug.Log($"SaveSlot {slotNumber} - Refreshing slot details");
        if (DataHandler.Instance == null)
        {
            //Debug.LogError($"SaveSlot {slotNumber} - DataHandler.Instance is null in RefreshSlotDetails");
            return;
        }

        DataHandler.Instance.LoadAllSaves();
        DisplaySlotDetails();
    }

    public void DisplaySlotDetails()
    {
        if (saveDetailsText == null)
        {
            //Debug.LogError($"SaveSlot {slotNumber} - saveDetailsText is null in DisplaySlotDetails");
            return;
        }

        if (DataHandler.Instance == null)
        {
            //Debug.LogError($"SaveSlot {slotNumber} - DataHandler.Instance is null in DisplaySlotDetails");
            saveDetailsText.text = "Error: Save system not initialized";
            return;
        }

        try
        {
            if (DataHandler.Instance.IsSaveSlotEmpty(slotNumber))
            {
                saveDetailsText.text = "No save data exists";
            }
            else
            {
                var saveData = DataHandler.Instance.playerData[slotNumber];
                if (saveData != null)
                {
                    saveDetailsText.text = $"Name: {saveData.profileName}\n" +
                                         $"Total Bits: {saveData.bits}\n" +
                                         $"Time Played: {FormatTime(saveData.timePlayed)}";
                }
                else
                {
                    //Debug.LogError($"SaveSlot {slotNumber} - PlayerData is null for this slot");
                    saveDetailsText.text = "Error: Save data corrupted";
                }
            }
        }
        catch (System.Exception e)
        {
            Debug.LogError($"SaveSlot {slotNumber} - Exception in DisplaySlotDetails: {e}");
            saveDetailsText.text = "Error displaying save details";
        }
    }

    private string FormatTime(float timeInSeconds)
    {
        int hours = Mathf.FloorToInt(timeInSeconds / 3600);
        int minutes = Mathf.FloorToInt((timeInSeconds % 3600) / 60);
        return $"{hours:D2}:{minutes:D2}";
    }

    public void TriggerMainMenuTransition()
    {
        //Debug.Log($"SaveSlot {slotNumber} - Triggering main menu transition");
        if (mainMenu != null)
        {
            mainMenu.EnableMainMenu();
        }
        else
        {
            Debug.LogError($"SaveSlot {slotNumber} - mainMenu reference is null");
        }
    }

    public void SaveNewGame(string profileName)
    {
        //Debug.Log($"SaveSlot {slotNumber} - Creating new save with name: {profileName}");

        if (DataHandler.Instance == null)
        {
            //Debug.LogError($"SaveSlot {slotNumber} - DataHandler.Instance is null in SaveNewGame");
            return;
        }

        try
        {
            var newSaveData = new PlayerData
            {
                profileName = profileName,
                slotNumber = slotNumber,
                bits = 0,
                timePlayed = 0f,
            };

            DataHandler.Instance.playerData[slotNumber] = newSaveData;
            DataHandler.Instance.SaveGame(slotNumber);

            if (GameData.Instance != null)
            {
                GameData.Instance.activeSaveSlot = slotNumber;
                GameData.Instance.UpdateDataFromSave();
                //Debug.Log($"SaveSlot {slotNumber} - GameData updated successfully");
            }
            else
            {
                Debug.LogError($"SaveSlot {slotNumber} - GameData.Instance is null in SaveNewGame");
            }

            DisplaySlotDetails();
            ShowSaveStatus("Save created successfully!");

            // Hide UI elements
            if (nameInputField != null) nameInputField.gameObject.SetActive(false);
            if (saveStatusText != null) saveStatusText.gameObject.SetActive(false);
            if (onScreenKeyboard != null) onScreenKeyboard.SetActive(false);

            TriggerMainMenuTransition();
        }
        catch (System.Exception e)
        {
            Debug.LogError($"SaveSlot {slotNumber} - Exception in SaveNewGame: {e}");
            ShowSaveStatus("Error creating save");
        }
    }
}