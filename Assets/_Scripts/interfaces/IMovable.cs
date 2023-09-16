using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IMovable
{
    public Transform Transform { get; set; }
    public Vector2 Position { get; }
    public BoundingBox BoundingBox { get; set; }
    public Vector2 TargetPos { get; set; }
    public Vector2 StartPos { get; set; }
    public float Speed { get; set; }

    public void Move();
}
