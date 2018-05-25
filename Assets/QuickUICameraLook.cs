using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class QuickUICameraLook : MonoBehaviour {

    private Camera cam;
    private Collider col;
    public GameObject[] uiElement;
    public float minDist = 8;

    // Use this for initialization
    void Start () {
        cam = Camera.main;
        if(uiElement.Length == 0)
        {
            uiElement = new GameObject[1];
            uiElement[0] = transform.GetChild(0).gameObject;
        }
	}
	
	// Update is called once per frame
	void Update () {
        cam = Camera.main;
        foreach(GameObject element in uiElement)
        {
            element.transform.forward = cam.transform.forward;
            CameraDistanceAlpha(element, cam);
        }
	}

    void CameraDistanceAlpha(GameObject el, Camera cam)
    {
        Image img = el.GetComponent<Image>();
        if (img != null)
        {
            Color tmp; tmp.r = 1; tmp.g = 1; tmp.b = 1; tmp.a = Mathf.Min(1, Vector3.Distance(transform.position, cam.transform.position) - minDist);
            img.color = tmp;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player")) { SetActiveElements(true); }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player")) { SetActiveElements(false); }
    }

    void SetActiveElements(bool state)
    {
        foreach(GameObject element in uiElement)
        {
            element.SetActive(state);
        }
    }
}
