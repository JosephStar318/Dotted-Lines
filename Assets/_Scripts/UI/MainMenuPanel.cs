using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainMenuPanel : MonoBehaviour
{
    [SerializeField] private Button playButton;
    private void OnEnable()
    {
        playButton.onClick.AddListener( ()=> GameManager.Instance.StartGame());
    }
    private void OnDisable()
    {
        playButton.onClick.RemoveAllListeners();
    }
}
