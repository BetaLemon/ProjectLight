using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mirror : MonoBehaviour {

    public GameObject Kamehameha;

    private bool reflecting;
    private Vector3 incomingVec, normalVec, hitPoint;

	// Use this for initialization
	void Start () {

	}

    public void Reflect(Vector3 inVec, Vector3 normal, Vector3 point)
    {
        incomingVec = inVec;
        normalVec = normal;
        hitPoint = point;
        reflecting = true;
    }
	
	// Update is called once per frame
	void FixedUpdate () {
        if (reflecting){
            Kamehameha.transform.localScale = new Vector3(16, 16, 16);
            Kamehameha.transform.position = hitPoint;
            Kamehameha.transform.forward = Vector3.Reflect(incomingVec, normalVec);
            reflecting = false;
        }
        else
        {
            Kamehameha.transform.localScale = new Vector3(0, 0, 0);
        }
        transform.Rotate(new Vector3(0,1,0));
	}
}
