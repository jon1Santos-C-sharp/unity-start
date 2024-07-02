using System;
using UnityEngine;

[Serializable]
public class WalkAnimationState
{   
    private Animator animator;
    public float walkTresholdValue = 0.75f;
    public bool animationIsMoving;

    public void AwakeState(Animator obj)
    {
        animator = obj;
    }
    public void SetSpeed(float speedBase)
    {
        if(speedBase <= walkTresholdValue)
        {
            animator.speed = walkTresholdValue;
        }else{
            animator.speed = speedBase;
        }
    }
    public void Play(Vector2 input, Vector2 currentSpeed)
    {
        if(input.magnitude > 0)
        {
            animationIsMoving = true;
            animator.Play("Walk");
        }
        
        if(input.magnitude == 0 && currentSpeed.magnitude < walkTresholdValue)
        {
            animationIsMoving = false;
        }

        animator.SetBool("isMoving", animationIsMoving);
        if(input.x != 0) animator.SetFloat("moveX", input.x);
    }
}
