using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameSfxScript : MonoBehaviour
{
    [SerializeField] private GameSfxDataSO gameSfxData;

    private void OnEnable()
    {
        DottedLine.OnDotStackPushed += DottedLine_OnDotStackPushed;
        DottedLine.OnLineCleared += DottedLine_OnLineCleared;
        GameManager.OnGameOver += GameManager_OnGameOver;
    }
    private void OnDisable()
    {
        DottedLine.OnDotStackPushed -= DottedLine_OnDotStackPushed;
        DottedLine.OnLineCleared -= DottedLine_OnLineCleared;
        GameManager.OnGameOver -= GameManager_OnGameOver;
    }
    private void DottedLine_OnDotStackPushed()
    {
        AudioUtility.CreateSFXRandom(gameSfxData.popClips, transform.position, AudioUtility.AudioGroups.SFX, 0);
    }
    private void DottedLine_OnLineCleared(Vector2 pos, int count)
    {
        AudioUtility.CreateSFX(gameSfxData.scoreClip, pos, AudioUtility.AudioGroups.SFX, 0);
    }
    private void GameManager_OnGameOver()
    {
        AudioUtility.CreateSFX(gameSfxData.failClip, transform.position, AudioUtility.AudioGroups.SFX, 0);
    }

}
