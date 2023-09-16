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
    private bool _isGameOver;
    private void Start()
    {
        dotList = new List<Dot>(spawnAttr.spawnLimit);
        Dot.OnDestroy += Dot_OnDestroy;
        GameManager.OnGameOver += GameManager_OnGameOver;

        SpawnDot();
        StartCoroutine(SpawnRoutine());
    }
    private void OnDestroy()
    {
        Dot.OnDestroy -= Dot_OnDestroy;
        GameManager.OnGameOver -= GameManager_OnGameOver;
    }

    private void FixedUpdate()
    {
        if (_isGameOver) return;

        foreach (Dot dot in dotList)
        {
            dot.Move();
        }
    }
    private void GameManager_OnGameOver()
    {
        _isGameOver = true;
        StopAllCoroutines();
    }

    private void Dot_OnDestroy(Dot dot)
    {
        dotList.Remove(dot);
    }

    private IEnumerator SpawnRoutine()
    {
        float elapsedTime = 0;
        while (true)
        {
            if (elapsedTime > spawnAttr.spawnPeriod)
            {
                elapsedTime = 0;
                if (dotList.Count < spawnAttr.spawnLimit)
                    SpawnDot();
            }

            elapsedTime += Time.fixedDeltaTime;
            yield return new WaitForFixedUpdate();
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
    public float spawnPeriod;
    public float baseSpeed;
    public float speedRange;
    public int spawnLimit;

}
