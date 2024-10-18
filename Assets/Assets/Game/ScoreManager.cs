using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;
using System.Text;
using TMPro;
using UnityEngine.SocialPlatforms.Impl;

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
                instance = FindFirstObjectByType<ScoreManager>();
                instance.StartSingleton();
            }

            return instance;
        }
    }

    [Header("Score UI")]
    [SerializeField] private TMP_Text timeCounter = null;
    [SerializeField] private TMP_Text scoreCounter = null;
    [SerializeField] private Slider healthSlider = null;
    [SerializeField] private Slider energySlider = null;


    private int milliseconds;
    private int seconds;
    private int minutes;

    private int score;
    private int bits;
    private int bitsBonus;
    private int health;
    private int energy;

    private int timeBonus;

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
            healthSlider.value = health;
        }
    }

    public int Energy
    {
        get { return energy; }
        set
        {
            energy = value;
            energySlider.value = energy;
        }
    }

    public int Bits
    {
        get { return bits; }
        set
        {
            bits = value;
        }
    }

    //public int BitBonus
    //{
    //    get { return bitsBonus; }
    //    set
    //    {
    //        bitsBonus = value;
    //        bitsBonusCounter.text = bitsBonus.ToString();
    //    }
    //}

    public int Score
    {
        get { return score; }
        set
        {
            score = value;
            scoreCounter.text = score.ToString();
        }
    }

    private void StartSingleton()
    {
        health = 6;
        time = 0;
    }

    //public int TimeBonus
    //{
    //    get { return timeBonus; }
    //    set
    //    {
    //        timeBonus = value;
    //        timeBonusCounter.text = timeBonus.ToString();
    //    }
    //}

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
                timer.AppendFormat("{0:D2} : {1:D2} : {2:D2}", minutes, seconds, milliseconds);
                timeCounter.text = timer.ToString();
            }
        }
    }

    public void ResetTimer()
    {
        time = minutes = seconds = milliseconds = 0;
        timeCounter.text = "00 : 00 : 00";
    }

    public void Die()
    {
        stopTimer = true;

        StageManager.Instance.Restart();
    }
}
