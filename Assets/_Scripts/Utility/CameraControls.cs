using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CameraControls : MonoBehaviour
{
    [SerializeField] private CinemachineVirtualCamera virtualCamera;
    [SerializeField] private Transform intersectionTransform;

    private CinemachineImpulseSource impulseSource;

    private void OnEnable()
    {
        GameManager.OnGameOver += GameManager_OnGameOver;
        DottedLine.OnLineCleared += DottedLine_OnLineCleared;
    }
    private void OnDisable()
    {
        GameManager.OnGameOver -= GameManager_OnGameOver;
        DottedLine.OnLineCleared -= DottedLine_OnLineCleared;
    }
    private void Start()
    {
        impulseSource = GetComponent<CinemachineImpulseSource>();
    }

    private void GameManager_OnGameOver()
    {
        virtualCamera.LookAt = intersectionTransform;
        virtualCamera.Follow = intersectionTransform;
        virtualCamera.GetCinemachineComponent<CinemachineFramingTransposer>().m_ScreenY = 0.4f;
    }
    private void DottedLine_OnLineCleared(Vector2 pos, int count)
    {
        impulseSource.GenerateImpulseWithForce(0.2f);
    }
}
