using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KamehamehaScript : MonoBehaviour {

    public GameObject kamehameha;
    public float rotateSpeed;
    public Color color = Color.white; //Color of the light ray, is white by default

    void Update () {
        
        kamehameha.transform.Rotate(0, 0, rotateSpeed);
	}
}
