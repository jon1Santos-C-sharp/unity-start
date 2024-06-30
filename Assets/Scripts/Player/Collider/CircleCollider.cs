using System;
using UnityEngine;

[Serializable]
public class CircleCollider
{   
    public float circleRadius = 0.2f;
    public CircleCollider2D collider;

    public void AwakeState(CircleCollider2D obj){
        collider = obj;
        collider.forceReceiveLayers = LayerMask.GetMask("Default");
    }
}
