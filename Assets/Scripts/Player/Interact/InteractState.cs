using System;
using UnityEngine;

[Serializable]
public class InteractState
{   
    public float circleRadius = 0.2f;
    public float interactRadius = 0.2f;
    private LayerMask interactableObjectsLayer;

    public void AwakeState(){
        interactableObjectsLayer = LayerMask.GetMask("InteractableObjects");
    }
    public void Interact(bool inputDown, Vector2 currentPos, Vector2 lastDirection)
    {
       if(inputDown){
        Vector2 targetPos = currentPos + lastDirection * interactRadius;
        Collider2D interactableObj = IsInteractable(targetPos);
        if(interactableObj){
           interactableObj.GetComponent<IInteractable>()?.Interact();
        }
       }
    }
    public Collider2D IsInteractable(Vector2 targetPos)
    {
        Collider2D interactable = Physics2D.OverlapCircle(targetPos, circleRadius, interactableObjectsLayer);
        return interactable;
    }
}
