using System.Collections;
using UnityEngine;

public class PlayerController : MonoBehaviour
{

    public float speed = 5f; // Velocidade de movimento do jogador
    private Rigidbody2D rb; // Referência ao componente Rigidbody2D do jogador

    private Animator animator;

    public Hashtable LayerColliderOption;
    [System.Serializable] // Faz com que instâncias públicas dessa classe apareçam no inspector
    public class ColliderOptions
    {
        public LayerMask layer;
        public float colliderRadius = 0.2f;
    }

    public ColliderOptions colliderOptions;

    private float lastDirection;

    private bool isMoving;

    void Awake(){
        animator = GetComponent<Animator>();
    }

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        
        float moveHorizontal = Input.GetAxisRaw("Horizontal"); // Pega o eixo de movimentação horizontal (em qualquer dispositivo) sem filtro de suavização
        float moveVertical = Input.GetAxisRaw("Vertical");
        Vector2 moviment = new (moveHorizontal, moveVertical);
        
        Move(moviment);

        if (moveHorizontal != 0) lastDirection = moveHorizontal; // Manter a sprite da última direção do movimento do personagem
        animator.SetFloat("moveX", moveHorizontal != 0 ? moveHorizontal : lastDirection);
        animator.SetBool("isMoving", isMoving);
    }

    void Move(Vector2 vec){
        
         if (vec != Vector2.zero) // Verificando se o personagem está parado, a fim de setar o atributo isMoving
        {
            Vector2 newPosition = rb.position + speed * Time.fixedDeltaTime * vec;

            if (IsWalkable(newPosition)) // Verificando colisões
            {
                rb.MovePosition(newPosition);
            }

            isMoving = true;
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
