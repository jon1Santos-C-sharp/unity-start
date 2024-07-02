using UnityEngine;

public class PlayerInputs : MonoBehaviour
{   
    public Vector2 moveInput;
    void Update()
    {
        moveInput = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical")).normalized;
    }
}