using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PegScript : MonoBehaviour
{
    private Collider2D _collider2D;
    private Renderer _renderer;
    private bool _isHit = false;
    // Start is called before the first frame update
    void Start()
    {
        _collider2D = GetComponent<Collider2D>();
        _renderer = GetComponent<Renderer>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!_isHit)
        {
            _collider2D.enabled = true;
        }
    }
    
    private void OnCollisionEnter2D(Collision2D col)
    {
        if (col.gameObject.tag.Equals("Player"))
        {
            col.rigidbody.AddForce(new Vector2(0, -1), ForceMode2D.Impulse);
            _isHit = true;
        }
        _collider2D.enabled = false;
        _renderer.enabled = false;
    }

    public void Reset(bool isClone)
    {
        _isHit=false;
        _collider2D.enabled = true;
        if (!isClone)
        {
            _renderer.enabled = true;
        }
    }
}
