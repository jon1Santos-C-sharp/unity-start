using System;
using UnityEngine;

[RequireComponent(typeof(PlayerController), typeof(SolidObjects))]
public class WalkState : State, IState
{   
    private Vector2 newPosition;
    public Vector2 currentSpeed;
    public float maxSpeed = 5.5f;
    public float acceleration = 2f;  // Taxa de aceleração
    public float deceleration = 2.5f;  // Taxa de desaceleração
    private readonly SolidObjects solidObjects;

    public WalkState(PlayerController player) : base(player)
    {
        solidObjects = new();
    }

    public void UpdateState()
    {
        SlideCollider();
    }
    public void FixedUpdateState()
    {
        SetSpeed();
        SetNewPosition();
    }
    
    public void SetSpeed()
    {
        if(player.inputs.moveInput.magnitude > 0)
        {
            currentSpeed = Vector2.Lerp(currentSpeed, player.inputs.moveInput * maxSpeed, Time.fixedDeltaTime * acceleration); 
        }else{
            currentSpeed = Vector2.Lerp(currentSpeed, Vector2.zero, Time.fixedDeltaTime * deceleration);
        }
    }
    public void SetNewPosition()
    {
        newPosition = player.objects.rb.position + currentSpeed * Time.fixedDeltaTime;
        if(solidObjects.IsWalkable(newPosition, player.objects.solidObjectsLayer)) player.objects.rb.position = newPosition;
        else currentSpeed = Vector2.zero;
    }

    public void SlideCollider()
    {
        if(solidObjects.IsWalkable(newPosition, player.objects.solidObjectsLayer)) return;
        if(newPosition.x != player.objects.rb.position.x)
        {
            if(player.inputs.moveInput.y > 0) player.inputs.moveInput.Set(0, 1);
            if(player.inputs.moveInput.y < 0) player.inputs.moveInput.Set(0, -1);    
        }
        if(newPosition.y != player.objects.rb.position.y)
        {
            if(player.inputs.moveInput.x < 0) player.inputs.moveInput.Set(-1, 0);
            if(player.inputs.moveInput.x > 0) player.inputs.moveInput.Set(1, 0);
        }
    }
}
