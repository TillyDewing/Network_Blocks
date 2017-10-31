using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class PlayerMovement : MonoBehaviour
{
    public float speed = 6f;
    public float sensitivityX = 50;
    public float sensitivityY = 50;
    public float gravity = 20f;
    public float jumpSpeed = 10;
    public GameObject Camera;
    CharacterController controller;
    Vector3 moveDirection = Vector3.zero;
    private void Awake()
    {
        controller.GetComponent<CharacterController>();
    }

    private void Update()
    {
        
        
        if (controller.isGrounded)
        {
            moveDirection = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
            moveDirection = transform.TransformDirection(moveDirection);
            moveDirection *= speed;
            if (Input.GetButton("Jump"))
            {
                moveDirection.y = jumpSpeed;
            }

        }

        moveDirection.y -= gravity;
        controller.Move(moveDirection);
    }
}
