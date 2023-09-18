using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class BakcgroundPreview : MonoBehaviour, IPointerClickHandler
{
    [SerializeField] private CanvasGroup panelCg;

    private bool isInPreview = false;
    public void OnPointerClick(PointerEventData eventData)
    {
        isInPreview = !isInPreview;

        if (isInPreview)
        {
            StartCoroutine(FadeOut(panelCg, 0.2f));
        }
        else
        {
            StartCoroutine(FadeIn(panelCg, 0.2f));
        }
    }

    private IEnumerator FadeOut(CanvasGroup cg, float time, Action action = null)
    {
        float elapsedTime = 0f;

        while (elapsedTime <= time)
        {
            cg.alpha = Mathf.Lerp(1, 0, elapsedTime / time);
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        cg.alpha = 0;
        action?.Invoke();
        yield return null;
    }
    private IEnumerator FadeIn(CanvasGroup cg, float time, Action action = null)
    {
        float elapsedTime = 0f;

        while (elapsedTime <= time)
        {
            cg.alpha = Mathf.Lerp(0, 1, elapsedTime / time);
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        cg.alpha = 1;
        action?.Invoke();
        yield return null;
    }
}
