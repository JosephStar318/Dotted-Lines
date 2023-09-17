using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DottedLine : MonoBehaviour
{
    public static event Action<Vector2, int> OnLineCleared;
    public static event Action OnDotStackPushed;

    [SerializeField] private LineRenderer lineRenderer;
    [SerializeField] private Transform selectionTransform;

    private List<EdgeCollider2D> edgeColliders = new List<EdgeCollider2D>();

    private Stack<Dot> dotStack = new Stack<Dot>();
    private List<Vector3> points = new List<Vector3>(20);

    private Queue<Dot> targetDotQueue = new Queue<Dot>();
    private Dot targetDot;

    [SerializeField] private float targetLerpTime = 0.5f;
    private float elapsedLerpTime = 0;

    public List<Vector3> GetPoints => points;
    private void OnEnable()
    {
        GameManager.OnGameOver += GameManager_OnGameOver;
        Dot.OnClicked += Dot_OnClicked;
    }
    private void OnDisable()
    {
        GameManager.OnGameOver -= GameManager_OnGameOver;
        Dot.OnClicked -= Dot_OnClicked;
    }
    private void GameManager_OnGameOver()
    {
        this.enabled = false;
    }
    private void FixedUpdate()
    {
        UpdatePositions();
        UpdateSelectionTransform();
        UpdateLineRenderer();
        UpdateColliders();
    }

    private void UpdatePositions()
    {
        points.Clear();
        if (targetDot != null)
            points.Add(selectionTransform.position);

        foreach (var dot in dotStack)
        {
            points.Add(dot.Position);
        }
    }
    private void UpdateSelectionTransform()
    {
        if (targetDot != null)
        {
            selectionTransform.position = Vector2.Lerp(selectionTransform.position, targetDot.Position, elapsedLerpTime / targetLerpTime);
            elapsedLerpTime += Time.fixedDeltaTime;

            if (elapsedLerpTime > targetLerpTime)
            {
                elapsedLerpTime = 0;

                if (dotStack.Contains(targetDot))
                {
                    DestroyUntilDot(targetDot);
                }
                else
                {
                    dotStack.Push(targetDot);
                    OnDotStackPushed?.Invoke();
                }

                if (targetDotQueue.Count > 0) targetDot = targetDotQueue.Dequeue();
                else targetDot = null;
            }
        }
        if (targetDot == null && dotStack.Count > 0)
        {
            selectionTransform.position = dotStack.Peek().Position;
        }

    }
    private void UpdateLineRenderer()
    {
        lineRenderer.positionCount = points.Count;
        lineRenderer.SetPositions(points.ToArray());
    }
    private void UpdateColliders()
    {
        List<Vector2> points2D = new List<Vector2>(points.Count);
        foreach (Vector3 point in points)
        {
            points2D.Add(point);
        }
        for (int i = 0; i < points2D.Count - 1; i++)
        {
            EdgeCollider2D edgeCollider2D;
            if (edgeColliders.Count < i + 1)
            {
                edgeCollider2D = gameObject.AddComponent<EdgeCollider2D>();
                edgeColliders.Add(edgeCollider2D);
            }
            else
            {
                edgeCollider2D = edgeColliders[i];
            }
            edgeCollider2D.SetPoints(points2D.GetRange(i, 2));
        }

        //remove dirty colliders
        if(edgeColliders.Count > 0 && edgeColliders.Count > points2D.Count - 1)
        {
            int startIndex = points2D.Count - 1;
            int count = edgeColliders.Count - startIndex;
            List<EdgeCollider2D> redundantColliders = edgeColliders.GetRange(startIndex, count);
            foreach (var coll in redundantColliders)
            {
                Destroy(coll);
            }
            edgeColliders.RemoveRange(startIndex, count);
        }
    }

    private void Dot_OnClicked(Dot dot)
    {
        if (dotStack.Count == 0)
        {
            selectionTransform.position = dot.Position;
            selectionTransform.gameObject.SetActive(true);
            dotStack.Push(dot);
            OnDotStackPushed?.Invoke();
        }
        else
        {
            Dot lastDot = dotStack.Peek();
            if (dot == lastDot || dot == targetDot)
            {
                return;
            }
            if (targetDot == null)
            {
                targetDot = dot;
            }
            else
            {
                targetDotQueue.Enqueue(dot);
            }
        }
    }

    private void DestroyUntilDot(Dot dot)
    {
        bool dotDestroyed = false;
        int destroyCounter = 0;
        while (dotDestroyed == false)
        {
            Dot tempDot = dotStack.Pop();
            if (tempDot == dot)
            {
                dotDestroyed = true;
            }
            tempDot.Destroy();
            points.RemoveAt(points.Count - 1);
            destroyCounter++;
        }

        if (dotStack.Count == 0)
        {
            selectionTransform.gameObject.SetActive(false);
        }
        OnLineCleared?.Invoke(dot.Position, destroyCounter);
    }
}
