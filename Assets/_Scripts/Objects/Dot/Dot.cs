using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dot : MonoBehaviour, IMovable, IClickable
{
    public static event Action<Dot> OnClicked;
    public static event Action<Dot> OnDestroy;
    public Transform Transform { get; set; }
    public Vector2 Position => Transform.position;
    public BoundingBox BoundingBox { get; set; }
    public Vector2 StartPos { get; set; }
    public Vector2 TargetPos { get; set; }
    public float Speed { get; set; }
    public bool IsDirty { get; private set; }

    [SerializeField] private Animation _anim;

    float elapsedTime = 0;
    public void Initialize(Transform trform, BoundingBox boundingBox, float speed)
    {
        Transform = trform;
        BoundingBox = boundingBox;
        Speed = speed;

        StartPos = Position;
        TargetPos = BoundingBox.GetRandomPos();
    }

    public void Move()
    {
        float time = 100 / Speed;
        Transform.position = Vector2.Lerp(StartPos, TargetPos, elapsedTime / time);

        elapsedTime += Time.fixedDeltaTime;
        if (elapsedTime > time)
        {
            StartPos = Position;
            TargetPos = BoundingBox.GetRandomPos();
            elapsedTime = 0;
        }
    }

    public void OnClick()
    {

        OnClicked?.Invoke(this);
    }
    public void Destroy()
    {
        _anim.Play("Dot_Destroy");
        GetComponent<Collider2D>().enabled = false;
        IsDirty = true;
        Destroy(gameObject,1f);
        OnDestroy?.Invoke(this);
    }
   
}
