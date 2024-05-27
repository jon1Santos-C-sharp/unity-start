using Unity.VisualScripting;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private Vector2 inputDirections;
    private Vector2 currentVelocity = Vector2.zero;
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
        inputDirections = new Vector2(moveHorizontal, moveVertical).normalized;
        if (inputDirections.magnitude > 0)
        {
            lastHorizontalDirection = moveHorizontal != 0 ? moveHorizontal : lastHorizontalDirection;
        }

        animator.SetFloat("moveX", lastHorizontalDirection);
        animator.SetBool("isMoving", isMoving);
    }

    void FixedUpdate()
    {
        // Calcula a nova velocidade
        if (inputDirections.magnitude > 0)
        {
            currentVelocity = Vector2.Lerp(currentVelocity, inputDirections * moveOptions.speed, Time.fixedDeltaTime * moveOptions.acceleration);
        }
        else
        {
            currentVelocity = Vector2.Lerp(currentVelocity, Vector2.zero, Time.fixedDeltaTime * moveOptions.deceleration);
        }

        // Move o personagem com base na nova velocidade calculada
        Move(currentVelocity);
    }

    void Move(Vector2 moveVec){
        
       if (moveVec.magnitude > 0.1f) // Um pequeno valor para verificar se está realmente se movendo
        {
            Vector2 newPosition = rb.position + moveVec * Time.fixedDeltaTime;
            if (IsWalkable(newPosition))
            {
                rb.MovePosition(newPosition);
                isMoving = true;
            }
            else
            {
                currentVelocity = Vector2.zero; // Parar o movimento ao colidir
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
        public float deceleration = 5f;  // Taxa de desaceleração
    }

    [System.Serializable] // Faz com que instâncias públicas dessa classe apareçam no inspector
    public class ColliderOptions
    {
        public LayerMask layer;
        public float colliderRadius = 0.3f;
    }

    // public float moveSpeed;

    // public bool isMoving = false;

    // private Vector2 input;
    // private void Update(){
    //     // if(!isMoving){
    //         input.x = Input.GetAxisRaw("Horizontal");
    //         input.y = Input.GetAxisRaw("Vertical");

    //         if(input != Vector2.zero){
    //             var targetPos = transform.position;
    //             targetPos.x += input.x;
    //             targetPos.y += input.y;

    //             StartCoroutine(Move(targetPos));
    //         }
    //     // }
    // }

    // IEnumerator Move(Vector3 targetPos){
    //     while((targetPos - transform.position).sqrMagnitude > Mathf.Epsilon){
    //         transform.position = Vector3.MoveTowards(transform.position, targetPos, moveSpeed * Time.fixedDeltaTime);
    //         yield return null;
    //     }
    //     transform.position = targetPos;
    //     // isMoving = false;
    // }
}
