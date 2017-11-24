using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuScript : MonoBehaviour {

    public Camera cam;
    public GameObject cameraTarget;
    public float panSpeed = 1.0f;
    public GameObject canvas;

    private bool isThere = false;
    private bool pleaseMove = false;

    float Lerp(float goal, float speed, float currentVal)
    {
        if (currentVal > goal)
        {
            if (currentVal - speed < goal) { return currentVal = goal; }
            return currentVal -= speed;
        }
        else if (currentVal < goal)
        {
            if (currentVal + speed > goal) { return currentVal = goal; }
            return currentVal += speed;
        }
        else return currentVal;
    }

    public void moveCameraToSaves()
    {
        pleaseMove = true;
    }

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        if (isThere == false && pleaseMove == true)
        {
            print("Pingas");
            Vector3 move;/*
        move.x = Lerp(cameraTarget.transform.position.x, panSpeed, cam.transform.position.x);
        move.y = Lerp(cameraTarget.transform.position.y, panSpeed, cam.transform.position.y);
        move.z = Lerp(cameraTarget.transform.position.z, panSpeed, cam.transform.position.z);*/
            move = Vector3.Lerp(cameraTarget.transform.position, cam.transform.position, panSpeed);

            cam.transform.position = move;

            isThere = (cameraTarget.transform.position == cam.transform.position);
            /* cam.transform.position = cameraTarget.transform.position;*/
        }
        if(isThere && pleaseMove) { canvas.SetActive(false); pleaseMove = false; }
    }
}
