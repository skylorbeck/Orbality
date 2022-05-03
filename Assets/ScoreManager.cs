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
    [SerializeField] private TextMeshProUGUI _pointsText;
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
                string seconds = (comboTimer % 60).ToString("00");
                string milliseconds = ((comboTimer * 1000) % 1000).ToString("000");
                comboTimerText.text =(seconds + ":" + milliseconds);
                comboTimerText.color = Color.Lerp(Color.red,Color.white, comboTimer/comboTimerMax);
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
        highScore = SaveManager.Instance.GetHighScore();
        _comboBarImage = comboBar.GetComponent<Image>();
    }

    public void AddScore(int amount)
    {
        if (amount> 0)
        {
            int points = amount * Math.Max(1,combo);
            score += points;
            _pointsText.transform.position = GameManager.Instance.goal.transform.position*0.75f;
            _pointsText.text = "+" + points;
                _pointsText.GetComponent<Animator>().SetTrigger("pop");
            combo++;
            comboTimer = comboTimerMax;
        }
        if (score <0)
        {
            score = 0;
        }

        if (score > highScore)
        {
            highScore = score;
            SaveManager.Instance.SetHighScore( highScore);
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