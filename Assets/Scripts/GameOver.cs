using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOver: MonoBehaviour
{
    public AudioClip gameOverClip;   // Assign your GameOver sound here in Inspector
    private AudioSource audioSource;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();

        if (gameOverClip != null && audioSource != null)
        {
            audioSource.PlayOneShot(gameOverClip, 1.0f);
        }
    }

    public void RestartGame()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene("Level1"); // replace with your Level 1 scene name
    }

    public void QuitToMainMenu()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene("MainMenu"); // replace with your Main Menu scene name
    }
}

