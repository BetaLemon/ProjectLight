using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GemPickup : MonoBehaviour
{

    private Vector3 _startPosition; //Serves as a reference for base sinoidal motion animation
    private GameObject player; //PlayerReference
    public float getDraggedRadius = 5.0f; //Radius from which the collectable will start getting absorbed
    public float dragSpeed = 0.2f; //Speed at which the collectable is dragged towards the player on proximity
    public Rigidbody rb;

    void Start()
    {
        _startPosition = transform.position;
        player = GameObject.FindWithTag("Player"); //THIS SYSTEM OF OBTAINING PLAYER COORDINATES MAY BE IMPROVED IN EFFICIENCY. BUT PLAYER POSITION IS NEEDED
        rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        //Check if collectable tag is ManaCharge or SmallGemstone
        if (tag == "ManaCharge" || tag == "SmallGemstone")
        {
            float distance = Vector3.Distance(transform.position, Player.instance.transform.position);
            if (distance < getDraggedRadius)
            {
                // The thing moves towards the player:
                transform.position = Vector3.MoveTowards(transform.position, Player.instance.transform.position, dragSpeed / distance);
            }
        }
        transform.position = new Vector3(transform.position.x, _startPosition.y + Mathf.Sin(Time.time * 4) / 4, transform.position.z); //Sinoidal motion for position (Up and down)

    }
}