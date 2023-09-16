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
    }
    private void OnEnable()
    {
        ScoreManager.OnScoreChanged += ScoreManager_OnScoreChanged;
    }
    private void OnDisable()
    {
        ScoreManager.OnScoreChanged -= ScoreManager_OnScoreChanged;
    }
    private void ScoreManager_OnScoreChanged(int score)
    {
        scoreText.SetText(score.ToString());
    }
}
