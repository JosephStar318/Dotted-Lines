using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VFXManager : MonoBehaviour
{
    [SerializeField] private GameVfxDataSO gameVfxData;
    [SerializeField] private Material colorfulDotMaterial;

    private void OnEnable()
    {
        Dot.OnDestroy += Dot_OnDestroy;
        DottedLine.OnLineCleared += DottedLine_OnLineCleared;
        GameManager.OnGameOver += GameManager_OnGameOver;
    }
   
    private void OnDisable()
    {
        Dot.OnDestroy -= Dot_OnDestroy;
        DottedLine.OnLineCleared -= DottedLine_OnLineCleared;
        GameManager.OnGameOver -= GameManager_OnGameOver;
    }

    private void Dot_OnDestroy(Dot dot)
    {
        Instantiate(gameVfxData.dotDestroyParticles, dot.Position, Quaternion.identity);
    }
    private void DottedLine_OnLineCleared(Vector2 arg1, int count)
    {
        if (count >= 8)
        {
            SetColorfulDotMaterial(true);
        }
        else
        {
            SetColorfulDotMaterial(false);
        }
    }
    private void GameManager_OnGameOver()
    {
        SetColorfulDotMaterial(false);
    }

    private void SetColorfulDotMaterial(bool state)
    {
        colorfulDotMaterial.SetInt("_Activated", state ? 1 : 0);
    }
}
