using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    public static event Action<int, int> OnScoreChanged;
    public static event Action<Vector2, int> OnScoreCalculated;
    public static ScoreManager Instance { get; private set; }
    public int Score { get; private set; }
    public int HighScore { get; private set; }

    [SerializeField] private int scoreMultiplier;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void OnEnable()
    {
        DottedLine.OnLineCleared += DottedLine_OnLineCleared;
        GameManager.OnGameStarted += GameManager_OnGameStarted;
    }
    private void OnDisable()
    {
        DottedLine.OnLineCleared -= DottedLine_OnLineCleared;
        GameManager.OnGameStarted -= GameManager_OnGameStarted;
    }
    private void GameManager_OnGameStarted()
    {
        Score = 0;
        HighScore = PlayerprefsHelper.GetHighScore();
        OnScoreChanged?.Invoke(Score, HighScore);
    }

    private void DottedLine_OnLineCleared(Vector2 position, int count)
    {
        int calculatedScore = CalculateScore(count);
        OnScoreCalculated?.Invoke(position, calculatedScore);

        Score += calculatedScore;
        HighScore = Mathf.Max(Score, HighScore);

        PlayerprefsHelper.SetHighScore(HighScore);
        OnScoreChanged?.Invoke(Score, HighScore);
    }

    private int CalculateScore(int numberOfDots)
    {
        return numberOfDots * scoreMultiplier;
    }
}
