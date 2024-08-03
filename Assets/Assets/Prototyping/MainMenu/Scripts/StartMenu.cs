using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityStandardAssets.CrossPlatformInput;

public class StartMenu : MonoBehaviour
{

    [Header("Components")]
    [SerializeField] private string nextStage = "";

    void Update()
    {
        if (CrossPlatformInputManager.GetButton("Start") || CrossPlatformInputManager.GetButton("JumpAction"))
        {
            SceneManager.LoadSceneAsync(nextStage);
        }
    }
}
