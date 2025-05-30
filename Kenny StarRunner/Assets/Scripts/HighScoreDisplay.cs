using TMPro;
using UnityEngine;

public class HighScoreDisplay : MonoBehaviour
{
    [SerializeField] private TMP_Text highScoreText;

    void Start()
    {
        UpdateHighScores();
    }

    public void UpdateHighScores()
    {
        if (highScoreText != null && GameManager.Instance != null)
        {
            var scores = GameManager.Instance.HighScores;
            string text = "High Scores:\n";
            int rank = 1;
            for (int i = 0; i < scores.Length; i++)
            {
                if (scores[i] > 0)
                {
                    text += $"{rank}. {scores[i]:D4}\n";
                    rank++;
                }
            }
            highScoreText.text = text.TrimEnd('\n');
        }
    }
}
