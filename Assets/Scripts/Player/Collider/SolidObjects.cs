using System;
using UnityEngine;

[Serializable]
public class SolidObjects
{   
    public float circleRadius = 0.2f;
    private LayerMask solidObjectsLayer;

    public void AwakeState(){
        solidObjectsLayer = LayerMask.GetMask("SolidObjects");
    }
    public bool IsWalkable(Vector2 targetPos)
    {
        Collider2D collider = Physics2D.OverlapCircle(targetPos, circleRadius, solidObjectsLayer);
        return collider == null;
    }
}
