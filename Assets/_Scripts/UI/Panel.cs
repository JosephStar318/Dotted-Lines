using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public abstract class Panel : MonoBehaviour
{
    protected Animator _anim;
    public bool IsActive { get; private set; }
    private void Start()
    {
        _anim = GetComponent<Animator>();
    }
    public virtual void Show()
    {
        _anim.SetTrigger("Show");
        IsActive = true;
    }
    public virtual void Hide()
    {
        _anim.SetTrigger("Hide");
        IsActive = false;
    }
}
