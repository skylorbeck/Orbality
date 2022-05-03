using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class MainMenuManager : MonoBehaviour
{
    [SerializeField]private TextMeshProUGUI highScoreText;

    // Start is called before the first frame update
    void Start()
    {
        highScoreText.text = "High Score: " + SaveManager.Instance.saveData.highScore;

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
