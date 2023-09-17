using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class PausePanel : Panel
{
    [SerializeField] private Button homeBtn;
    [SerializeField] private Button restartBtn;
    [SerializeField] private Button continueBtn;
    [SerializeField] private ToggleBar soundTougleBar;

    private void OnEnable()
    {
        homeBtn.onClick.AddListener(GameManager.Instance.ReturnMainMenu);
        restartBtn.onClick.AddListener(GameManager.Instance.RestartGame);
        continueBtn.onClick.AddListener(GameManager.Instance.UnpauseGame);
        soundTougleBar.OnToggle.AddListener(SetSound);
        LoadPrefs();
    }
    private void OnDisable()
    {
        homeBtn.onClick.RemoveListener(GameManager.Instance.ReturnMainMenu);
        restartBtn.onClick.RemoveListener(GameManager.Instance.RestartGame);
        continueBtn.onClick.RemoveListener(GameManager.Instance.UnpauseGame);
        soundTougleBar.OnToggle.RemoveListener(SetSound);
    }
   
    private void LoadPrefs()
    {
        int volume = PlayerprefsHelper.GetSoundState();
        AudioUtility.SetMasterVolume(volume);
        soundTougleBar.UpdateStateWithoutNotify(volume == 100 ? true : false);
    }
    private void SetSound(bool state)
    {
        int volume = state == true ? 100 : 0;
        AudioUtility.SetMasterVolume(volume);
        PlayerprefsHelper.SetSoundState(volume);
    }
}
