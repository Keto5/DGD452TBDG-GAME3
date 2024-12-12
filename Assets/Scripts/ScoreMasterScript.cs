using UnityEngine;
using TMPro;

public class ScoreMasterScript : MonoBehaviour
{
    // Current score
    private int score = 0;

    // Reference to the TextMeshPro text component for score display
    public TextMeshProUGUI scoreText;

    void Start()
    {
        // Initialize the score display
        UpdateScoreText();
    }

    // Method to add points to the score
    public void AddPoints(int points)
    {
        score += points;
        UpdateScoreText();
    }

    // Updates the TextMeshPro UI with the current score
    private void UpdateScoreText()
    {
        if (scoreText != null)
        {
            scoreText.text = "Score: " + score;
        }
        else
        {
            Debug.LogWarning("Score text is not assigned in the inspector!");
        }
    }
}