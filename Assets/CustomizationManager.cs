using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class CustomizationManager : MonoBehaviour
{
    int _currentBall = 0;
    int _currentGuide = 0;
    int _currentGlow = 0;
    int _currentParticle = 0;
    int _currentCollider = 0;
    [SerializeField] GraphicRaycaster _canvasRaycaster;
    [SerializeField] TextMeshProUGUI _ballText;
    [SerializeField] TextMeshProUGUI _guideText;
    [SerializeField] TextMeshProUGUI _glowText;
    [SerializeField] TextMeshProUGUI _particleText;
    [SerializeField] TextMeshProUGUI _colliderText;
    
    [SerializeField] private PlayerPreviewer _ballPreviewer;
    [SerializeField] private PhysicsSimulator _physicsSimulator;
    [SerializeField] private GameObject CollidersCollection;

    // Start is called before the first frame update
    void Start()
    {
        _currentBall = SaveManager.Instance.GetBallSkin();
        _ballText.text = _currentBall.ToString();
        _currentGuide = SaveManager.Instance.GetGuideSkin();
        _guideText.text = _currentGuide.ToString();
        _currentGlow = SaveManager.Instance.GetGlowSkin();
        _glowText.text = _currentGlow.ToString();
        _currentParticle = SaveManager.Instance.GetParticleSkin();
        _particleText.text = _currentParticle.ToString();
        _currentCollider = SaveManager.Instance.GetColliderSkin();
        _colliderText.text = _currentCollider.ToString();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            //todo put confirmation message about saving customization here
            SaveCustomization();
            PauseManager.Instance.PauseUnpause();
            _canvasRaycaster.enabled = !_canvasRaycaster.enabled;
        }
    }

    public void ChangeBallSkin(int skin)
    {
        _currentBall = Math.Clamp(skin, 0,2);
        _ballText.text = _currentBall.ToString();
        _ballPreviewer.SetBallSkin(_currentBall);
    }

    public void ChangeGuideSkin(int skin)
    {
        _currentGuide = Math.Clamp(skin, 0, 2);
        _guideText.text = _currentGuide.ToString();
        _physicsSimulator.SetGuideMaterial(_currentGuide);
    }

    public void ChangeGlowSkin(int skin)
    {
        _currentGlow = Math.Clamp(skin, 0, 2);
        _glowText.text = _currentGlow.ToString();
        _ballPreviewer.SetGlow(_currentGlow);
        _ballPreviewer.SetSimulation();
    }

    public void ChangeParticleSkin(int skin)
    {
        _currentParticle = Math.Clamp(skin, 0, 2);
        _particleText.text = _currentParticle.ToString();
        _ballPreviewer.SetVFX(_currentParticle);
    }

    public void ChangeColliderSkin(int skin)
    {
        _currentCollider = Math.Clamp(skin, 0, 2);
        _colliderText.text = _currentCollider.ToString();
       foreach (SkinnableObject co in CollidersCollection.GetComponentsInChildren<SkinnableObject>())
       {
           co.SetSkinIndex(_currentCollider);
       }
    }

    public void SaveCustomization()
    {
        SaveManager.Instance.saveData.ballSkin = _currentBall;
        SaveManager.Instance.saveData.guideSkin = _currentGuide;
        SaveManager.Instance.saveData.glowSkin = _currentGlow;
        SaveManager.Instance.saveData.particleSkin = _currentParticle;
        SaveManager.Instance.saveData.colliderSkin = _currentCollider;
        SaveManager.Instance.SaveGame();
    }

    public void ResetCustomization()
    {
        SaveManager.Instance.saveData.ballSkin = 0;
        SaveManager.Instance.saveData.guideSkin = 0;
        SaveManager.Instance.saveData.glowSkin = 0;
        SaveManager.Instance.saveData.particleSkin = 0;
        SaveManager.Instance.saveData.colliderSkin = 0;
        SaveManager.Instance.SaveGame();
    }

    public void ChangeBallSkin(bool increase)
    {
        ChangeBallSkin(_currentBall + (increase ? 1 : -1));
    }

    public void ChangeGuideSkin(bool increase)
    {
        ChangeGuideSkin(_currentGuide + (increase ? 1 : -1));
    }

    public void ChangeGlowSkin(bool increase)
    {
        ChangeGlowSkin(_currentGlow + (increase ? 1 : -1));
    }

    public void ChangeParticleSkin(bool increase)
    {
        ChangeParticleSkin(_currentParticle + (increase ? 1 : -1));
    }

    public void ChangeColliderSkin(bool increase)
    {
        ChangeColliderSkin(_currentCollider + (increase ? 1 : -1));
    }

    public int GetCurrentBallSkin()
    {
        return _currentBall;
    }

    public int GetCurrentGuideSkin()
    {
        return _currentGuide;
    }

    public int GetCurrentGlowSkin()
    {
        return _currentGlow;
    }

    public int GetCurrentParticleSkin()
    {
        return _currentParticle;
    }

    public int GetCurrentColliderSkin()
    {
        return _currentCollider;
    }
    
    public void SetScore(Slider slider)
    {
        _ballPreviewer.SetMaterialScore((int)slider.value);
    }
}
