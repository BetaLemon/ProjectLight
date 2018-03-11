using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightRay : MonoBehaviour {

    public GameObject LightRayGeometry;
    public float rotateSpeed;
    public Color color = Color.white; //Color of the light ray, is white by default

    void Update () {
        //renderer.material.SetFloat("_Blend", someFloatValue);
        LightRayGeometry.GetComponent<MeshRenderer>().materials[0].SetColor("_MKGlowColor", color);
        LightRayGeometry.transform.Rotate(0, 0, rotateSpeed);
	}
}
