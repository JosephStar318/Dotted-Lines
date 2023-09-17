using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static event Action OnGameOver;
    public static event Action<int> OnDifficultyChanged;
    public static event Action OnGamePaused;
    public static event Action OnGameUnpaused;
    public static GameManager Instance { get; private set; }


    private int difficultyIndex;
    private void Awake()
    {
        if(Instance == null)
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
        LineCollisionChecker.OnLineCollision += LineCollisionChecker_OnLineCollision;
        ScoreManager.OnScoreChanged += ScoreManager_OnScoreChanged;
    }

    private void OnDisable()
    {
        LineCollisionChecker.OnLineCollision -= LineCollisionChecker_OnLineCollision;
        ScoreManager.OnScoreChanged -= ScoreManager_OnScoreChanged;
    }

    private void ScoreManager_OnScoreChanged(int score)
    {
        int calculatedDifficulty = CalculateDifficulty(score);
        if (difficultyIndex != calculatedDifficulty)
        { 
            difficultyIndex = calculatedDifficulty;
            OnDifficultyChanged?.Invoke(difficultyIndex);
        }
    }
    private int CalculateDifficulty(int score)
    {
        return Mathf.FloorToInt( score / 100);
    }
    private void LineCollisionChecker_OnLineCollision(Vector2 pos)
    {
        Debug.Log("Game over");
        UIManager.Show<GameOverPanel>();
        OnGameOver?.Invoke();
    }
    public void PauseGame()
    {

        OnGamePaused?.Invoke();
    }
    public void UnpauseGame()
    {

        OnGameUnpaused?.Invoke();
    }
    public void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
