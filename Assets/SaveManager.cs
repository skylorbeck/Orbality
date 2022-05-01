using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class SaveManager : MonoBehaviour
{
    public static SaveManager Instance;
    private SaveData saveData = new SaveData();
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
        StreamWriter writer = new StreamWriter(Application.persistentDataPath + "/save.json");
        writer.Write(saveJson);
    }
    
    public class SaveData
    {
        public int ballSkin = 0;
        public int highScore = 0;
    }
}
