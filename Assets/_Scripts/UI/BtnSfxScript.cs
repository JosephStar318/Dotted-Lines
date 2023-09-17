using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class BtnSfxScript : MonoBehaviour, ISelectHandler
{
    [SerializeField] private AudioClip selectSound;
    [SerializeField] private AudioClip clickSound;

    private Button btn;

    private void Start()
    {
        btn = GetComponent<Button>();
        btn.onClick.AddListener(OnClick);
    }
    private void OnDestroy()
    {
        btn.onClick.RemoveListener(OnClick);
    }

    private void OnClick()
    {
        if(clickSound != null)
            AudioUtility.CreateSFX(clickSound, transform.position, AudioUtility.AudioGroups.UI, 0f);
    }
    public void OnSelect(BaseEventData eventData)
    {
        if (selectSound != null)
            AudioUtility.CreateSFX(selectSound, transform.position, AudioUtility.AudioGroups.UI, 0f);
    }

}
