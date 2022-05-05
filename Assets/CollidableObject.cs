using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollidableObject : MonoBehaviour
{
    [SerializeField] private int _skinIndex = 0;
    SpriteRenderer _spriteRenderer;

    void Start()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
        SetSkinIndex(SaveManager.Instance.GetColliderSkin());
    }

    public void SetSkinIndex(int index)
    {
        if (_spriteRenderer == null)
        {
            _spriteRenderer = GetComponent<SpriteRenderer>();
        }
        _skinIndex = index;
        _spriteRenderer.sprite = Resources.Load<Sprite>("ColliderSprites/" + _skinIndex);//todo convert all to Resources.Load
        
    }
    
    public int GetSkinIndex()
    {
        return _skinIndex;
    }
    
    
}
