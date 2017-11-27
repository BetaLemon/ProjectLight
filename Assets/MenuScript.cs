using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuScript : MonoBehaviour {

    public Camera cam;
    public GameObject cameraTarget;
    public float panSpeed = 0.08f;
    public GameObject canvas;

    private bool pleaseMove;

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

    public void activateCameraMove()
    {
        pleaseMove = true;
    }

	// Use this for initialization
	void Start () {
        pleaseMove = false;
	}
	
	// Update is called once per frame
	void Update () {
        if (pleaseMove)
        {
            Vector3 move;
            while(cam.transform.position != cameraTarget.transform.position)
            {
                move.x = Lerp(cameraTarget.transform.position.x, panSpeed, cam.transform.position.x);
                move.y = Lerp(cameraTarget.transform.position.y, panSpeed, cam.transform.position.y);
                move.z = Lerp(cameraTarget.transform.position.z, panSpeed, cam.transform.position.z);
                cam.transform.position = move;
            }
            canvas.SetActive(false);
            pleaseMove = false;
        }
    }
}
