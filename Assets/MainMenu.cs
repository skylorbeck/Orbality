using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{

    public void LoadLevel(int level)
    {
        SceneManager.LoadScene(level);
    }
    public void LoadLevel(string level)
    {
        SceneManager.LoadScene(level);
    }
    public void QuitGame()
    {
        Debug.Log("Quit");
        Application.Quit();
    }
}
