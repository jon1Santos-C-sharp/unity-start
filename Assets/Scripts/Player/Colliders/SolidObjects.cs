using System;
using UnityEngine;

[Serializable]
public class SolidObjects
{   
    public float circleRadius = 0.2f;
   
    public bool IsWalkable(Vector2 targetPos, LayerMask solidObjectsLayer)
    {
        Collider2D collider = Physics2D.OverlapCircle(targetPos, circleRadius, solidObjectsLayer);
        return collider == null;
    }
}
