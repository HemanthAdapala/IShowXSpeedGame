using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ScoreManager : MonoBehaviour
{
    public TextMeshProUGUI ScoreText; // Reference to the UI score text
    private int score = 0; // Current score

    void OnEnable()
    {
        // Subscribe to the cube passed center event
        GameEventManager.OnCubePassedCenter += IncreaseScore;
    }

    void OnDisable()
    {
        // Unsubscribe from the event
        GameEventManager.OnCubePassedCenter -= IncreaseScore;
    }

    void IncreaseScore()
    {
        score++;
        UpdateScoreDisplay();
    }

    void UpdateScoreDisplay()
    {
        if (ScoreText != null)
        {
            if (score <= 9)
            {
                ScoreText.text = "Score:  0" + score;
            }
            else
                ScoreText.text = "Score: " + score;
        }
    }
}