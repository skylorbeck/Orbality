using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class SaveManager : MonoBehaviour
{
    public static SaveManager Instance;
    public SaveData saveData = new SaveData();
    private void Start()
    {
        if (Instance == null)
        {
            Instance = this;
            if (File.Exists(Application.persistentDataPath + "/save.json"))
            {
                string json = File.ReadAllText(Application.persistentDataPath + "/save.json");
                saveData = JsonUtility.FromJson<SaveData>(json);
            }
        }
        else
        {
            Destroy(gameObject);
        }
        DontDestroyOnLoad(gameObject);
    }

    public void SaveGame()
    {
        String saveJson = JsonUtility.ToJson(saveData);
        File.WriteAllBytes(Application.persistentDataPath + "/save.json", System.Text.Encoding.UTF8.GetBytes(saveJson));
    }
    
    public class SaveData
    {
        public int ballSkin = 0;
        public int highScore = 0;
        public int guideSkin = 0;
        public int glowSkin = 0;
        public int particleSkin = 0;
        public int colliderSkin = 0;
    }
    
    public int GetBallSkin()
    {
        return saveData.ballSkin;
    }
    
    public int GetHighScore()
    {
        return saveData.highScore;
    }
    
    public int GetGuideSkin()
    {
        return saveData.guideSkin;
    }
    
    public int GetGlowSkin()
    {
        return saveData.glowSkin;
    }
    
    public int GetParticleSkin()
    {
        return saveData.particleSkin;
    }
    
    public int GetColliderSkin()
    {
        return saveData.colliderSkin;
    }
    
    public void SetBallSkin(int skin)
    {
        saveData.ballSkin = skin;
    }
    
    public void SetHighScore(int score)
    {
        saveData.highScore = score;
    }
    
    public void SetGuideSkin(int skin)
    {
        saveData.guideSkin = skin;
    }
    
    public void SetGlowSkin(int skin)
    {
        saveData.glowSkin = skin;
    }
    
    public void SetParticleSkin(int skin)
    {
        saveData.particleSkin = skin;
    }
    
    public void SetColliderSkin(int skin)
    {
        saveData.colliderSkin = skin;
    }
    
    public void Reset()
    {
        saveData.ballSkin = 0;
        saveData.highScore = 0;
        saveData.guideSkin = 0;
        saveData.glowSkin = 0;
        saveData.particleSkin = 0;
        saveData.colliderSkin = 0;
    }
}
