using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    public static event Action<int> OnScoreChanged;
    public static event Action<Vector2, int> OnScoreCalculated;
    public static ScoreManager Instance { get; private set; }
    public int Score { get; private set; }

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
    }
    private void OnDisable()
    {
        DottedLine.OnLineCleared -= DottedLine_OnLineCleared;
    }
   
    private void DottedLine_OnLineCleared(Vector2 position, int count)
    {
        int calculatedScore = CalculateScore(count);
        OnScoreCalculated?.Invoke(position, calculatedScore);

        Score += calculatedScore;
        OnScoreChanged?.Invoke(Score);
    }

    private int CalculateScore(int numberOfDots)
    {
        return numberOfDots * scoreMultiplier;
    }
}
