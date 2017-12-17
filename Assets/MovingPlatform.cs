using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingPlatform : MonoBehaviour {

    public GameObject[] positions;
    public GameObject platform;

    private int currentNode;

    public float speed;

    // Use this for initialization
    void Start () {

    }
	
	// Update is called once per frame
	void Update () {
        platform.transform.position = Vector3.MoveTowards(platform.transform.position, positions[currentNode].transform.position, speed);
        if(Vector3.Equals(platform.transform.position, positions[currentNode].transform.position)) { currentNode = (currentNode + 1) % positions.Length; }
    }
}
