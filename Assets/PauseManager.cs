using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseManager : MonoBehaviour
{
    public static PauseManager Instance;
    private bool _paused;
    [SerializeField] public SpriteRenderer blackoutSprite;
    [SerializeField] public Transform[] Buttons;
    // Start is called before the first frame update
    void Start()
    {
        if (Instance!)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (_paused)
        {
            if (blackoutSprite.color.a < 0.75f)
            {
                blackoutSprite.color += new Color(0, 0, 0, 0.01f);
            }
            else
            {
                blackoutSprite.color = new Color(0, 0, 0, 0.75f);
            }
            
            if (Time.timeScale >0.1f)
            {
                Time.timeScale -= Time.timeScale * 0.01f;
            }
            else
            {
                Time.timeScale = 0;
            }

            foreach (var button in Buttons)
            {
                button.gameObject.SetActive(true);
                if (button.localScale.magnitude<1)
                {
                    button.localScale += new Vector3(0.01f, 0.01f, 0.01f);
                }
                else
                {
                    button.localScale = Vector3.one;
                }
            }
        }
        else
        {
            if (blackoutSprite.color.a >= 0.75f)
            {
                blackoutSprite.color -= new Color(0, 0, 0, 0.01f);
            }
            else
            {
                blackoutSprite.color = new Color(0, 0, 0, 0f);
            }
            if (Time.timeScale < 1)
            {
                Time.timeScale += Math.Min(0.01f, 1 - Time.timeScale);
            }
            else
            {
                Time.timeScale = 1f;
            }
            foreach (var button in Buttons)
            {
                button.gameObject.SetActive(true);
                if (button.localScale.magnitude>=1)
                {
                    button.localScale -= new Vector3(0.01f, 0.01f, 0.01f);
                }
                else
                {
                    button.localScale = Vector3.zero;
                    button.gameObject.SetActive(false);
                }
            }
        }
    }

    public void PauseUnpause()
    {
        _paused = !_paused;
    }
    
    public bool IsPaused()
    {
        return _paused;
    }
    public void SetPaused(bool paused)
    {
        _paused = paused;
    }
}
