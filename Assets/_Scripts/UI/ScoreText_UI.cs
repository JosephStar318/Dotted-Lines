using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ScoreText_UI : MonoBehaviour
{
    private TextMeshProUGUI scoreText;

    private void Start()
    {
        scoreText = GetComponent<TextMeshProUGUI>();
        ScoreManager.OnScoreChanged += ScoreManager_OnScoreChanged;
    }
    private void OnDestroy()
    {
        ScoreManager.OnScoreChanged -= ScoreManager_OnScoreChanged;
    }
    private void ScoreManager_OnScoreChanged(int score)
    {
        scoreText.SetText(score.ToString());
    }
}
