using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VFXManager : MonoBehaviour
{
    [SerializeField] private GameVfxDataSO gameVfxData;
    private void OnEnable()
    {
        Dot.OnDestroy += Dot_OnDestroy;
    }
    private void OnDisable()
    {
        Dot.OnDestroy -= Dot_OnDestroy;
    }

    private void Dot_OnDestroy(Dot dot)
    {
        Instantiate(gameVfxData.dotDestroyParticles, dot.Position, Quaternion.identity);
    }
}
