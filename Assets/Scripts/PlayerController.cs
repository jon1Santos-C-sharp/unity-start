using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Burst;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerController : MonoBehaviour
{

    public float speed = 5f; // Velocidade de movimento do jogador
    private Rigidbody2D rb; // Referência ao componente Rigidbody2D do jogador

    private Animator animator;

    private float lastDirection;

    private bool isMoving;
    void Awake(){
        animator = GetComponent<Animator>();
    }

    void Start()
    {
        rb = GetComponent<Rigidbody2D>(); // Obtém a referência ao Rigidbody2D do jogador
    }

    // Update is called once per frame
    void Update()
    {
        float moveHorizontal = Input.GetAxis("Horizontal"); // Obtém a entrada horizontal (teclas de seta ou A e D)
        float moveVertical = Input.GetAxis("Vertical");

        Vector2 movement = new();

        movement.y += moveVertical;
        movement.x += moveHorizontal;

        if(moveHorizontal != 0) lastDirection = moveHorizontal;
        if(moveHorizontal != 0 || moveVertical != 0) isMoving = true;
        else isMoving = false;
        animator.SetFloat("moveX", moveHorizontal != 0 ? moveHorizontal : lastDirection);
        animator.SetBool("isMoving", isMoving);

        rb.velocity = movement * speed; // Aplica o movimento ao Rigidbody2D do jogador
    }


    // NO RIGIDBODY2 MOVE

    // public float moveSpeed;

    // public bool isMoving;

    // private Vector2 input;
    // private void Update(){
    //     if(!isMoving){
    //         input.x = Input.GetAxis("Horizontal");
    //         input.x = Input.GetAxis("Vertical");

    //         if(input != Vector2.zero){
    //             var targetPos = transform.position;
    //             targetPos.x += input.x;
    //             targetPos.y += input.y;

    //             StartCoroutine(Move(targetPos))
    //         }
    //     }
    // }

    // IEnumerator Move(Vector3 targetPos){
    //     while((targetPos - transform.position).sqrMagnitude > Mathf.Epsilon){
    //         transform.position = Vector3.MoveTowards(transform.position, targetPos, moveSpeed * Time.deltaTime);
    //         yield return null;
    //     }
    //     transform.position = targetPos;
    //     isMoving = false;
    // }
}
