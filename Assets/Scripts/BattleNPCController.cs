using UnityEngine;

public class BattleNPCController : MonoBehaviour, IInteractable, INPC
{
    private Animator animator;
    private Rigidbody2D rb;
    private LayerMask playerLayer;
    public float playerDetectionRadius = 4.7f;
    public void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        playerLayer = LayerMask.GetMask("Default");
    }
    public void Update()
    {
        FlipHorizontal();
    }
    public void FlipHorizontal()
    {
        Collider2D hasInteract = Physics2D.OverlapCircle(rb.position, playerDetectionRadius, playerLayer);
        if(hasInteract)
        {
            Rigidbody2D PlayerRB = hasInteract.GetComponent<Rigidbody2D>();
            float playerPosition = PlayerRB.position.magnitude;
            float npcPosition = rb.position.magnitude;
            if(playerPosition < npcPosition){
                animator.SetFloat("Flip", -1);
            }else{
                animator.SetFloat("Flip", 1);
            }
        }
    }
    public void Interact()
    {
        Debug.Log("Battle NPC");
    }
}