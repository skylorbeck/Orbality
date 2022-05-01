using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ScoreManager : MonoBehaviour
{
    public static ScoreManager Instance;

    public int score;
    public int highScore;
    public int combo;
    public float comboTimer;
    [SerializeField]public float comboTimerMax;
    
    [SerializeField]private TextMeshProUGUI scoreText;
    [SerializeField]private TextMeshProUGUI highScoreText;
    [SerializeField]private TextMeshProUGUI comboText;
    [SerializeField]private TextMeshProUGUI comboTimerText;
    [SerializeField]private TextMeshProUGUI comboBreakText;
    [SerializeField]private RectTransform comboBar;
    private Image _comboBarImage;


    private void Update()
    {
        scoreText.text = "Score: " + score;
        highScoreText.text = "High Score: " + highScore;
        comboBar.localScale = new Vector3(comboTimer/comboTimerMax, comboTimer>0?Math.Min(comboTimerMax/comboTimer,3) :1, 1);
        _comboBarImage.color = Color.Lerp(Color.red,Color.white, comboTimer/comboTimerMax);

        if (combo>1)
        {
            comboText.enabled = true;
            comboText.text = combo + "x Combo!";
        } else comboText.enabled = false;

        switch (comboTimer)
        {
            case > 0:
                comboTimer -= Time.deltaTime;
                comboTimerText.text = comboTimer.ToString("F2");
                break;
            case < 0:
                ResetCombo();
                break;
        }
    }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        score = 0;
        combo = 0;
        comboTimer = 0;
        highScore = SaveManager.Instance.saveData.highScore;
        _comboBarImage = comboBar.GetComponent<Image>();
    }

    public void AddScore(int amount)
    {
        if (amount> 0)
        {
            combo++;
            comboTimer = comboTimerMax;
        }
        score += amount * combo;
        if (score <0)
        {
            score = 0;
        }

        if (score > highScore)
        {
            highScore = score;
            SaveManager.Instance.saveData.highScore = highScore;
        }
    }
    
    public void LosePoints(int amount)
    {
        ResetCombo();
        score -= amount;
        if (score <0)
        {
            score = 0;
        }
    }

    public void ResetScore()
    {
        score = 0;
        combo = 0;
    }

    public void ResetCombo()
    {
        if (combo>1)
        {
            comboBreakText.gameObject.GetComponent<Animator>().SetTrigger("pop");
        }
        combo = 0;
        comboTimer = 0;
        comboTimerText.text = "";
    }
}