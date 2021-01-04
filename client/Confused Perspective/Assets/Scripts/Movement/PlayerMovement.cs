using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float speed = 6.0f;
    public float jumpspeed = 8.0f;
    public float gravity = -20.0f;
    private Vector3 moveDir = Vector3.zero;
    void Update()
    {
        CharacterController controller = GetComponent<CharacterController>();
        if (controller.isGrounded)
        {
            moveDir = new Vector3(Input.GetAxis("Horizontal"),0,Input.GetAxis("Vertical"));
            moveDir = transform.TransformDirection(moveDir);
            moveDir *= speed;
            if (Input.GetButton("Jump"))
                moveDir.y = jumpspeed;
        }

        moveDir.y += gravity * Time.deltaTime;
        controller.Move(moveDir * Time.deltaTime);
    }
}
