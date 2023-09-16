using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
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
        Time.timeScale = 0f;
        Debug.Log("Game over");
    }
}
