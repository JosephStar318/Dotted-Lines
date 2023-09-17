using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Reflection;
using System.Linq;
using System;

public class UIManager : MonoBehaviour
{
    private static Dictionary<System.Type, Panel> panelDictionary = new Dictionary<System.Type, Panel>();
    private void Awake()
    {
        panelDictionary.Clear();

        panelDictionary.Add(typeof(GameOverPanel), FindObjectOfType<GameOverPanel>());
        panelDictionary.Add(typeof(PausePanel), FindObjectOfType<PausePanel>());

    }
    public static void Open<T>() where T : Panel
    {
        foreach (var panelEntry in panelDictionary)
        {
            if (panelEntry.Key.Equals(typeof(T))) continue;

            if(panelEntry.Value.IsActive)
                panelEntry.Value.Hide();
        }
        panelDictionary[typeof(T)].Show();
    }
    public static void Close<T>() where T : Panel
    {
        panelDictionary[typeof(T)].Hide();
    }
}
