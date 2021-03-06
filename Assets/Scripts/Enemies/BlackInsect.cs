﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlackInsect : MonoBehaviour {

    //// Script for a basic enemy that walks between a number of set points in a cycle.
    [FMODUnity.EventRef]
    public string smallEnemyDamagedSound;

    float timeForAnotherComplaint = 0.0f;
    float timeToComplain;

    enum EnemyState { WALKING, HURT, WAITING };

    private CharacterController controller; // CharacterController that controls the enemy.
    public GameObject[] positions;          // Stores all the positions the enemy is gonna work towards to.

    public float speed = 5.0f;          // Speed at which it moves.
    public float rotationSpeed = 0.05f; // Speed at which he rotates over the 'y' axis when changing target position.
    public float gravity = 2.0f;        // Stores gravity applied.

    private bool alive;                 // Stores whether the enemy is alive or not.
    public bool isAMovingEnemy = true;
    public float tolerance = 0.5f;      // Tolerance for checking if a position is reached.

    private Vector3 directionVector;    // Vector of direction in which it is gonna move.
    private int activeNode;             // The current node it moves towards.

    public float damageDeal = 0.5f;     // Ammount of damage dealt on contact with player. The player looks for this damage.

    public int maxLife = 5;
    public float life;

    private EnemyState state;
    private float[] stateDeltas = {0,0,0};  // WALKING, HURT, WAITING
    private float[] stateSwitchTime = { 0, 0.1f, 0.5f };

    // FOR DEBUGGING:
    public GameObject DarkSphere;
    public float LightInteractionRadius = 8;

    private Transform defaultTransform;

    public EnemySpawner enemySpawner;

    Quaternion lerpLook;

            // Would be nice to add the functionality so that it can walk back and forwards, instead of in a cycle.

    // Use this for initialization
    void Start () {
        controller = GetComponent<CharacterController>(); //El primer que troba dins d'aquest objecte (com que només en té un no importa).

        alive = true;   // When starting we want it to be alive. We maybe could make it so could be set from Unity's editor, but it's not done.

        //GameObject temp = ClosestNode();    // We want the closest position.
        //directionVector = (temp.transform.position - transform.position)*speed; // We want the direction to be from where we are to the closest position. (at a speed)
        //controller.Move(directionVector);   // We move the enemy towards that first position.
        activeNode = getActiveNode();       // We set the activeNode to the one we are approaching.

        life = maxLife; // We set its initial life to be the maximum life it can have.

        defaultTransform = transform;
	}

    void Update()
    {
        if (life <= 0) { life = 0; alive = false; }   // If its life is below or equal 0, it's considered dead.
        LightInteraction();
    }
	
	// Update is called once per frame
	void FixedUpdate () {

        if (alive)  // If the enemy is alive:
        {
            if (isAMovingEnemy)
            {
                Vector3 tmpVec;
                int stateIndex = (int)state;
                switch (state)
                {
                    case EnemyState.WALKING:
                        // Move towards the (currentNode+1) (modular aritmethics) by using its position and at the set speed:
                        tmpVec = Vector3.zero;
                        tmpVec.x = positions[(activeNode + 1) % positions.Length].transform.position.x - transform.position.x;
                        tmpVec.z = positions[(activeNode + 1) % positions.Length].transform.position.z - transform.position.z;

                        tmpVec = tmpVec.normalized * speed;

                        directionVector.x = tmpVec.x; directionVector.z = tmpVec.z;
                        break;
                    case EnemyState.HURT:

                        timeForAnotherComplaint += Time.deltaTime;
                        Debug.Log(timeForAnotherComplaint + "/" + timeToComplain);
                        if (timeForAnotherComplaint >= timeToComplain)
                        {
                            timeForAnotherComplaint = 0.0f;
                            FMODUnity.RuntimeManager.PlayOneShot(smallEnemyDamagedSound);
                        }

                        Vector3 player = FindObjectOfType<Player>().transform.position;
                        tmpVec = Vector3.Normalize(player - transform.position) * (-1) * (speed + 5);
                        directionVector.x = tmpVec.x; directionVector.z = tmpVec.z;

                        stateDeltas[stateIndex] += Time.deltaTime;
                        if (stateDeltas[stateIndex] > stateSwitchTime[stateIndex]) { state = EnemyState.WAITING; stateDeltas[stateIndex] = 0; }
                        break;
                    case EnemyState.WAITING:
                        directionVector.x = 0; directionVector.z = 0;
                        stateDeltas[stateIndex] += Time.deltaTime;
                        if (stateDeltas[stateIndex] > stateSwitchTime[stateIndex]) { state = EnemyState.WALKING; stateDeltas[stateIndex] = 0; }
                        break;
                }

                //if (!controller.isGrounded) { directionVector.y -= gravity*2; }
                //else { directionVector.y = 0; }
                //print(controller.isGrounded);
                directionVector.y -= gravity * Time.deltaTime;

                // We want the enemy to look in the direction it's moving:
                if (directionVector != Vector3.zero) { lerpLook = Quaternion.LookRotation(new Vector3(directionVector.x, 0, directionVector.z)); }
                transform.rotation = Quaternion.Slerp(transform.rotation, lerpLook, rotationSpeed);

                // We move the enemy:
                controller.Move(directionVector * Time.deltaTime);

                // If the distance between (currentNode+1) and the enemy is smaller than the tolerance, then we have reached it. 
                Vector3 checkVec = positions[(activeNode + 1) % positions.Length].transform.position;
                if (Vector2.Distance(new Vector2(checkVec.x, checkVec.z), new Vector2(transform.position.x, transform.position.z)) < tolerance)
                {
                    activeNode = (activeNode + 1) % positions.Length;   // So we set the activeNode to the next one.

                    //transform.Rotate(0, Mathf.SmoothDampAngle(, 0);  // without smoothing.
                    //transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(directionVector), Time.deltaTime * speed);
                }
            }
        }
        else    // If the enemy is dead:
        {
            // This is for debugging only:

            //transform.parent.gameObject.SetActive(false); //Disactivate whole enemy
            if(transform.localScale != Vector3.zero)
            {
                GetComponent<DropsSystem>().Drop(transform.position); //Call drops for this enemy
                transform.localScale = new Vector3(0, 0, 0);
                if (enemySpawner != null) { enemySpawner.PleaseRespawn(this); }
                else { transform.parent.gameObject.SetActive(false); }//Disactivate whole enemy
                Player.instance.DestroyDamageInstance();
            }
                        

            //DarkSphere.SetActive(false);
            //GetComponent<SphereCollider>().enabled = false;
        }
    }

    // Function that returns the closest node around:
    GameObject ClosestNode() { //Troba el node mes proper
        float tempDistance = 0f;    // The distance.
        int whichPoint = 0;         // The index of the node.
        tempDistance = Vector3.Distance(transform.position, positions[0].transform.position);   // Initial distance for the first node.
        for (int i = 0; i < positions.Length; i++) {    // For all nodes in array.
            if (Vector3.Distance(transform.position, positions[i].transform.position) < tempDistance) { // If the distance is smaller than the previous one found, then...
                tempDistance = Vector3.Distance(transform.position, positions[i].transform.position);   // ... store the distance for the next check.
                whichPoint = i;                                                                         // and also store the index of the array with that distance.
            }
            
        }
        return positions[whichPoint];   // We will have the index of the nearest node. We return the corresponding GameObject.
    }

    // Function that returns the index of the current node if it is reached.
    int getActiveNode()
    {
        int tmp = 99;   // Just because we don't expect there to be 99 different positions in the array and we want to use this as control.
        for(int i = 0; i < positions.Length; i++)   // For all nodes in positions array.
        {
            if(Vector3.Distance(transform.position, positions[i].transform.position) < tolerance)   // If the distance is smaller than the tolerance...
            {
                tmp = i;    // ... the current node is i;
            }
        }
        if(tmp == 99) { /*print("No active Node");*/ }
        return tmp; // Return the found node.
    }

    void LightInteraction()
    {
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, LightInteractionRadius/2);
        if(hitColliders.Length == 0) { return; }
        for(int i = 0; i < hitColliders.Length; i++)
        {
            if (hitColliders[i].CompareTag("BlackInsect")) { continue; }
            if (hitColliders[i].CompareTag("LightOrb")) {
                if (hitColliders[i].GetComponent<LightOrb>().orbCharge > 0)
                {
                    hitColliders[i].GetComponent<LightOrb>().SubtractFromOrb(false);
                    life -= 0.01f;
                }
            }
        }
    }

    public void Hurt()
    {
        if(state == EnemyState.WALKING)
        {
            if (life > 0) { life -= 1; }
            state = EnemyState.HURT;
        }
        //knockback = true;
        //print("Enemy was hurt. Life is " + life);
    }

    public void Spawn(Vector3 where)
    {
        transform.position = where;
        life = maxLife;
        alive = true;
        //transform.parent.gameObject.SetActive(true);
        state = EnemyState.WALKING;
        transform.localScale = new Vector3(1, 1, 1);
    }

    public float getDamageDealt() { return damageDeal; } //Used by the player for on contact with insect dark area damage dealing

}
