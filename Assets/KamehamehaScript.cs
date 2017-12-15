using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KamehamehaScript : MonoBehaviour {

    public GameObject kamehameha;
    public float rotateSpeed;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        kamehameha.transform.Rotate(0, 0, rotateSpeed);
	}
}
