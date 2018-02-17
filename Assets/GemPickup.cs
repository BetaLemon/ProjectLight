using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GemPickup : MonoBehaviour {

    private Vector3 _startPosition; //Serves as a reference for base sinoidal motion animation
    public GameObject player; //PlayerReference
    public float getDraggedRadius = 5; //Radius from which the collectable will start getting absorbed
    public float dragSpeedIncrementer = 10; //Speed at which the collectable is dragged towards the player on proximity
    public Rigidbody rb;

    void Start () {
        _startPosition = transform.position;
        player = GameObject.FindGameObjectWithTag("Player"); //THIS SYSTEM OF OBTAINING PLAYER COORDINATES MAY BE IMPROVED IN EFFICIENCY. BUT PLAYER POSITION IS NEEDED
        rb = GetComponent<Rigidbody>();
    }
	
	void FixedUpdate() {
        transform.position = _startPosition + new Vector3(0.0f, Mathf.Sin(Time.time*4)/4, 0.0f); //Sinoidal motion for position (Up and down)
        //Check if collectable tag is ManaCharge or SmallGemstone
        if (tag == "ManaCharge" || tag == "SmallGemstone") {
            //Check for player proximity
            Vector3 distanceFromPlayer = player.transform.position - transform.position;
            float distanceModule = distanceFromPlayer.magnitude;
            //print(distanceModule);
                //Check if player proximity is closer than get dragged radius
                if (Mathf.Abs(distanceModule) < getDraggedRadius) {
                    //Travel towards player at speed according to proximity (The closer the faster):
                    float dragSpeed = (getDraggedRadius / distanceModule) * dragSpeedIncrementer;
                    //Calculate velocity to apply to collectable:
                    Vector3 appliedVelocity = distanceFromPlayer.normalized * dragSpeed;
                    //Apply the velocity to the object:
                    rb.AddForce(appliedVelocity, ForceMode.Force);
                }
        }
    }

    void targetSeek(Transform target, float distanceToStop, float speed)
    {
        var direction = Vector3.zero;
        if (Vector3.Distance(transform.position, target.position) > distanceToStop)
        {
            direction = target.position - transform.position;
            
        }
    }
}
