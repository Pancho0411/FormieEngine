using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class FadeManager : MonoBehaviour
{
    [Header("Fade Settings")]
    public Image fadeImage;  // The UI Image that will act as the fade screen
    public float fadeDuration = 1f;  // Duration of the fade effect

    private void Start()
    {
        // Ensure the fade image is initially transparent
        fadeImage.color = new Color(fadeImage.color.r, fadeImage.color.g, fadeImage.color.b, 0);
    }

    // Method to fade in (usually when respawn trigger is hit)
    public void FadeIn()
    {
        StartCoroutine(Fade(0f, 1f));
    }

    // Method to fade out (usually when respawn completes)
    public void FadeOut()
    {
        StartCoroutine(Fade(1f, 0f));
    }

    // Coroutine to handle the fade effect
    private IEnumerator Fade(float startAlpha, float endAlpha)
    {
        float timeElapsed = 0f;

        // Gradually change the alpha value of the fade image
        while (timeElapsed < fadeDuration)
        {
            float alpha = Mathf.Lerp(startAlpha, endAlpha, timeElapsed / fadeDuration);
            fadeImage.color = new Color(fadeImage.color.r, fadeImage.color.g, fadeImage.color.b, alpha);

            timeElapsed += Time.deltaTime;
            yield return null;
        }

        // Ensure the final value is set
        fadeImage.color = new Color(fadeImage.color.r, fadeImage.color.g, fadeImage.color.b, endAlpha);
    }
}
