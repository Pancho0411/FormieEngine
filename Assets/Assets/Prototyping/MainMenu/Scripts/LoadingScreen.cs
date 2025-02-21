using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LoadingScreen : MonoBehaviour
{
    public Slider loadingBar;

    private void Start()
    {
        if (string.IsNullOrEmpty(GameData.Instance.selectedScene))
        {
            Debug.LogError("No scene selected to load.");
            return; // Do not proceed if no scene is set
        }

        StartCoroutine(LoadSelectedScene());
    }

    private IEnumerator LoadSelectedScene()
    {
        // Save player data before loading the next scene
        SavePlayerData();

        string sceneToLoad = GameData.Instance.selectedScene;

        if (!string.IsNullOrEmpty(sceneToLoad))
        {
            AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(sceneToLoad);

            while (!asyncLoad.isDone)
            {
                // Update loading bar
                loadingBar.value = asyncLoad.progress / 0.9f;
                yield return null;
            }
        }
        else
        {
            Debug.LogError("No scene selected to load.");
        }
    }

    private void SavePlayerData()
    {
        int activeSlot = GameData.Instance.activeSaveSlot;

        if (activeSlot >= 0)
        {
            // Use the centralized method to update and save player data
            DataHandler.Instance.UpdateAndSavePlayerData(
                activeSlot,
                GameData.Instance
            );
        }
        else
        {
            Debug.LogWarning("No active save slot. Player data will not be saved.");
        }
    }
}
