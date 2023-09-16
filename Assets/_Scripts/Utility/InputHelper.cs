using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputHelper : MonoBehaviour
{
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
            Ray ray = Camera.main.ScreenPointToRay(touch.position);
           
            if(Physics2D.RaycastNonAlloc(ray.origin, ray.direction, results) > 0)
            {
                if(results[0].transform.TryGetComponent(out IClickable clickable))
                {
                    clickable.OnClick();
                }
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
