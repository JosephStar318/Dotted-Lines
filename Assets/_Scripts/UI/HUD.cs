using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class HUD : MonoBehaviour
{
    [SerializeField] private Button pauseBtn;

    private void OnEnable()
    {
        pauseBtn.onClick.AddListener(GameManager.Instance.PauseGame);
    }
    private void OnDisable()
    {
        pauseBtn.onClick.RemoveListener(GameManager.Instance.PauseGame);
    }
}
