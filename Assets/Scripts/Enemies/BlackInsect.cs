using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlackInsect : MonoBehaviour {

    private CharacterController controller;
    public GameObject[] positions;

    public float speed = 1.0f;
    public float gravity;

    private Vector3 directionVector;

    // Use this for initialization
    void Start () {
        controller = GetComponent<CharacterController>(); //El primer que troba dins d'aquest objecte

        GameObject temp = closest();
        directionVector = (temp.transform.position - transform.position)*speed;
        controller.Move(directionVector);

	}
	
	// Update is called once per frame
	void Update () {

    }

    GameObject closest() { //Troba el node mes proper
        float tempDistance = 0f;
        int whichPoint = 0;
        tempDistance = Vector3.Distance(transform.position, positions[0].transform.position);
        for (int i = 0; i < positions.Length; i++) {
            if (Vector3.Distance(transform.position, positions[i].transform.position) < tempDistance) {
                tempDistance = Vector3.Distance(transform.position, positions[i].transform.position);
                whichPoint = i;
            }
            
        }
        return positions[whichPoint];
    }
}
