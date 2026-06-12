using UnityEngine;

/// Handles the player's score during gameplay.
/// 
/// Other scripts can call AddScore() when enemies are defeated.
/// The UIManager reads the current score and displays it on screen.
public class ScoreManager : MonoBehaviour
{
    // The player's current score.
    private int currentScore;

    /// Adds points to the player's score.
    public void AddScore(int pointsToAdd)
    {
        // Do not add negative points.
        if (pointsToAdd <= 0)
        {
            return;
        }

        currentScore += pointsToAdd;

        Debug.Log("Score: " + currentScore);
    }

    /// Returns the current score.
    /// UIManager uses this to update the score text.
    public int GetCurrentScore()
    {
        return currentScore;
    }

    /// Resets the score back to zero.
    /// Useful when restarting or starting a new run.
    public void ResetScore()
    {
        currentScore = 0;
    }
}