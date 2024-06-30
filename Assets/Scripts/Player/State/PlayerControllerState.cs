using UnityEngine;

public class PlayerControllerState : MonoBehaviour
{   
    public Vector2 moveInput;
    public WalkState walkState;
    public WalkAnimationState walkAnimationState;
    public SolidObjects solidObjects;
    public InteractState interactState;

    public CircleCollider circleCollider;
    void Awake()
    {
        InitStates();
        AwakeStates();
    }
    void Update()
    {
        moveInput = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical")).normalized;
        walkAnimationState.SetSpeed(walkState.currentSpeed.magnitude / walkState.maxSpeed);
        walkAnimationState.Play(moveInput, walkState.currentSpeed);
        SlideCollider(!solidObjects.IsWalkable(walkState.newPosition) && interactState.IsInteractable(walkState.newPosition));
        interactState.Interact(Input.GetKeyDown(KeyCode.E), walkState.rb.position, moveInput);
    }
    void FixedUpdate()
    {
        walkState.FixedUpdateSpeed(moveInput);
        walkState.SetNewPosition();
        walkState.FixedUpdatePosition(solidObjects.IsWalkable(walkState.newPosition) && !interactState.IsInteractable(walkState.newPosition));
    }

    void InitStates()
    {
        walkState = new ();
        walkAnimationState = new();
        solidObjects = new();
        circleCollider = new();
        interactState = new();
    }
    void AwakeStates()
    {
        solidObjects.AwakeState();
        interactState.AwakeState();
        walkState.AwakeState(GetComponent<Rigidbody2D>());
        walkAnimationState.AwakeState(GetComponent<Animator>());
        circleCollider.AwakeState(GetComponent<CircleCollider2D>());
    }
    public void SlideCollider(bool condition)
    {
        if(condition) return;
        if(walkState.newPosition.x != walkState.rb.position.x)
        {
            if(moveInput.y > 0) moveInput.Set(0, 1);
            if(moveInput.y < 0) moveInput.Set(0, -1);    
        }
        if(walkState.newPosition.y != walkState.rb.position.y)
        {
            if(moveInput.x < 0) moveInput.Set(-1, 0);
            if(moveInput.x > 0) moveInput.Set(1, 0);
        }
    }
   
}
