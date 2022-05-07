using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkinnableObject : MonoBehaviour
{
    [SerializeField] private int _skinIndex = 0;
    SpriteRenderer _spriteRenderer;
    
    [SerializeField] private String _collisionTag = "";
    void Start()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
        SetSkinIndex(SaveManager.Instance.GetColliderSkin());
    }

    public void SetSkinIndex(int index)
    {
        _skinIndex = index;
        if (_spriteRenderer == null)
        {
            _spriteRenderer = GetComponent<SpriteRenderer>();
        }
        
        _spriteRenderer.sprite = Resources.Load<Sprite>(_collisionTag +"/" + _skinIndex);//todo convert all to Resources.Load
    }
    
    public int GetSkinIndex()
    {
        return _skinIndex;
    }
}
