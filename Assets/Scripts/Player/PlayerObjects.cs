using System;
using UnityEngine;

public class PlayerObjects : MonoBehaviour
{   
    public Rigidbody2D rb;
    public CircleCollider2D circleCollider;
    public LayerMask solidObjectsLayer;

    public void AwakeState(){
    }

    public void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        circleCollider = GetComponent<CircleCollider2D>();
        circleCollider.forceReceiveLayers = LayerMask.GetMask("Default");
        solidObjectsLayer = LayerMask.GetMask("SolidObjects", "InteractableObjects");
    }
}
