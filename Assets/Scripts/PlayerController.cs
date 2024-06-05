using System;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private Vector2 inputDirection; // Input de movimentação
    private Vector2 currentVelocity = Vector2.zero; // Como velocidade se trata de uma grandeza vetorial a mesma deve conter módulo (magnitude, direção e sentido)
    private Rigidbody2D rb; // Referência ao componente Rigidbody2D do jogador

    private Animator animator;

    private float lastHorizontalDirection;
    private bool animationIsMoving;
    public ColliderOptions colliderOptions;
    public InteractableOptions interactableOptions;

    public MoveOptions moveOptions;

    void Awake(){
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        colliderOptions.layer = LayerMask.GetMask("SolidObjects");
        interactableOptions.layer = LayerMask.GetMask("InteractableObjects");
    }

    void Update()
    {
        
        float moveHorizontal = Input.GetAxisRaw("Horizontal"); // Pega o eixo de movimentação horizontal (em qualquer dispositivo) sem filtro de suavização
        float moveVertical = Input.GetAxisRaw("Vertical");
        bool blink = Input.GetKeyDown(KeyCode.J);

        inputDirection = new Vector2(moveHorizontal, moveVertical).normalized;
        lastHorizontalDirection = moveHorizontal != 0 ? moveHorizontal : lastHorizontalDirection;
        
        if(blink) rb.position += inputDirection * 2;

        animator.SetFloat("moveX", lastHorizontalDirection);
        animator.SetBool("isMoving", animationIsMoving);
    }

    void FixedUpdate()
    {
        // Calcula a nova velocidade
        if (inputDirection.magnitude > 0)
        {
            currentVelocity = Vector2.Lerp(currentVelocity, inputDirection * moveOptions.maxSpeed, Time.fixedDeltaTime * moveOptions.acceleration); 
        }
        else
        {
            currentVelocity = Vector2.Lerp(currentVelocity, Vector2.zero, Time.fixedDeltaTime * moveOptions.deceleration);
        }

        // Move o personagem com base na nova velocidade calculada
        Move();
    }

    void Move()
    {
       if (currentVelocity.magnitude > 0.1f) // Um treshold para verificar se está realmente se movendo
        {
            Vector2 newPosition = rb.position + currentVelocity * Time.fixedDeltaTime;
            if (IsWalkable(newPosition))
            {
                rb.MovePosition(newPosition);
                animationIsMoving = true;
            }
            else
            {
                currentVelocity = Vector2.zero; // Parar o movimento ao colidir
            }
        }
        else
        {
            animationIsMoving = false;
        }
    }

    bool IsWalkable(Vector2 targetPos)
    {
        var collider = Physics2D.OverlapCircle(targetPos, colliderOptions.radius, colliderOptions.layer);
        var interactable = Physics2D.OverlapCircle(targetPos, interactableOptions.radius, interactableOptions.layer);
        return  collider == null && interactable == null;
    }

    
    [Serializable] // Faz com que instâncias públicas dessa classe apareçam no inspector
    public class MoveOptions
    {
        public float maxSpeed = 5.5f;
        public float acceleration = 2f;  // Taxa de aceleração
        public float deceleration = 5.5f;  // Taxa de desaceleração
    }

    [Serializable] // Faz com que instâncias públicas dessa classe apareçam no inspector
    public class ColliderOptions
    {
        public LayerMask layer;
        public float radius = 0.2f;
    }

    [Serializable] // Faz com que instâncias públicas dessa classe apareçam no inspector
    public class InteractableOptions
    {
        public LayerMask layer;
        public float radius = 0.3f;
    }
}
