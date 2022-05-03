using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using Random = UnityEngine.Random;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    [SerializeField] public PegManager pegManager;
    [SerializeField] public PlayerController player;
    [SerializeField] public PhysicsSimulator physicsSimulator;
    [SerializeField] public GameObject goal;
    [SerializeField] private Unity.Mathematics.Random random;
    [SerializeField] private float timeLeft = 60f;
    [SerializeField] private TextMeshProUGUI timeText;
    private void Start()
    {
        random = new Unity.Mathematics.Random((uint)DateTime.Now.Millisecond);
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Update()
    {
        timeLeft -= Time.deltaTime;
        if (timeLeft < 0)
        {
            timeLeft = 0;
           PauseManager.Instance.BackToMenu();
        }
        string minutes = Mathf.Floor(timeLeft / 60).ToString("00");
        string seconds = (timeLeft % 60).ToString("00");
        string milliseconds = ((timeLeft * 1000) % 1000).ToString("000");
        timeText.text = minutes + ":" + seconds + ":" + milliseconds;
        if (Input.GetKeyDown(KeyCode.R))
        {
            Reset(false);
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            PauseManager.Instance.PauseUnpause();
        }
    
    }
    
 
    public void Reset(bool won)
    {
        if (!won)
        {
            ScoreManager.Instance.LosePoints(5);
        }
        pegManager.Reset(won);
        player.Reset();
        physicsSimulator.Reset();
            PauseManager.Instance.SetPaused(false);
    }

    public Unity.Mathematics.Random GetRandom()
    {
        return random;
    }
}
