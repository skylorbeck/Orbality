using System;
using TMPro;
using UnityEngine;

public class EndGameManager : MonoBehaviour
{
    private bool _gameOver;

    [SerializeField] private Transform[] Buttons;
    [SerializeField] private TextMeshProUGUI ScoreText;
    private Transform ScoreTextTransform;

    private void Start()
    {
        ScoreTextTransform = ScoreText.transform;
    }

    void Update()
    {
        if (_gameOver)
        {
            foreach (var button in Buttons)
            {
                button.gameObject.SetActive(true);
                if (button.localScale.x < 1)
                {
                    button.localScale += new Vector3(0.01f, 0.01f, 0.01f);
                }
                else
                {
                    button.localScale = Vector3.one;
                }
            }
            ScoreTextTransform.gameObject.SetActive(true);
            if (ScoreTextTransform.localScale.x < 1)
            {
                ScoreTextTransform.localScale += new Vector3(0.01f, 0.01f, 0.01f);
            }
            else
            {
                ScoreTextTransform.localScale = Vector3.one;
            }
        }
        else
        {
            foreach (var button in Buttons)
            {
                if (button.localScale.x >= 0.01f)
                {
                    button.localScale -= new Vector3(0.05f, 0.05f, 0.05f);
                }
                else
                {
                    button.localScale = Vector3.zero;
                    button.gameObject.SetActive(false);
                }
            }
            if (ScoreTextTransform.localScale.x >= 0.01f)
            {
                ScoreTextTransform.localScale -= new Vector3(0.05f, 0.05f, 0.05f);
            }
            else
            {
                ScoreTextTransform.localScale = Vector3.zero;
                ScoreTextTransform.gameObject.SetActive(false);
            }
        }
    }

    public void InvertGameOver()
    {
        _gameOver = !_gameOver;
    }
    

    public void SetGameover(bool gameOver)
    {
        SetGameover(gameOver,0);
    }
    public void SetGameover(bool gameOver, int score)
    {
        ScoreText.text = "Final Score: " + score;
        _gameOver = gameOver;
    }

    public void RestartGame()
    {
        _gameOver = false;
        GameManager.Instance.NewGame();
    }
}