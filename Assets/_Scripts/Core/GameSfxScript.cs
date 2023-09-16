using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameSfxScript : MonoBehaviour
{
    [SerializeField] private GameSfxDataSO gameSfxData;

    private void OnEnable()
    {
        DottedLine.OnDotStackPushed += DottedLine_OnDotStackPushed;
    }
    private void OnDisable()
    {
        DottedLine.OnDotStackPushed -= DottedLine_OnDotStackPushed;
    }
    private void DottedLine_OnDotStackPushed()
    {
        AudioUtility.CreateSFXRandom(gameSfxData.popClips, transform.position, AudioUtility.AudioGroups.SFX, 0);
    }
}
