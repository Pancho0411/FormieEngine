using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using System.Collections;

public class MainMenu : MonoBehaviour
{
    public Animator animator;
    private FadeManager fadeManager;
    [SerializeField]private float fadeLength = 1f;

    [Header("Menu Navigation")]
    public EventSystem eventSystem;
    public GameObject SlotButton;
    public GameObject DeleteSlotButton;
    public GameObject logoSplash;
    public GameObject MenuButton;
    public GameObject SettingsButton;
    public GameObject QuitButton;
    public GameObject onScreenKeyboard; // Reference to the keyboard panel

    void Start()
    {
        fadeManager = FindAnyObjectByType<FadeManager>();

        fadeManager.FadeOut();

        if(SceneManager.GetActiveScene().name == "MenuScreen")
        {
            eventSystem.SetSelectedGameObject(MenuButton, new BaseEventData(eventSystem));
        }
    }

    //Disables splash screen and takes you to save slots
    public void StartGame()
    {
        AnimatorStateInfo currentState = animator.GetCurrentAnimatorStateInfo(0);

        if (currentState.IsName("ShowTitleScreen") && logoSplash.activeSelf)
        {
            StartCoroutine(Fade("ShowSaveSlots"));

            eventSystem.SetSelectedGameObject(SlotButton, new BaseEventData(eventSystem));
        }
        else
        {
            return;
        }
    }

    //Handles what happens when you press the back button (currently the same as the boost button, so the c key or X/B on controller)
    public void GoBack()
    {
        AnimatorStateInfo currentState = animator.GetCurrentAnimatorStateInfo(0);

        //is in slots menu
        if (currentState.IsName("ShowSaveSlots")){
            StartCoroutine(Fade("ShowTitleScreen"));

            onScreenKeyboard.SetActive(false);

            // Reset EventSystem to ensure proper button navigation
            eventSystem.SetSelectedGameObject(null);
        }
        //is in main menu
        else if (currentState.IsName("ShowMainMenu")) {
            fadeManager.FadeIn();

            EnableTitleScreen();
        }
        //is in settings menu
        else if (currentState.IsName("ShowSettingsMenu")) {
            StartCoroutine(Fade("ShowMainMenu"));

            eventSystem.SetSelectedGameObject(MenuButton, new BaseEventData(eventSystem));
        }
        //is in title screen
        else if (currentState.IsName("ShowTitleScreen")) {
            QuitGameTransition();
        }
        //is in quit screen
        else if (currentState.IsName("ShowQuitScreen")){
            StartCoroutine(Fade("ShowTitleScreen"));

            eventSystem.SetSelectedGameObject(QuitButton, new BaseEventData(eventSystem));
        }
    }
    

    //Enable Settings Menu
    public void EnableSettingsMenu()
    {
        StartCoroutine(Fade("ShowSettingsMenu"));

        eventSystem.SetSelectedGameObject(SettingsButton, new BaseEventData(eventSystem));
    }

    //Go to loading screen for Main Menu
    public void EnableMainMenu()
    {
        //sets the scene to be loaded
        GameData.Instance.selectedScene = "MenuScreen";

        //opens loading screen
        SceneManager.LoadSceneAsync("LoadingScreen");
    }

    //Go to loading screen for Main Menu
    public void EnableTitleScreen()
    {
        //sets the scene to be loaded
        GameData.Instance.selectedScene = "TitleScreen";

        //opens loading screen
        SceneManager.LoadSceneAsync("LoadingScreen");
    }

    //Open Arcade Mode
    public void LoadArcade()
    {
        //sets the mode
        GameData.Instance.isCampaign = false;
        GameData.Instance.isArcade = true;

        //opens loading screen
        SceneManager.LoadSceneAsync("TrackSelect");
    }

    //Open Campaign Select Screen
    public void LoadCampaign()
    {
        //sets the mode
        GameData.Instance.isCampaign = true;
        GameData.Instance.isArcade = false;

        //opens loading screen
        SceneManager.LoadSceneAsync("CampaignSelect");
    }

    //Open Tutorial Screen
    public void LoadTraining()
    {
        //set the scene to be loaded
        GameData.Instance.selectedScene = "Tutorial";
        GameData.Instance.currentTrack = "Tutorial";

        //sets the mode
        GameData.Instance.isCampaign = false;
        GameData.Instance.isArcade = false;

        //opens loading screen
        SceneManager.LoadSceneAsync("LoadingScreen");
    }

    //Open Quit Dialogue Box
    public void QuitGameTransition()
    {
        StartCoroutine(Fade("ShowQuitScreen"));

        eventSystem.SetSelectedGameObject(QuitButton, new BaseEventData(eventSystem));
    }

    //Quit Game
    public void Quit()
    {
        Application.Quit();
    }

    //Close Quit Dialogue Box
    public void CancelQuit()
    {
        StartCoroutine(Fade("ShowTitleScreen"));
    }

    [Header("Slot Management")]
    public DeleteSlot[] deleteSlotsArray;
    public SaveSlot[] saveSlotsArray;

    //this should disable the save slots and enable their corresponding delete slots. That will be managed via having the same slot number, thus the same index, so all this is the toggling on for the button press
    public void enableDeleteSlots()
    {
        StartCoroutine(Fade("ShowDeleteSlots"));
        eventSystem.SetSelectedGameObject(DeleteSlotButton, new BaseEventData(eventSystem));

        //reference to deleteslot to then call RefreshSlotDetails
        foreach (SaveSlot saveSlot in saveSlotsArray)
        {
            saveSlot.RefreshSlotDetails();
        }

        foreach (DeleteSlot deleteSlot in deleteSlotsArray)
        {
            deleteSlot.RefreshSlotDetails();
        }
    }

    public void disableDeleteSlots()
    {
        StartCoroutine(Fade("ShowSaveSlots"));
        eventSystem.SetSelectedGameObject(SlotButton, new BaseEventData(eventSystem));

        //reference to saveslot to then call RefreshSlotDetails
        foreach (DeleteSlot deleteSlot in deleteSlotsArray)
        {
            deleteSlot.RefreshSlotDetails();
        }

        foreach (SaveSlot saveSlot in saveSlotsArray)
        {
            saveSlot.RefreshSlotDetails();
        }
    }

    private IEnumerator Fade(string trigger)
    {
        DisableInput();
        fadeManager.FadeIn();

        // Wait for the fade length
        yield return new WaitForSeconds(fadeLength);
        animator.SetTrigger(trigger);
        yield return new WaitForSeconds(fadeLength);

        fadeManager.FadeOut();
        EnableInput();
    }

    // Disables input
    public void DisableInput()
    {
        eventSystem.enabled = false;
    }

    // Enables input
    public void EnableInput()
    {
        eventSystem.enabled = true;
    }
}