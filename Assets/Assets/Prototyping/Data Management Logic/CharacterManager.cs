using UnityEngine;
using System.Collections.Generic;
using System.Collections;

public class CharacterManager : MonoBehaviour
{
    public static CharacterManager Instance { get; private set; }

    public List<Player> allCharacters = new List<Player>(); // All available characters
    public List<Player> unlockedCharacters = new List<Player>(); // List of unlocked characters

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public bool IsCharacterUnlocked(string characterId)
    {
        return GameData.Instance.charactersUnlocked.Contains(characterId);
    }

    public void SyncUnlockedCharacters()
    {
        unlockedCharacters.Clear();
        foreach (string characterId in GameData.Instance.charactersUnlocked)
        {
            Player player = allCharacters.Find(c => c.id == characterId);
            if (player != null)
            {
                unlockedCharacters.Add(player);
                player.isUnlocked = true;
            }
        }
    }

    public void UnlockCharacter(string characterId)
    {
        if (GameData.Instance.charactersUnlocked.Contains(characterId)) return;
        
        Player player = allCharacters.Find(c => c.id == characterId);
        if(player != null)
        {
            GameData.Instance.UnlockCharacter(characterId);
            unlockedCharacters.Add(player);
            player.isUnlocked = true;
            DataHandler.Instance.UpdateAndSavePlayerData(GameData.Instance.activeSaveSlot, GameData.Instance);
        }
    }
}
