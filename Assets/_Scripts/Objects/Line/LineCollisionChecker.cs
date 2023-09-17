using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LineCollisionChecker : MonoBehaviour
{
    public static event Action<Vector2> OnLineCollision;

    [SerializeField] private LayerMask collisionLayer;

    private DottedLine _dottedLine;

    private RaycastHit2D[] results = new RaycastHit2D[3];

    private void OnEnable()
    {
        GameManager.OnGameOver += GameManager_OnGameOver;
        GameManager.OnGamePaused += GameManager_OnGamePaused;
        GameManager.OnGameUnpaused += GameManager_OnGameUnpaused;
    }
    private void OnDisable()
    {
        GameManager.OnGameOver -= GameManager_OnGameOver;
        GameManager.OnGamePaused -= GameManager_OnGamePaused;
        GameManager.OnGameUnpaused -= GameManager_OnGameUnpaused;
    }
    private void Start()
    {
        _dottedLine = GetComponent<DottedLine>();
    }
    private void FixedUpdate()
    {
        List<Vector3> points = _dottedLine.GetPoints;

        for (int i = 0; i < points.Count - 1; i++)
        {
            int collisionCount = Physics2D.LinecastNonAlloc(points[i], points[i + 1], results, collisionLayer);
            if (collisionCount >= 3)
            {
                Vector2 hitPoint = results[2].point;
                if (Vector2.Distance(hitPoint, points[i]) < 0.05f ||
                    Vector2.Distance(hitPoint, points[i + 1]) < 0.05f)
                {
                    continue;
                }
                else
                {
                    Debug.DrawLine(Vector2.zero, hitPoint, Color.green);
                    OnLineCollision?.Invoke(hitPoint);
                    return;
                }
            }
            else if (collisionCount == 2)
            {
                EdgeCollider2D edgeCollider1 = (EdgeCollider2D)results[0].collider;
                EdgeCollider2D edgeCollider2 = (EdgeCollider2D)results[1].collider;
                Vector2 dir1 = edgeCollider1.points[0] - edgeCollider1.points[1];
                Vector2 dir2 = edgeCollider2.points[0] - edgeCollider2.points[1];

                float angle = Vector2.Angle(dir1, -dir2);
                if (Mathf.Abs(angle) < 1f)
                {
                    Vector2 hitPoint = points[i];
                    Debug.DrawLine(Vector2.zero, hitPoint, Color.yellow);
                    OnLineCollision?.Invoke(hitPoint);
                    return;
                }
            }
        }

       
    }

    private void GameManager_OnGamePaused()
    {
        this.enabled = false;
    }

    private void GameManager_OnGameOver()
    {
        this.enabled = false;
    }
    private void GameManager_OnGameUnpaused()
    {
        this.enabled = true;
    }
}
