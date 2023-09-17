using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IntersectionIndicator : MonoBehaviour
{
    private void Start()
    {
        LineCollisionChecker.OnLineCollision += LineCollisionChecker_OnLineCollision;
        gameObject.SetActive(false);
    }
    private void OnDestroy()
    {
        LineCollisionChecker.OnLineCollision -= LineCollisionChecker_OnLineCollision;
    }
    private void LineCollisionChecker_OnLineCollision(Vector2 pos)
    {
        transform.position = pos;
        gameObject.SetActive(true);
    }
}
