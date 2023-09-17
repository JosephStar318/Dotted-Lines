using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public abstract class Panel : MonoBehaviour
{
    protected Animator _anim;
    private void Start()
    {
        _anim = GetComponent<Animator>();
    }
    public virtual void Show()
    {
        _anim.SetTrigger("Show");
    }
    public virtual void Hide()
    {
        _anim.SetTrigger("Hide");
    }
}
