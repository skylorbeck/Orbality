using System;
using System.Collections;
using System.Collections.Generic;
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
            ScoreManager.Instance.AddScore(-5);
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
