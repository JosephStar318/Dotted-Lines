using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static event Action OnGameStarted;
    public static event Action OnGameOver;
    public static event Action<int> OnDifficultyChanged;
    public static event Action OnGamePaused;
    public static event Action OnGameUnpaused;
    public static event Action OnDotsSpawn;
    public static GameManager Instance { get; private set; }

    private int difficultyIndex;
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
        LineCollisionChecker.OnLineCollision += LineCollisionChecker_OnLineCollision;
        ScoreManager.OnScoreChanged += ScoreManager_OnScoreChanged;
    }
    private void OnDisable()
    {
        LineCollisionChecker.OnLineCollision -= LineCollisionChecker_OnLineCollision;
        ScoreManager.OnScoreChanged -= ScoreManager_OnScoreChanged;
    }
    private void ScoreManager_OnScoreChanged(int score, int highScore)
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
        return Mathf.FloorToInt(score / 100);
    }
    private void LineCollisionChecker_OnLineCollision(Vector2 pos)
    {
        Debug.Log("Game over");
        UIManager.Open<GameOverPanel>();
        OnGameOver?.Invoke();
    }
    public void DifficultyUp()
    {
        OnDifficultyChanged?.Invoke(10);
    }
    public void DotsSpawn()
    {
        OnDotsSpawn?.Invoke();
    }

    public void PauseGame()
    {
        UIManager.Open<PausePanel>();
        OnGamePaused?.Invoke();
    }
    public void UnpauseGame()
    {
        UIManager.Close<PausePanel>();
        OnGameUnpaused?.Invoke();
    }
    public void RestartGame()
    {
        StartCoroutine(Delay.Seconds(0.5f, () =>
        {
            AsyncOperation loadOp = SceneManager.LoadSceneAsync("Game");
            loadOp.completed += (ap) => OnGameStarted.Invoke();
            difficultyIndex = 0;
        }));
    }
    public void ReturnMainMenu()
    {
        StartCoroutine(Delay.Seconds(0.5f, () =>
        {
            SceneManager.LoadScene("MainMenu");
        }));
    }
    public void StartGame()
    {
        StartCoroutine(Delay.Seconds(0.5f, () =>
        {
            AsyncOperation loadOp = SceneManager.LoadSceneAsync("Game");
            loadOp.completed += (ap) => OnGameStarted.Invoke();
            difficultyIndex = 0;
        }));
    }
}
