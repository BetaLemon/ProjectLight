using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {

    CharacterController controller;
    public float speed = 6.0f;
    public float gravity = 1.0f;
    private Vector3 moveDirection;
    private Quaternion lerpLook;

    void Start()
    {
        controller = GetComponent<CharacterController>();
        moveDirection = Vector3.zero;
    }

    void FixedUpdate()
    {
        moveDirection.x = Input.GetAxis("Horizontal") * speed;
        moveDirection.z = Input.GetAxis("Vertical") * speed;

        if (!controller.isGrounded)
        {
            moveDirection.y -= gravity;
        }
        else
        {
            moveDirection.y = 0;
        }

        if (Input.GetButton("Jump"))
            moveDirection.y = 20;

        controller.Move(moveDirection * Time.deltaTime);

        if (moveDirection.x != 0 || moveDirection.z != 0)
        {
            lerpLook = Quaternion.LookRotation(new Vector3(moveDirection.x, 0, moveDirection.z));
        }

        transform.rotation = Quaternion.Slerp (transform.rotation, lerpLook, Time.deltaTime * speed);

        /*
        if (controller.isGrounded)
        {
            //moveDirection.y = 0;
            tmp = jumpSpeed *Input.GetAxis("Jump");
            print(Input.GetAxis("Jump"));
        }
        moveDirection.y += tmp;

        moveDirection.x = moveDirection.z = 0;
        controller.Move(moveDirection);

        //moveDirection = Vector3.zero;*/
    }

    void Update()
    {
        
    }
}
