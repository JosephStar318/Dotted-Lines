using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DotSpawner : MonoBehaviour
{
    [SerializeField] private BoundingBox boundingBox;
    [SerializeField] private Transform dotPrefab;
    [SerializeField] private SpawnAttr spawnAttr;

    private List<Dot> dotList;
    private Coroutine spawnRoutine;
    private void Start()
    {
        dotList = new List<Dot>(spawnAttr.spawnLimit);

        Dot.OnDestroy += Dot_OnDestroy;
        GameManager.OnGameOver += GameManager_OnGameOver;
        GameManager.OnGamePaused += GameManager_OnGamePaused;
        GameManager.OnGameUnpaused += GameManager_OnGameUnpaused;
        GameManager.OnDifficultyChanged += GameManager_OnDifficultyChanged;
        GameManager.OnDotsSpawn += GameManager_OnDotsSpawn;
        
        SpawnMultipleDots(10);
        spawnRoutine = StartCoroutine(SpawnRoutine());
    }
    private void OnDestroy()
    {
        Dot.OnDestroy -= Dot_OnDestroy;
        GameManager.OnGameOver -= GameManager_OnGameOver;
        GameManager.OnGamePaused -= GameManager_OnGamePaused;
        GameManager.OnGameUnpaused -= GameManager_OnGameUnpaused;
        GameManager.OnDifficultyChanged -= GameManager_OnDifficultyChanged;
        GameManager.OnDotsSpawn -= GameManager_OnDotsSpawn;
    }

    private void FixedUpdate()
    {
        foreach (Dot dot in dotList)
        {
            dot.Move();
        }
    }
    private void GameManager_OnDotsSpawn()
    {
        SpawnMultipleDots(10);
    }
    private void GameManager_OnGameOver()
    {
        this.enabled = false;
        StopCoroutine(spawnRoutine);
    }
    private void GameManager_OnGamePaused()
    {
        this.enabled = false;  
        StopCoroutine(spawnRoutine);
    }
    private void GameManager_OnGameUnpaused()
    {
        this.enabled = true;
        spawnRoutine = StartCoroutine(SpawnRoutine());
    }
    private void GameManager_OnDifficultyChanged(int difficultyIndex)
    {
        spawnAttr.baseSpeed += difficultyIndex * 0.1f;
        spawnAttr.spawnSpeed += difficultyIndex * 0.02f;
    }

    private void Dot_OnDestroy(Dot dot)
    {
        dotList.Remove(dot);
    }

    private IEnumerator SpawnRoutine()
    {
        float elapsedTime = 0;
        float time = 1 / spawnAttr.spawnSpeed;
        while (true)
        {
            if (elapsedTime > time)
            {
                elapsedTime = 0;
                if (dotList.Count < spawnAttr.spawnLimit)
                    SpawnDot();
            }

            elapsedTime += Time.fixedDeltaTime;
            yield return new WaitForFixedUpdate();
        }
    }
    private void SpawnMultipleDots(int count)
    {
        for (int i = 0; i < count; i++)
        {
            SpawnDot();
        }
    }
    private void SpawnDot()
    {
        float randomSpeed = UnityEngine.Random.Range(spawnAttr.baseSpeed, spawnAttr.baseSpeed + spawnAttr.speedRange);

        Transform newDotTransform = Instantiate(dotPrefab, boundingBox.GetRandomPos(), Quaternion.identity);
        Dot dot = newDotTransform.GetComponent<Dot>();
        dot.Initialize(newDotTransform, boundingBox, randomSpeed);

        dotList.Add(dot);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawSphere(boundingBox.lowerLimit, 0.1f);
        Gizmos.color = Color.red;
        Gizmos.DrawSphere(boundingBox.upperLimit, 0.1f);

    }
}

[System.Serializable]
public class SpawnAttr
{
    public float spawnSpeed;
    public float baseSpeed;
    public float speedRange;
    public int spawnLimit;

}
