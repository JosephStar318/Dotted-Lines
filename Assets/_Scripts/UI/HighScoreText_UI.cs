using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class HighScoreText_UI : MonoBehaviour
{
    private TextMeshProUGUI highScoreText;

    private void Start()
    {
        highScoreText = GetComponent<TextMeshProUGUI>();
        int highScore = PlayerprefsHelper.GetHighScore();
        highScoreText.SetText($"{highScore}");

        ScoreManager.OnScoreChanged += ScoreManager_OnScoreChanged;
    }
    private void OnDestroy()
    {
        ScoreManager.OnScoreChanged -= ScoreManager_OnScoreChanged;
    }
    private void ScoreManager_OnScoreChanged(int score, int highScore)
    {
        highScoreText.SetText($"{highScore}");
    }
}
