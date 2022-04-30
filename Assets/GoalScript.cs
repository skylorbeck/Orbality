using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using Random = Unity.Mathematics.Random;

public class GoalScript : MonoBehaviour
{
    [SerializeField] float waitTime = 1f;
    [SerializeField] float timeElapsed = 0;
    [SerializeField] int[] X = { 10, 15 };
    [SerializeField] int[] Y = { -8, 6 };
    bool _hasWon = false;
    private Random _random;
    [SerializeField] GameObject _goal;
    private bool _isSimulation;
    [SerializeField] bool doRandomize = false;
    [SerializeField] private TextMeshProUGUI _cleanShotText;

    void Start()
    {
        _random = GameManager.Instance.GetRandom();
        _isSimulation = gameObject.CompareTag("Simulated");
        Reset(true);
    }

    void FixedUpdate()
    {
        if (!_hasWon) return;
        timeElapsed += Time.deltaTime;
        if (timeElapsed < waitTime) return;
        Reset(false);
    }

    public void Reset(bool soft)
    {
        if (doRandomize)
        {
            _goal.transform.position = !_isSimulation
                ? new Vector3(_random.NextFloat(X[0], X[1]), _random.NextFloat(Y[0], Y[1]), 0)
                : GameManager.Instance.goal.transform.position;
        }

        if (!_isSimulation && !soft)
        {
            GameManager.Instance.Reset(true);
        }

        _hasWon = false;
        timeElapsed = 0;
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (!col.gameObject.CompareTag("Player") || _hasWon) return;
        PlayerController pc = col.GetComponent<PlayerController>();
        if (pc.GetCleanShot())
        {
            ScoreManager.Instance.AddScore(2);
            _cleanShotText.transform.position = transform.position;
            _cleanShotText.GetComponent<Animator>().SetTrigger("pop");
        }
        else
        {
            ScoreManager.Instance.AddScore(1);
        }

        Debug.Log("Goal");
        _hasWon = true;

    }
}