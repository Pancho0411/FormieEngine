using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;
using System.Text;
using TMPro;

[AddComponentMenu("Formie Engine/Game/Score Manager")]
public class ScoreManager : MonoBehaviour
{
    private static ScoreManager instance;

    public static ScoreManager Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<ScoreManager>();
                instance.StartSingleton();
            }

            return instance;
        }
    }

    [Header("Score UI")]
    [SerializeField] private TMP_Text timeCounter = null;
    [SerializeField] private TMP_Text healthCounter = null;
    public TMP_Text finalScoreCounter = null;


    private int milliseconds;
    private int seconds;
    private int minutes;

    private int health;

    public float time { get; set; }
    public bool stopTimer { get; set; }

    private static readonly string[] digits = new string[] { "0", "1", "2", "3", "4", "5", "6", "7", "8", "9", ":", "." };
    private static readonly StringBuilder timer = new StringBuilder(8);

    public int Health
    {
        get { return health; }
        set
        {
            health = value;
            healthCounter.text = health.ToString("D3");
        }
    }

    private void StartSingleton()
    {
        health = 6;
        time = 0;
    }

    private void Update()
    {
        if (!stopTimer)
        {
            time += Time.deltaTime;
            var oldMillisecond = milliseconds;

            minutes = (int)(time / 60f) % 60;
            seconds = (int)(time % 60);
            milliseconds = (int)(time * 100f) % 100;

            if (milliseconds != oldMillisecond)
            {
                timer.Length = 0;
                timer.Append(digits[minutes / 10]);
                timer.Append(digits[minutes % 10]);
                timer.Append(digits[10]);
                timer.Append(digits[seconds / 10]);
                timer.Append(digits[seconds % 10]);
                timer.Append(digits[11]);
                timer.Append(digits[milliseconds / 10]);
                timer.Append(digits[milliseconds % 10]);
                timeCounter.text = timer.ToString();
            }
        }
    }

    public void ResetTimer()
    {
        time = minutes = seconds = milliseconds = 0;
        timeCounter.text = "00:00.00";
    }

    public void Die()
    {
        stopTimer = true;

        StageManager.Instance.Restart();
    }
}
