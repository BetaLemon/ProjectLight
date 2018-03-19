using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {

    enum PlayerState { STANDING, WALKING, RUNNING};

    #region Variables
    // Variables:
    CharacterController controller;       // For controlling the player.
    PlayerState state;
    // Player movement variables:
    public float walkSpeed = 3.0f;        // Maximal speed when walking.
    public float runSpeed = 6.0f;         // Maximal speed when running.
    public float gravity = 1.0f;          // Gravity (unfortunately, it's linear for the moment being).
    public float minimumFallDamageDistance = 150;
    // Intern player variables:
    private float speed;                  // Speed applied to player.
    private Vector3 moveDirection;        // The direction the player is gonna move towards.
    private Vector3 forward;
    private float fallDistance;
    private float prevFallDistance;
    // Used for controlling the player:
    private PlayerInput input;
    private bool canMove = true;
    // Used for making the player move where the camera is looking at:
    private Camera camera;
    private Vector3 camForward;
    private Vector3 camRight;
    #endregion

    void Start()    // When the script starts.
    {
        controller = GetComponent<CharacterController>();   // We get the player's CharacterController.
        moveDirection = Vector3.zero;                       // We set the player's direction to (0,0,0).
        input = GetComponent<PlayerInput>();                // We get the player's input controller.
        camera = Camera.main;                               // We fetch the main camera.
    }

    void Update()
    {
        if (!canMove) return;   // If the player can't move then the following code shouldn't be executed.
        
        if (input.getInput("Horizontal") == 0 && input.getInput("Vertical") == 0) { state = PlayerState.STANDING; }
        else if ( input.getInput("Run") != 0 ) state = PlayerState.RUNNING; //Run mode if input for running
        //Check for a double input of: W, A, S, D, and if so, apply runModeActive true
        else if (input.wasDoubleClicked("doubleClickD")) { state = PlayerState.RUNNING; }
        else if (input.wasDoubleClicked("doubleClickA")) { state = PlayerState.RUNNING; }
        else if (input.wasDoubleClicked("doubleClickW")) { state = PlayerState.RUNNING; }
        else if (input.wasDoubleClicked("doubleClickS")) { state = PlayerState.RUNNING; }

        if (!(state == PlayerState.RUNNING)) //The player should walk, run mode is inactive
        {
            speed += ;
        }
        else //Run mode is active, apply run speed
        {
            movementMultiplier = runSpeed;
        }

       //Basic movement system:
       moveDirection.x = input.getInput("Horizontal") * movementMultiplier;  // The player's x movement is the Horizontal Input (0-1) * speed.
       moveDirection.z = input.getInput("Vertical") * movementMultiplier;    // The player's y movement is the Vertical Input (0-1) * speed.
    }

    void FixedUpdate()  // What the script executes at a fixed framerate. Good for physics calculations. Avoids stuttering.
    {
        if (!canMove) { return; }
        if (!controller.isGrounded) // If the player is not grounded / is in the air.
        {
            moveDirection.y = -1*gravity; // We apply gravity.
            fallDistance += Mathf.Abs(moveDirection.y);
        }
        else    // Else, the player is touching the ground.
        {
            //moveDirection.y = 0;    // His vertical (y) movement is reset.
            prevJumpTime = 0;       // His time in the air is 0.
            fallDistance = 0;
        }

        if(fallDistance == 0 && prevFallDistance != 0)
        {
            //print(prevFallDistance);
            if(prevFallDistance > minimumFallDamageDistance)
            {
                GetComponent<Player>().fallDamage(prevFallDistance);
            }
        }

        prevFallDistance = fallDistance;

        //print(controller.isGrounded);// print(moveDirection.y);

        if (prevJumpTime < maxJumpTime)  // If the player has been in the air less than Max, and...
        {
            if (input.isPressed("Jump"))        // ... the Jump button is pressed...
                moveDirection.y += jumpSpeed;   // ... add JumpSpeed to the vertical movement (y).
        }

        prevJumpTime += Time.deltaTime;     // We add the deltaTime to the time in the air (if he is on the ground, it will be reset to 0 in the next execution).

        camForward = camera.transform.forward;
        camRight = camera.transform.right;
        camForward.y = 0;
        camRight.y = 0;
        camForward = camForward.normalized;
        camRight = camRight.normalized;

        Vector3 move = moveDirection.x * camRight + moveDirection.z * camForward;
        move.y = moveDirection.y;

        controller.Move(move * Time.deltaTime);    // We tell the CharacterController to move the player in the direction, by the Delta for smoothness.
        /*
        //Please remove this piece of shit after the alpha:
        playerRig.GetComponent<Animation>().enabled = false;
        if (moveDirection.x == 0) { moveDirection.x = transform.forward.x;  } //Last bit is Alpha bullshit
        else { playerRig.GetComponent<Animation>().enabled = true; }
        if (moveDirection.z == 0) { moveDirection.z = transform.forward.z;}
        else { playerRig.GetComponent<Animation>().enabled = true; }*/
        //moveDirection.y = transform.forward.y;
        /*
       // if (moveDirection.x != 0 || moveDirection.z != 0)   // If the player is moving...
       // {
            lerpLook = Quaternion.LookRotation(new Vector3(moveDirection.x, 0, moveDirection.z));   // ... the direction is gonna be in the direction of movement.
                                                                                                    //  }
        //transform.forward = Vector3.RotateTowards(transform.forward, moveDirection, speed, speed);

        transform.rotation = Quaternion.Slerp (transform.rotation, lerpLook, Time.deltaTime*4);   // We rotate the player towards lerpLook, applying a lerp.
        */
        if(moveDirection.x != 0 || moveDirection.z != 0) { forward = moveDirection; forward.y = 0; }
        controller.gameObject.transform.forward = forward;
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

    public void AllowMovement() { canMove = true; }

    public void Move(Vector3 direction) {
        controller.Move(direction * Time.deltaTime);
    }

    public void StopMovement() { canMove = false; }
}
