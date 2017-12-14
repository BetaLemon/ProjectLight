﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerLight : MonoBehaviour {

    enum LightMode { NEAR, FAR };

    public Light staffLight;
    //public Light spotLight;

    public GameObject kamehameha;

    private float defaultStaffLightRange = 4;
    private float maxStaffLightRange = 100;
    private float minStaffLightRange = 10;

    //public float defaultSpotLightIntensity;
    private float defaultKameScale;

    private LightMode lightMode;
    private float prevLightAxis = 0;


    float Lerp(float goal, float speed, float currentVal)
    {
        if (currentVal > goal)
        {
            if(currentVal - speed < goal) { return currentVal = goal; }
            return currentVal -= speed;
        }
        else if (currentVal < goal)
        {
            if (currentVal + speed > goal) { return currentVal = goal; }
            return currentVal += speed;
        }
        else return currentVal;
    }

	// Use this for initialization
	void Start () {
        lightMode = LightMode.NEAR;
        //defaultSpotLightIntensity = spotLight.intensity;
        defaultKameScale = kamehameha.transform.localScale.z;
	}
	
	// Update is called once per frame
	void Update () {

        if(Input.GetAxis("LightSwitch") != 0 && prevLightAxis == 0)
        {
            if(lightMode == LightMode.NEAR) { lightMode = LightMode.FAR; }
            else if(lightMode == LightMode.FAR) { lightMode = LightMode.NEAR; }
            //print(lightMode);
        }

        prevLightAxis = Input.GetAxis("LightSwitch");
        
        if(Input.GetAxis("LightMax") != 0)
        {
            staffLight.range += 2;
        }

        switch (lightMode)
        {
            case LightMode.NEAR:
                /*
                pointLight.range = defaultPointLightRange;
                spotLight.intensity = 0.0f;*/
                staffLight.range = Lerp(defaultStaffLightRange, 1.5f, staffLight.range);
                //spotLight.intensity = Lerp(0.0f, 5.0f, spotLight.intensity);
                kamehameha.transform.localScale = new Vector3(16, 16, Lerp(defaultKameScale, 2f, kamehameha.transform.localScale.z));
                break;
            case LightMode.FAR:
                /*
                pointLight.range = 8.0f;
                spotLight.intensity = defaultSpotLightIntensity;*/
                staffLight.range = Lerp(3.0f, 5f, staffLight.range);
                //spotLight.intensity = Lerp(defaultSpotLightIntensity, 2f, spotLight.intensity);
                kamehameha.transform.localScale = new Vector3(16,16,Lerp(16, 2f,kamehameha.transform.localScale.z));
                break;
            default:
                //print("Error: wrong light mode.");
                break;
        }
	}
}
