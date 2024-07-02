using UnityEngine;

[RequireComponent(typeof(PlayerInputs), typeof(PlayerObjects))]
public class PlayerController : MonoBehaviour
{   
    public PlayerObjects objects;
    public PlayerInputs inputs;
    public WalkState walkState;
    public WalkAnimationState walkAnimationState;
    public SolidObjects solidObjects;
    public InteractState interactState;

    void Awake()
    {
        InitStates();
        AwakeStates();
    }
    void Update()
    {
        walkState.UpdateState();
        walkAnimationState.SetSpeed(walkState.currentSpeed.magnitude / walkState.maxSpeed);
        walkAnimationState.Play(inputs.moveInput, walkState.currentSpeed);
        interactState.Interact(Input.GetKeyDown(KeyCode.E), objects.rb.position, inputs.moveInput);
    }
    void FixedUpdate()
    {
        walkState.FixedUpdateState();
    }

    void InitStates()
    {
        walkState = new(this);
        walkAnimationState = new();
        solidObjects = new();
        interactState = new();
    }
    void AwakeStates()
    {
        inputs = GetComponent<PlayerInputs>();
        objects = GetComponent<PlayerObjects>();
        interactState.AwakeState();
        walkAnimationState.AwakeState(GetComponent<Animator>());
    }

    
    
   
}