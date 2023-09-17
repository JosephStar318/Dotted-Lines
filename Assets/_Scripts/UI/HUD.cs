using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class HUD : MonoBehaviour
{
    [SerializeField] private Button pauseBtn;
    [SerializeField] private Button speedUpBtn;
    [SerializeField] private Button spawnDotsBtn;

    private void OnEnable()
    {
        pauseBtn.onClick.AddListener(GameManager.Instance.PauseGame);
        speedUpBtn.onClick.AddListener(GameManager.Instance.DifficultyUp);
        spawnDotsBtn.onClick.AddListener(GameManager.Instance.DotsSpawn);
    }
    private void OnDisable()
    {
        pauseBtn.onClick.RemoveListener(GameManager.Instance.PauseGame);
        speedUpBtn.onClick.RemoveListener(GameManager.Instance.DifficultyUp);
        spawnDotsBtn.onClick.RemoveListener(GameManager.Instance.DotsSpawn);
    }
}
