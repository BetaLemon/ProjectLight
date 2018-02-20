using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {

    // Variables:
    CharacterController controller; // For controlling the player.
    public float speed = 3.0f;      // Speed at which he moves.
    public float jumpSpeed = 10f;   // Speed at which he jumps.
    public float gravity = 1.0f;    // Gravity acceleration.
    private Vector3 moveDirection;  // The direction the player is gonna move towards.
    private Quaternion lerpLook;    // The direction at which the player is gonna look.

    private float prevJumpTime;     // For controlling the time the player spends in the air.
    public float maxJumpTime;       // The maximum time the player will be able to be in the air.

    private PlayerInput input;

    //Please remove this piece of shit after the alpha:
    public GameObject playerRig; //Alpha bullshit

    void Start()    // When the script starts.
    {
        controller = GetComponent<CharacterController>();   // We get the player's CharacterController.
        moveDirection = Vector3.zero;                       // We set the player's direction to (0,0,0).
        input = GetComponent<PlayerInput>();
    }

    void Update()
    {
        moveDirection.x = input.getInput("Horizontal") * speed;  // The player's x movement is the Horizontal Input (0-1) * speed.
        moveDirection.z = input.getInput("Vertical") * speed;    // The player's y movement is the Vertical Input (0-1) * speed.
    }

    void FixedUpdate()  // What the script executes at a fixed framerate. Good for physics calculations. Avoids stuttering.
    {
   
        if (!controller.isGrounded) // If the player is not grounded / is in the air.
        {
            moveDirection.y -= gravity; // We apply gravity.
        }
        else    // Else, the player is touching the ground.
        {
            moveDirection.y = 0;    // His vertical (y) movement is reset.
            prevJumpTime = 0;       // His time in the air is 0.
        }
        if(prevJumpTime < maxJumpTime)  // If the player has been in the air less than Max, and...
        {
            if (input.isPressed("Jump"))        // ... the Jump button is pressed...
                moveDirection.y += jumpSpeed;   // ... add JumpSpeed to the vertical movement (y).
        }

        prevJumpTime += Time.deltaTime;     // We add the deltaTime to the time in the air (if he is on the ground, it will be reset to 0 in the next execution).

        controller.Move(moveDirection * Time.deltaTime);    // We tell the CharacterController to move the player in the direction, by the Delta for smoothness.

        //Please remove this piece of shit after the alpha:
        playerRig.GetComponent<Animation>().enabled = false;
        if (moveDirection.x == 0) { moveDirection.x = transform.forward.x;  } //Last bit is Alpha bullshit
        else { playerRig.GetComponent<Animation>().enabled = true; }
        if (moveDirection.z == 0) { moveDirection.z = transform.forward.z;}
        else { playerRig.GetComponent<Animation>().enabled = true; }
        //moveDirection.y = transform.forward.y;

       // if (moveDirection.x != 0 || moveDirection.z != 0)   // If the player is moving...
       // {
            lerpLook = Quaternion.LookRotation(new Vector3(moveDirection.x, 0, moveDirection.z));   // ... the direction is gonna be in the direction of movement.
                                                                                                    //  }
        //transform.forward = Vector3.RotateTowards(transform.forward, moveDirection, speed, speed);

        transform.rotation = Quaternion.Slerp (transform.rotation, lerpLook, Time.deltaTime*4);   // We rotate the player towards lerpLook, applying a lerp.

        // I don't know what this old junk is:

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

        // End of junk.
    }

    void OnTriggerStay(Collider other)  // If entering a Trigger Collider.
    {
        if(other.gameObject.tag == "MovingPlatform")    // If the trigger belongs to the MovingPlatform, make that the player's parent.
        {
            transform.parent = other.transform;
        }
    }

    void OnTriggerExit(Collider other)  // If leaving a Trigger Collider.
    {
        if(other.gameObject.tag ==   "MovingPlatform")  // If he's leaving the MovingPlatform, make the player it's own.
        {
            transform.parent = null;
        }
    }
}
