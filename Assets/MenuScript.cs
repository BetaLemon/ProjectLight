using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuScript : MonoBehaviour {

    public Camera cam;
    public GameObject cameraTarget;
    public float panSpeed = 1.0f;
    public GameObject canvas;

    private float startTime;
    private float journeyLength;

    bool compareVec(Vector3 a, Vector3 b)
    {
        return (a.x == b.x && a.y == b.y && a.z == b.z);
    }

    public void moveCameraToSaves()
    {
        startTime = Time.time;
        journeyLength = Vector3.Distance(transform.position, cameraTarget.transform.position);
        Vector3 move;
        while(!compareVec(cam.transform.position, cameraTarget.transform.position))
        {
            float distCovered = (Time.time - startTime) * panSpeed;
            float fracJourney = distCovered / journeyLength;
            move = Vector3.Lerp(cam.transform.position, cameraTarget.transform.position, fracJourney);
            cam.transform.position = move;
        }
        if (compareVec(cam.transform.position, cameraTarget.transform.position)) { canvas.SetActive(false); }
    }

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {

    }
}
