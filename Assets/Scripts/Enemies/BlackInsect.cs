using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlackInsect : MonoBehaviour {

    private CharacterController controller;
    public GameObject[] positions;

    public float speed = 5.0f;
    public float rotationSpeed = 0.05f;
    public float gravity;

    private bool alive;
    public float tolerance = 0.5f;

    private Vector3 directionVector;
    private int activeNode;

    // Use this for initialization
    void Start () {
        controller = GetComponent<CharacterController>(); //El primer que troba dins d'aquest objecte

        alive = true;

        GameObject temp = closest();
        directionVector = (temp.transform.position - transform.position)*speed;
        controller.Move(directionVector);
        activeNode = getActiveNode();
        print("activeNode is: " + activeNode);
        print("Length is: " + (activeNode+1)%positions.Length);
	}
	
	// Update is called once per frame
	void FixedUpdate () {
        if (alive)
        {
            directionVector = Vector3.Normalize((positions[(activeNode + 1) % positions.Length].transform.position - transform.position)) * speed;

            Quaternion lerpLook = Quaternion.LookRotation(new Vector3(directionVector.x, 0, directionVector.z));
            transform.rotation = Quaternion.Slerp(transform.rotation, lerpLook, rotationSpeed);

            controller.Move(directionVector * Time.deltaTime);
            if(Vector3.Distance(positions[(activeNode +1)%positions.Length].transform.position, transform.position) < tolerance)
            {
                activeNode = (activeNode + 1) % positions.Length;
                //transform.Rotate(0, Mathf.SmoothDampAngle(, 0);  // without smoothing.
                //transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(directionVector), Time.deltaTime * speed);
            }
        }
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

    int getActiveNode()
    {
        int tmp = 99;
        for(int i = 0; i < positions.Length; i++)
        {
            if(Vector3.Distance(transform.position, positions[i].transform.position) < tolerance)
            {
                tmp = i;
            }
        }
        if(tmp == 99) { print("No active Node"); }
        return tmp;
    }
}
