using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingPlatform : MonoBehaviour {

    // The MovingPlatform script controlls a model that behaves like a moving platform. It moves between
    // the different nodes that are set in Unity's editor.

    public GameObject[] positions;  // Array with all the different positions it moves to. Set in Editor.
    public GameObject platform;     // Model that will be moved. Needs to have a Trigger Collider to work properly.

    private int currentNode;        // The current node the platform is moving towards.
    public bool isRunning;          // Controls if the platform is moving or not.

    public float speed;             // Speed at which the platform moves.

    /* An option could be added that allows the platform to got forwards and backwards, instead
     * of traversing the nodes in a cicle.
     */

    // Use this for initialization
    void Start () {

    }
	
	// Update is called once per frame
	void FixedUpdate () {
        if (isRunning)  // If the platforming is running:
        {
            // Change the position of the platform to the one of the current node, at speed:
            platform.transform.position = Vector3.MoveTowards(platform.transform.position, positions[currentNode].transform.position, speed);
            // If the platform has reached currentNode, then go to the next node (modular arithmetics):
            if (Vector3.Equals(platform.transform.position, positions[currentNode].transform.position)) { currentNode = (currentNode + 1) % positions.Length; }
        }
    }

    // If a Trigger Object is set to activate this moving platform, the following function will be called:
    public void getTriggered() { isRunning = true; }    // If it is activated then set running to true.
}
