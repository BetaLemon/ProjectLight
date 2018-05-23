using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuickUICameraLook : MonoBehaviour {

    private Camera cam;
    private Collider col;
    private GameObject uiElement;

	// Use this for initialization
	void Start () {
        cam = Camera.main;
        uiElement = transform.GetChild(0).gameObject;
	}
	
	// Update is called once per frame
	void Update () {
        cam = Camera.main;
        transform.forward = cam.transform.forward;
	}

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player")) { uiElement.SetActive(true); }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player")) { uiElement.SetActive(false); }
    }
}
