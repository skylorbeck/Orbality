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

public class PlayerPreviewer : MonoBehaviour
{

    private Camera _cam;

    [SerializeField] private Vector3 mousePos;
    [SerializeField] private Vector3 startingPos = Vector3.zero;
    [SerializeField] private float speed = 100f;
    [SerializeField] private float multiplier = 5f;
    [SerializeField] private float maxLen = 1000f;
    [SerializeField] private PhysicsSimulator physicsSimulator;
    private Vector2 _force = Vector2.zero;
    private float _torque = 0f;
    private Rigidbody2D _rb;
    private Material _material;
    private VisualEffect _vfx;
    private bool _isSimulated = false;
    private bool _collided = false;

    private Vector2 _collisionPoint = Vector2.zero;

    void Start()
    {
        _cam = Camera.main;
        _rb = GetComponent<Rigidbody2D>();
        startingPos = transform.position;
        _rb.Sleep();
        _material = GetComponent<SpriteRenderer>().material;
        _vfx = GetComponent<VisualEffect>();
    }

    void Update()
    {
        if (_isSimulated)
        {
            return;
        }

        SetForce();
        _rb.Sleep();
       
    }



    void SetForce()
    {
        _force = Vector2.ClampMagnitude((mousePos - transform.position) * speed * multiplier, maxLen * multiplier);
        physicsSimulator.SetForce(_force, _torque);
    }

    public void SetSimulation()
    {
        _isSimulated = CompareTag("Simulated");
        if (_isSimulated) return;
        _material = GetComponent<SpriteRenderer>().material;
        _material.SetFloat("_Score", 10);
        _vfx = GetComponent<VisualEffect>();

        _vfx.SetInt("Multiplier", 5);
        _vfx.SetFloat("Velocity", 5);
    }

    private void OnCollisionEnter2D(Collision2D col)
    {
        if (_isSimulated)
        {
            _collided = true;
            _collisionPoint = transform.position;
        }
    }

    public bool Collided()
    {
        return _collided;
    }
    
    public Vector2 GetCollisionPoint()
    {
        _collided = false;
        return _collisionPoint;
    }
}
