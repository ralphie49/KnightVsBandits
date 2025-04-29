using UnityEngine;
using UnityEngine.SceneManagement;


public class MainMenu : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public void StartGame()
    {
        SceneManager.LoadScene("Level1");
    }
    public void QuitGame()
    {
        Application.Quit();
        Debug.Log("Quit Game");
    }
}
