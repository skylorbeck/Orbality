using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using Random = UnityEngine.Random;

public class PlayerController : MonoBehaviour
{

    private Camera _cam;

    [SerializeField] private Vector3 mousePos;
    [SerializeField] private Vector3 startingPos = Vector3.zero;
    [SerializeField] bool awake = false;
    [SerializeField] bool hasShot = false;
    bool _cleanShot = true;
    [SerializeField] private float speed = 100f;
    [SerializeField] private float multiplier = 5f;
    [SerializeField] private float maxLen = 1000f;
    [SerializeField] private PhysicsSimulator physicsSimulator;
    private Vector2 _force = Vector2.zero;
    private float _torque = 0f;
    [SerializeField]private float waitTime = 0f;
    [SerializeField]private float maxWaitTime = 3f;
    [SerializeField]private int[] X = {-5,5};
    [SerializeField]private int[] Y = {-5,5};
    [SerializeField]private bool doRandomize = false;
    private Rigidbody2D _rb;
    private Unity.Mathematics.Random _random;
    private Material _material;
    private ParticleSystem _particleSystem;

    void Start()
    {
        _random = GameManager.Instance.GetRandom();
        _cam = Camera.main;
        _rb = GetComponent<Rigidbody2D>();
        startingPos = transform.position;
        RandomizeStartingPos();
        _rb.Sleep();
        _material = GetComponent<SpriteRenderer>().material;
        _particleSystem = GetComponent<ParticleSystem>();
    }

    private void FixedUpdate()
    {
        if (_rb.velocity.normalized.magnitude <0.1f && GetHasShot())
        {
            waitTime += Time.deltaTime;
            if (waitTime>maxWaitTime)
            {
                GameManager.Instance.Reset(false);
            }
        }
    }

    void Update()
    {
        SetForce();
        
        if (!GetAwake())
        {
            _rb.Sleep();
        }
    }

    private void OnMouseDown()
    {
        if (!GetHasShot() && !PauseManager.Instance.IsPaused())
        {
            awake = true;
            _torque = (Random.value - 0.5f) * 480;
        }
    }

    private void OnMouseUp()
    {
        if (GetAwake() && !GetHasShot() && !PauseManager.Instance.IsPaused())
        {
            mousePos = _cam.ScreenToWorldPoint(Input.mousePosition);
            SetForce();
            _rb.WakeUp();
            _rb.AddForce(_force);
            _rb.AddTorque(_torque);
            hasShot = true;
        }
        if (PauseManager.Instance.IsPaused())
        {
            awake = false;
        }
    }

    private void OnMouseDrag()
    {
        if (!PauseManager.Instance.IsPaused())
        {
            mousePos = _cam.ScreenToWorldPoint(Input.mousePosition);
        }
        if (PauseManager.Instance.IsPaused())
        {
            awake = false;
        }
    }

    public bool GetAwake()
    {
        return awake;
    }

    public bool GetHasShot()
    {
        return hasShot;
    }

    void SetForce()
    {
        _force = Vector2.ClampMagnitude((mousePos - transform.position) * speed * multiplier, maxLen * multiplier);
        physicsSimulator.SetForce(_force, _torque);
    }

    public void Reset()
    {
        RandomizeStartingPos();
        transform.position = startingPos;
        awake = false;
        _cleanShot = true;
        hasShot = false;
        _rb.velocity = Vector2.zero;
        _rb.rotation = 0;
        _rb.angularVelocity = 0;
        waitTime = 0f;
        float score = ScoreManager.Instance.score * 0.1f;
        _material.SetFloat("_Score", score);
        var emission = _particleSystem.emission;
        emission.rateOverTime = score;
        emission.rateOverDistance = score;
    }
    
    public void RandomizeStartingPos()
    {
        if (doRandomize)
        {
            startingPos = new Vector3(_random.NextInt(X[0],X[1]), _random.NextInt(Y[0],Y[1]), 0);
        }
    }

    private void OnCollisionEnter2D(Collision2D col)
    {
        _cleanShot = false;
    }
    
    public bool GetCleanShot()
    {
        return _cleanShot;
    }
    
}
