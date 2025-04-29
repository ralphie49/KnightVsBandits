using UnityEngine;
using UnityEngine.SceneManagement; 

public class QuitGame : MonoBehaviour
{
    public void Quit()
    {
        Debug.Log("Quit button pressed! Loading Main Menu...");
        SceneManager.LoadScene("MainMenu"); 
    }
}
