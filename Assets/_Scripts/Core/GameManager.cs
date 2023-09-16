using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static event Action OnGameOver;
    public static event Action OnGamePaused;
    public static GameManager Instance { get; private set; }

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
    }
    private void OnDisable()
    {
        LineCollisionChecker.OnLineCollision -= LineCollisionChecker_OnLineCollision;
    }
    private void LineCollisionChecker_OnLineCollision(Vector2 pos)
    {
        Debug.Log("Game over");
        OnGameOver?.Invoke();
    }
    public void PauseGame()
    {

        OnGamePaused?.Invoke();
    }
}
