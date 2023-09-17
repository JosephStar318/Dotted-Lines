using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameOverPanel : Panel
{
    [SerializeField] private Button replayBtn;
    public override void Show()
    {
        base.Show();
        replayBtn.onClick.AddListener(() => GameManager.Instance.RestartGame());
    }
}
