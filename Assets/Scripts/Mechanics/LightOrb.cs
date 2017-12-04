using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightOrb : MonoBehaviour {

    public Light glow;

    private float orbIntensity = 10;
    public float orbCharge;
    private float maxOrbCharge = 3.5f;
    private float minOrbCharge = 0;

    // Use this for initialization
    void Start () {

	}
	
	// Update is called once per frame
	void Update () {
        //print(GetComponent<Light>().intensity);
        glow.intensity = orbIntensity;
    }
}
