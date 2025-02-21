using UnityEngine;
using TMPro;

public class OnscreenKeyboard : MonoBehaviour
{
    public TMP_InputField inputField;
    private bool isUpperCase = true;
    public GameObject keyboardPanel;
    public SaveSlot currentSaveSlot;
    public Animator animator;

    public void AddCharacter(string character)
    {
        inputField.text += isUpperCase ? character.ToUpper() : character.ToLower();
    }

    public void Backspace()
    {
        if (!string.IsNullOrEmpty(inputField.text))
        {
            inputField.text = inputField.text.Substring(0, inputField.text.Length - 1);
        }
    }

    public void Submit()
    {
        Debug.Log("Submit button pressed with text: " + inputField.text);

        if (currentSaveSlot != null && !string.IsNullOrEmpty(inputField.text))
        {
            currentSaveSlot.SaveNewGame(inputField.text);

            // Clear and hide input
            inputField.text = "";
            ToggleKeyboard(false);

            // Handle animation
            if (animator != null)
            {
                AnimatorStateInfo currentState = animator.GetCurrentAnimatorStateInfo(0);
                if (currentState.IsName("ShowSaveSlots"))
                {
                    animator.SetTrigger("FadeOut");
                    animator.SetTrigger("ShowMainMenu");
                }
            }
        }
        else
        {
            Debug.LogError($"Submit failed - SaveSlot: {currentSaveSlot}, Text: {inputField.text}");
        }
    }

    public void ToggleCase()
    {
        isUpperCase = !isUpperCase;
    }

    public void ToggleKeyboard(bool show)
    {
        keyboardPanel.SetActive(show);
    }
}