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
    [SerializeField] private LayerMask dotLayer;
    [Range(0,1)][SerializeField] private float estimationDistanceWeight;

    private List<EdgeCollider2D> edgeColliders = new List<EdgeCollider2D>();

    private Stack<Dot> dotStack = new Stack<Dot>();
    private List<Vector3> points = new List<Vector3>(20);

    private Queue<Dot> targetDotQueue = new Queue<Dot>();
    private Dot targetDot;
    private Dot secondLastDot;

    [SerializeField] private float targetLerpTime = 0.5f;
    private float elapsedLerpTime = 0;

    public List<Vector3> GetPoints => points;

    private void OnEnable()
    {
        GameManager.OnGameOver += GameManager_OnGameOver;
        Dot.OnClicked += Dot_OnClicked;
        InputHelper.OnSwipe += InputHelper_OnSwipe;
    }
    private void OnDisable()
    {
        GameManager.OnGameOver -= GameManager_OnGameOver;
        Dot.OnClicked -= Dot_OnClicked;
        InputHelper.OnSwipe -= InputHelper_OnSwipe;
    }
    private void GameManager_OnGameOver()
    {
        this.enabled = false;
    }
    IEnumerator Start()
    {
        yield return new WaitForSeconds(1);
        SelectFirstDot(FindObjectOfType<Dot>());
    }
    private void FixedUpdate()
    {
        UpdatePositions();
        UpdateDots();
        UpdateLineRenderer();
        UpdateColliders();
    }

    private void UpdatePositions()
    {
        points.Clear();
        if (targetDot != null && Vector2.Distance(selectionTransform.position, dotStack.Peek().Position) > 0.2f)
            points.Add(selectionTransform.position);

        foreach (var dot in dotStack)
        {
            points.Add(dot.Position);
        }
    }
    private void UpdateDots()
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
                    DestroyLoopingDots(targetDot);
                }
                else
                {
                    secondLastDot = dotStack.Peek();
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
        if (edgeColliders.Count > 0 && edgeColliders.Count > points2D.Count - 1)
        {
            int startIndex = Mathf.Clamp(points2D.Count - 1, 0 ,points2D.Count);
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
            SelectFirstDot(dot);
    }

    private void SelectFirstDot(Dot dot)
    {
        selectionTransform.position = dot.Position;
        selectionTransform.gameObject.SetActive(true);
        dotStack.Push(dot);
        OnDotStackPushed?.Invoke();
    }

    private void InputHelper_OnSwipe(Vector2 dir)
    {
        if (dotStack.Count == 0) return;
        if (FindDotInDirection(dir, out Dot dot))
        {
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

    private bool FindDotInDirection(Vector2 dir, out Dot dot)
    {
        Dot lastDot = dotStack.Peek();
        dot = null;
        float minDotEstimation = float.MaxValue;
        Dot estimatedDot = null;

        RaycastHit2D[] hits = Physics2D.CircleCastAll(lastDot.Position, 0.5f, dir, 100, dotLayer);
        if (hits.Length > 0)
        {
            foreach (RaycastHit2D hit in hits)
            {
                dot = hit.transform.GetComponent<Dot>();

                //Make sure it's not selecting itself
                if (dot == lastDot || dot == targetDot)
                {
                    continue;
                }
                //Make sure it's not selecting the previous dot
                if (secondLastDot != null && secondLastDot == dot)
                {
                    continue;
                }

                float angle = Vector2.Angle(dot.Position - lastDot.Position, dir);
                //Make sure dot is in the direction
                if (angle > 20)
                {
                    continue;
                }

                //Take most inline of the closest dots
                float distance = Vector2.Distance(dot.Position, lastDot.Position);
                float dotEstimation = angle * (1- estimationDistanceWeight) + distance * estimationDistanceWeight;

                if (dotEstimation < minDotEstimation)
                {
                    minDotEstimation = dotEstimation;
                    estimatedDot = dot;
                }

            }
            dot = estimatedDot;
            return true;
        }
        return false;
    }

    private void DestroyLoopingDots(Dot dot)
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
        OnLineCleared?.Invoke(dot.Position, destroyCounter);

        if (dotStack.Count == 0)
        {
            SetSelectionTransformState();
        }
    }

    private void SetSelectionTransformState()
    {
        Dot[] dots = FindObjectsOfType<Dot>();

        foreach (Dot randomDot in dots)
        {
            if (randomDot.IsDirty == false)
            {
                SelectFirstDot(randomDot);
                return;
            }
        }
        selectionTransform.gameObject.SetActive(false);
    }
}
