using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class ToggleBar : MonoBehaviour
{
    [SerializeField] private bool isOn = true;

    private Slider slider;
    private Animation wobbleAnim;

    public UnityEvent<bool> OnToggle;

    private Button button;
    private bool isInitialized;

    private void Start()
    {
        if(isInitialized == false)
            Init();
        button.onClick.AddListener(OnBtnToggle);
    }

    private void Init()
    {
        slider = GetComponentInChildren<Slider>();
        wobbleAnim = GetComponentInChildren<Animation>();
        button = GetComponent<Button>();

        isInitialized = true;
    }

    private void OnDestroy()
    {
        button.onClick.RemoveListener(OnBtnToggle);
    }
    private void OnBtnToggle()
    {
        isOn = !isOn;
        if (isOn)
        {
            slider.value = 1;
        }
        else
        {
            slider.value = 0;
        }
        wobbleAnim.Play();
        OnToggle?.Invoke(isOn);
    }
    public void UpdateStateWithoutNotify(bool state)
    {
        if (isInitialized == false)
            Init();

        isOn = state;
        if (isOn)
        {
            slider.value = 1;
        }
        else
        {
            slider.value = 0;
        }
        wobbleAnim.Play();
    }
}
