using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputHelper : MonoBehaviour
{
    public static event Action<Vector2> OnSwipe;
    [SerializeField] private float pixelDistToDetect = 100f;
    [SerializeField] private float touchPeriod = 0.05f;
    [SerializeField] private float moveDelay = 0.2f;
    private Vector2 movePos;
    private float touchStartTime;
    private float lastMoveTime;
    private bool moveDone;

    private RaycastHit2D[] results;

    private void Start()
    {
        results = new RaycastHit2D[1];
    }
    private void Update()
    {
        #region Mobile
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);

            if (touch.phase == TouchPhase.Began)
            {
                movePos = Vector2.zero;
                touchStartTime = Time.time;
            }
            else if (touch.phase == TouchPhase.Moved)
            {
                movePos += touch.deltaPosition;
                if (Time.time - lastMoveTime > moveDelay && moveDone == false)
                {
                    if (movePos.magnitude > pixelDistToDetect)
                    {
                        OnSwipe?.Invoke(movePos);
                        movePos = Vector2.zero;
                        touchStartTime = 0;
                        lastMoveTime = Time.time;
                        moveDone = true;
                    }
                }
            }
            else if (touch.phase == TouchPhase.Ended)
            {
                if (Time.time - touchStartTime < touchPeriod)
                {
                    Ray ray = Camera.main.ScreenPointToRay(touch.position);

                    if (Physics2D.RaycastNonAlloc(ray.origin, ray.direction, results) > 0)
                    {
                        if (results[0].transform.TryGetComponent(out IClickable clickable))
                        {
                            clickable.OnClick();
                        }
                    }
                }
                moveDone = false;
            }
        }
        #endregion
        #region PC

        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            if (Physics2D.RaycastNonAlloc(ray.origin, ray.direction, results) > 0)
            {
                if (results[0].transform.TryGetComponent(out IClickable clickable))
                {
                    clickable.OnClick();
                }
            }
        }

        #endregion
    }
}
