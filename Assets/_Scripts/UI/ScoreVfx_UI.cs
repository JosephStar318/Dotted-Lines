using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreVfx_UI : MonoBehaviour
{
    [SerializeField] private GameObject scoreVfxPrefab;

    private void OnEnable()
    {
        ScoreManager.OnScoreCalculated += ScoreManager_OnScoreCalculated;
    }
    private void OnDisable()
    {
        ScoreManager.OnScoreCalculated -= ScoreManager_OnScoreCalculated;
    }
    private void ScoreManager_OnScoreCalculated(Vector2 position, int score)
    {
        Vector2 point = Camera.main.WorldToScreenPoint(position);
        GameObject vfx = Instantiate(scoreVfxPrefab, transform);
        vfx.transform.position = point;
        vfx.GetComponent<TMPro.TextMeshProUGUI>().SetText($"+{score}");
    }
}
