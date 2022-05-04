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

public class PlayerPreviewer : BallAbstraction
{

    new void Start()
    {
        base.Start();
        _rb.Sleep();
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

    private void OnCollisionEnter2D(Collision2D col)
    {
        if (_isSimulated)
        {
            _collided = true;
            _collisionPoint = transform.position;
        }
    }
}
