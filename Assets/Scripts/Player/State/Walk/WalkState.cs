using System;
using UnityEngine;

[Serializable]
public class WalkState
{   
    public Rigidbody2D rb;
    public Vector2 currentSpeed;

    public Vector2 newPosition;
    public float maxSpeed = 5.5f;
    public float acceleration = 2f;  // Taxa de aceleração
    public float deceleration = 2.5f;  // Taxa de desaceleração

    public void AwakeState(Rigidbody2D obj)
    {
        rb = obj;
    }
    public void FixedUpdateSpeed(Vector2 input)
    {
        if(input.magnitude > 0){
            currentSpeed = Vector2.Lerp(currentSpeed, input * maxSpeed, Time.fixedDeltaTime * acceleration); 
        }else{
            currentSpeed = Vector2.Lerp(currentSpeed, Vector2.zero, Time.fixedDeltaTime * deceleration);
        }
    }
    public void SetNewPosition()
    {
        newPosition = rb.position + currentSpeed * Time.fixedDeltaTime;
    }
    public void FixedUpdatePosition(bool condition)
    {
        if(condition) rb.position = newPosition;
        else currentSpeed = Vector2.zero;
    }
}
