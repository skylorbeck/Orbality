using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
    public static MenuManager Instance;

    private void Start()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void LoadLevel(int level)
    {
        SaveManager.Instance.SaveGame();
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
