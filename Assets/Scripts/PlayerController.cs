using System;
using UnityEngine;

public class PlayerController : MonoBehaviour
{   
    private Rigidbody2D rb; // Componente RigidBody para fins de cálculos físicos
    private float moveHorizontal; // Input de movimento horizontal
    private float moveVertical; // Input de movimento vertical
    private Vector2 inputDirection; // Vetor para armazenar a direção do objeto
    private Vector2 currentVelocity = Vector2.zero; // Vetor para armazenar a velocidade do objeto
    public MoveOptions moveOptions; // Instância para configurações de propriedades relacionadas a movimentação
    private Vector2 newPosition;

    private Animator animator; // Componente para aplicação de animação
 
    private float lastHorizontalDirection; // Var para identificar a última posição após uma movimentação (para fins de animação)
    private bool animationIsMoving; // Var para identificar se o objeto está parado ou em movimento (para fins de animação)

    public ColliderOptions colliderOptions; // Componente para identificação objetos colidíveis 
    private LayerMask solidObjectsLayer;
    public InteractableOptions interactableOptions; // Componente para identificação de objetos interativos
    private LayerMask interactableObjectsLayer;
    private bool isWakablePos;
    void Awake(){
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        solidObjectsLayer = LayerMask.GetMask("SolidObjects");
        interactableObjectsLayer = LayerMask.GetMask("InteractableObjects");
    }
    void Update()
    {
        SetInputs();
        Skills();
        UpdateAnimation();
    }
    private void SetInputs(){
        moveHorizontal = Input.GetAxisRaw("Horizontal"); // Pega o eixo de movimentação horizontal (em qualquer dispositivo) sem filtro de suavização
        moveVertical = Input.GetAxisRaw("Vertical");
        inputDirection = new Vector2(moveHorizontal, moveVertical).normalized;
        SlideColliders();
    }
    private void SlideColliders(){
         if(!isWakablePos){
            if(newPosition.x != rb.position.x){
                if(inputDirection.y > 0) inputDirection.Set(0, 1);
                if(inputDirection.y < 0) inputDirection.Set(0, -1);    
            }
            if(newPosition.y != rb.position.y){
                if(inputDirection.x < 0) inputDirection.Set(-1, 0);
                if(inputDirection.x > 0) inputDirection.Set(1, 0);
            }
        }
    }
    private void Skills(){
        Blink();
    }
    private void Blink(){
        bool blink = Input.GetKeyDown(KeyCode.J);
        if(blink){
             Vector2 targetPos = rb.position + inputDirection * 2;
            if(IsWalkable(targetPos)){
                rb.position = newPosition;
            }else{
                return;
            }
        };
    }
    private void UpdateAnimation(){
        if(moveHorizontal != 0){
            lastHorizontalDirection = moveHorizontal;
        }
        
        animator.SetFloat("moveX", lastHorizontalDirection);
        animator.SetBool("isMoving", animationIsMoving);
    }
    void FixedUpdate()
    {
        Move();
    }
    private void Move(){
        SetVelocity();
        SetNewPosition();
    }
    private void SetVelocity()
    {
        if(inputDirection.magnitude > 0)
        {
            currentVelocity = Vector2.Lerp(currentVelocity, inputDirection * moveOptions.maxSpeed, Time.fixedDeltaTime * moveOptions.acceleration); 
            animationIsMoving = true;
        }
        else
        {
            currentVelocity = Vector2.Lerp(currentVelocity, Vector2.zero, Time.fixedDeltaTime * moveOptions.deceleration);
        }
    }
    private void SetNewPosition()
    {
       if (currentVelocity.magnitude > 0.1f) // Um treshold para verificar se está realmente se movendo
        {
            animationIsMoving = true;
            newPosition = rb.position + currentVelocity * Time.fixedDeltaTime;
            if(IsWalkable(newPosition)){
                rb.MovePosition(newPosition);
                isWakablePos = true;
            }else
            {
                isWakablePos = false;
                currentVelocity = Vector2.zero;
            }
        }
        else
        {
            animationIsMoving = false;
        }
    }
    private bool IsWalkable(Vector2 targetPos)
    {
        var collider = Physics2D.OverlapCircle(targetPos, colliderOptions.radius, solidObjectsLayer);
        var interactable = Physics2D.OverlapCircle(targetPos, interactableOptions.radius, interactableObjectsLayer);
        return collider == null && interactable == null;
        
    }
    
    [Serializable] // Faz com que instâncias públicas dessa classe apareçam no inspector
    public class MoveOptions
    {
        public float maxSpeed = 5.5f;
        public float acceleration = 2f;  // Taxa de aceleração
        public float deceleration = 5.5f;  // Taxa de desaceleração
    }

    [Serializable]
    public class ColliderOptions
    {
        public float radius = 0.2f;
    }

    [Serializable]
    public class InteractableOptions
    {
        public float radius = 0.3f;
    }
}
