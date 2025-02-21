using UnityEngine;
using System.Collections.Generic;

public class GameManager : MonoBehaviour
{
    public static GameManager instance; // Singleton instance

    public class PlayerSelectionData
    {
        public GameObject characterPrefab;
        public GameObject vehiclePrefab;

        public PlayerSelectionData(GameObject character, GameObject vehicle)
        {
            characterPrefab = character;
            vehiclePrefab = vehicle;
        }
    }

    private Dictionary<int, PlayerSelectionData> playerSelections = new Dictionary<int, PlayerSelectionData>(); // Store selections by player index

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject); // Keep GameManager persistent across scenes
        }
        else if (instance != this)
        {
            Destroy(gameObject);
        }
    }

    public void StorePlayerSelection(int playerIndex, GameObject character, GameObject vehicle)
    {
        if (!playerSelections.ContainsKey(playerIndex))
        {
            playerSelections[playerIndex] = new PlayerSelectionData(character, vehicle);
            Debug.Log($"Stored selection for Player {playerIndex + 1}: Character - {character.name}, Vehicle - {vehicle.name}");
        }
        else
        {
            Debug.LogWarning($"Player {playerIndex + 1} has already selected: Character - {playerSelections[playerIndex].characterPrefab.name}, Vehicle - {playerSelections[playerIndex].vehiclePrefab.name}");
        }
    }

    public PlayerSelectionData GetPlayerSelection(int playerIndex)
    {
        if (playerSelections.TryGetValue(playerIndex, out PlayerSelectionData selectionData))
        {
            return selectionData;
        }
        return null;
    }
}
