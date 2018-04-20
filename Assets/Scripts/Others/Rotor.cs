using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rotor : MonoBehaviour {

    public GameObject obj;
    public GameObject pivot;
    public bool x, y, z;
    public float angle;
    public bool startRotating = true;
    private bool isRotating;

	// Use this for initialization
	void Start () {
        isRotating = startRotating;
        if(pivot == null) { pivot = obj; }
	}
	
	// Update is called once per frame
	void FixedUpdate () {
        if (isRotating)
        {
            Vector3 rotateAxis = Vector3.zero;
            if (x) { rotateAxis.x = 1; }
            if (y) { rotateAxis.y = 1; }
            if (z) { rotateAxis.z = 1; }
            obj.transform.RotateAround(pivot.transform.position, rotateAxis, angle);
        }
	}

    public void EnableRotor() { isRotating = true; }
    public void DisableRotor() { isRotating = false; }
}
