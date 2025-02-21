using UnityEngine;
using TMPro;

public class DeleteSlot : MonoBehaviour
{
    public int slotNumber;
    public TextMeshProUGUI deleteStatusText;
    public TextMeshProUGUI deleteDetailsText;

    //when the button is pressed, delete the save file by calling the DeleteSave method in Datahandler

    public void OnClickSaveSlot()
    {
        if (DataHandler.Instance != null)
        {
            // Call the delete method in DataHandler
            DataHandler.Instance.DeleteGame(slotNumber);

            // Update the UI to notify the user
            if (DataHandler.Instance.IsSaveSlotEmpty(slotNumber))
            {
                deleteStatusText.text = $"Slot {slotNumber} deleted successfully.";
                RefreshSlotDetails();
            }
            else
            {
                deleteStatusText.text = $"Failed to delete slot {slotNumber}.";
            }
        }
        else
        {
            Debug.LogError("DataHandler instance is null.");
            deleteStatusText.text = "Error: Unable to delete save.";
        }
    }

    public void RefreshSlotDetails()
    {
        Debug.Log($"SaveSlot {slotNumber} - Refreshing slot details");
        if (DataHandler.Instance == null)
        {
            Debug.LogError($"SaveSlot {slotNumber} - DataHandler.Instance is null in RefreshSlotDetails");
            return;
        }

        DataHandler.Instance.LoadAllSaves();
        DisplaySlotDetails();
    }

    public void DisplaySlotDetails()
    {
        if (deleteDetailsText == null)
        {
            Debug.LogError($"SaveSlot {slotNumber} - saveDetailsText is null in DisplaySlotDetails");
            return;
        }

        if (DataHandler.Instance == null)
        {
            Debug.LogError($"SaveSlot {slotNumber} - DataHandler.Instance is null in DisplaySlotDetails");
            deleteDetailsText.text = "Error: Save system not initialized";
            return;
        }

        try
        {
            if (DataHandler.Instance.IsSaveSlotEmpty(slotNumber))
            {
                deleteDetailsText.text = "No save data exists";
            }
            else
            {
                var saveData = DataHandler.Instance.playerData[slotNumber];
                if (saveData != null)
                {
                    deleteDetailsText.text = $"Name: {saveData.profileName}\n" +
                                         $"Total Bits: {saveData.bits}\n" +
                                         $"Time Played: {FormatTime(saveData.timePlayed)}";
                }
                else
                {
                    Debug.LogError($"SaveSlot {slotNumber} - PlayerData is null for this slot");
                    deleteDetailsText.text = "Error: Save data corrupted";
                }
            }
        }
        catch (System.Exception e)
        {
            Debug.LogError($"SaveSlot {slotNumber} - Exception in DisplaySlotDetails: {e}");
            deleteDetailsText.text = "Error displaying save details";
        }
    }

    private string FormatTime(float timeInSeconds)
    {
        int hours = Mathf.FloorToInt(timeInSeconds / 3600);
        int minutes = Mathf.FloorToInt((timeInSeconds % 3600) / 60);
        return $"{hours:D2}:{minutes:D2}";
    }
}
