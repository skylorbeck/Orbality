using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.VFX;
using Random = UnityEngine.Random;

public class PlayerController : BallAbstraction
{
    [SerializeField] bool awake = false;
    [SerializeField] bool hasShot = false;
    bool _cleanShot = true;
  
    [SerializeField]private float waitTime = 0f;
    [SerializeField]private float maxWaitTime = 3f;
    [SerializeField]private int[] X = {-5,5};
    [SerializeField]private int[] Y = {-5,5};
    [SerializeField]private bool doRandomize = false;
    private Unity.Mathematics.Random _random;

    new void Start()
    {
        base.Start();
        _random = GameManager.Instance.GetRandom();
        RandomizeStartingPos();
        _rb.Sleep();
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
        _vfx.SetFloat("Velocity", Math.Max( _rb.velocity.magnitude,1f));

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
        float score = ScoreManager.Instance.combo * 0.1f;
        _material.SetFloat("_Score", score);
        _vfx.SetInt("Multiplier", ScoreManager.Instance.combo-1);
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
        if (_isSimulated)
        {
                _collided = true;
                _collisionPoint = transform.position;
        }
        _cleanShot = false;
    }
    
    public bool GetCleanShot()
    {
        return _cleanShot;
    }
}
