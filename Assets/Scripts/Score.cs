using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Score : MonoBehaviour
{
    public TextMeshProUGUI currentScoreText;
    public TextMeshProUGUI highestScoreText;
    public int currentScore = 0;
    public int highestScore = 0;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        highestScore = PlayerPrefs.GetInt("HighestScore", 0);
        UpdateScoreUI();
    }

    public void AddScore(int points)
    {
        currentScore += points;
        if (currentScore > highestScore)
        {
            highestScore = currentScore;
            PlayerPrefs.SetInt("HighestScore", highestScore);
        }
        UpdateScoreUI();
    }
    void UpdateScoreUI()
    {
        currentScoreText.text = "Current Score: " + currentScore;
        highestScoreText.text = "Highest Score: " + highestScore;
    }
}
