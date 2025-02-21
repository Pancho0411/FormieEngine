using UnityEngine;
using System.Collections.Generic;

[System.Serializable]
public class PlayerData : ISerializationCallbackReceiver
{
    public string profileName;
    public int slotNumber;
    public int bits;
    public float timePlayed;
    public int numCharactersUnlocked;
    public List<string> charactersUnlocked = new List<string>();

    // Dictionary to store level data
    [System.NonSerialized]
    public Dictionary<string, LevelData> trackData = new Dictionary<string, LevelData>();

    // Serialization fields for trackData (key-value pairs)
    public List<string> trackKeys = new List<string>();
    public List<LevelData> trackValues = new List<LevelData>();

    // Serialize the dictionary into two lists: trackKeys and trackValues
    public void OnBeforeSerialize()
    {
        trackKeys.Clear();
        trackValues.Clear();
        foreach (var kvp in trackData)
        {
            trackKeys.Add(kvp.Key);
            trackValues.Add(kvp.Value);
        }
    }

    // Deserialize the lists back into the dictionary
    public void OnAfterDeserialize()
    {
        trackData = new Dictionary<string, LevelData>();
        for (int i = 0; i < trackKeys.Count; i++)
        {
            trackData[trackKeys[i]] = trackValues[i];
        }
    }

    public void UpdateTrackData(string trackName, LevelData track)
    {
        // Update or add the track data in the dictionary
        if (this.trackData.ContainsKey(trackName))
        {
            this.trackData[trackName] = track;
        }
        else
        {
            this.trackData.Add(trackName, track);
        }
    }
}
