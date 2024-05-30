using Unity.VisualScripting;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private Vector2 direction;
    private Vector2 currentPosition = Vector2.zero;
    private Rigidbody2D rb; // Referência ao componente Rigidbody2D do jogador

    private Animator animator;

    private float lastHorizontalDirection;
    private bool isMoving;
    public ColliderOptions colliderOptions;

    public MoveOptions moveOptions;

    void Awake(){
        animator = GetComponent<Animator>();
    }

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        colliderOptions.layer = LayerMask.GetMask("SolidObjects");
    }

    void Update()
    {
        
        float moveHorizontal = Input.GetAxisRaw("Horizontal"); // Pega o eixo de movimentação horizontal (em qualquer dispositivo) sem filtro de suavização
        float moveVertical = Input.GetAxisRaw("Vertical");
        bool blink = Input.GetKeyDown(KeyCode.J);

        lastHorizontalDirection = moveHorizontal != 0 ? moveHorizontal : lastHorizontalDirection;
        direction = new Vector2(moveHorizontal, moveVertical).normalized;
        
        if(blink) rb.position += direction * 2;

        animator.SetFloat("moveX", lastHorizontalDirection);
        animator.SetBool("isMoving", isMoving);
    }

    void FixedUpdate()
    {
        // Calcula a nova velocidade
        if (direction.magnitude > 0)
        {
            currentPosition = Vector2.Lerp(currentPosition, direction * moveOptions.speed, Time.fixedDeltaTime * moveOptions.acceleration);
        }
        else
        {
            currentPosition = Vector2.Lerp(currentPosition, Vector2.zero, Time.fixedDeltaTime * moveOptions.deceleration);
        }

        // Move o personagem com base na nova velocidade calculada
        Move();
    }

    void Move()
    {
       if (currentPosition.magnitude > 0.1f) // Um pequeno valor para verificar se está realmente se movendo
        {
            Vector2 newPosition = rb.position + currentPosition * Time.fixedDeltaTime;
            if (IsWalkable(newPosition))
            {
                rb.MovePosition(newPosition);
                isMoving = true;
            }
            else
            {
                currentPosition = Vector2.zero; // Parar o movimento ao colidir
            }
        }
        else
        {
            isMoving = false;
        }
    }

    bool IsWalkable(Vector2 targetPos)
    {
        Collider2D collider = Physics2D.OverlapCircle(targetPos, colliderOptions.colliderRadius, colliderOptions.layer);
        return collider == null;
    }

    
    [System.Serializable] // Faz com que instâncias públicas dessa classe apareçam no inspector
    public class MoveOptions
    {
        public float speed = 5.5f;
        public float acceleration = 2f;  // Taxa de aceleração
        public float deceleration = 5.5f;  // Taxa de desaceleração
    }

    [System.Serializable] // Faz com que instâncias públicas dessa classe apareçam no inspector
    public class ColliderOptions
    {
        public LayerMask layer;
        public float colliderRadius = 0.3f;
    }
}
