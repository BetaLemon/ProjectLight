﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerLight : MonoBehaviour {

    public enum LightMode { NEAR, FAR };

    public Light lightOrb;
    public GameObject lightCylinder;
    //EL SPOTLIGHT L'HE RETIRAT D'AQUEST CODI, CREC QUE S'AURÍA DE SUSTITUIR EL CILINDRE PER UN CILINDRE DE LLUM EN CONDICIONS, QUE DE VERITAT ILUMINI CORRECTAMENT. -Dylan

    //Light Orb variables
    private float defaultLightOrbRange = 3.0f; //Orb light base extension radius to which the update tends
    public float lerpSpeed = 0.2f;
    public float farStaffRange = 2.5f;
    public float maxExpandingLight;
    public float expandingLightSpeed;

    //Light Cylinder variables
    private float defaultLightCylinderScale;

    private LightMode lightMode; //Near or Far light modes
    private float prevLightAxis = 0;

    public float power = 100.0f;

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
        defaultLightCylinderScale = lightCylinder.transform.localScale.z;
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
        
        switch (lightMode)
        {
            case LightMode.NEAR:

                lightOrb.range = Lerp(defaultLightOrbRange, lerpSpeed, lightOrb.range); //Light Orb radius to it's current range at LerpSpeed
                lightCylinder.transform.localScale = new Vector3(16, 16, Lerp(defaultLightCylinderScale, 2f, lightCylinder.transform.localScale.z)); //Light cylinder back to 0 length
                if (lightCylinder.transform.localScale.z == 0) { lightCylinder.SetActive(false); } //Cilinder activity off since we are on near mode

                if (Input.GetAxis("LightMax") != 0) //If expand light orb input is detected
                {
                    lightOrb.range += expandingLightSpeed; //Expand the light on input at expansion speed
                    if (lightOrb.range > maxExpandingLight) { lightOrb.range = maxExpandingLight; } //Light orb expansion limit
                }

                break;
            case LightMode.FAR:
                if (!(GetComponent<PlayerInteraction>().isHittingMirror())) //Afegeixo mes bloquejadors aqui... pero EL TEU SISTEMA TE BUGS ALEX, BUGS MOLT LLETJOS XD
                {
                    lightCylinder.SetActive(true);

                    lightOrb.range = Lerp(farStaffRange, lerpSpeed, lightOrb.range);
                                                                                            
                    lightCylinder.transform.localScale = new Vector3(16, 16, Lerp(16, 2f, lightCylinder.transform.localScale.z));
                }
                else
                {
                    lightCylinder.transform.localScale = new Vector3(16, 16, Vector3.Distance(GetComponent<PlayerInteraction>().getRayHit().point, lightCylinder.transform.position) / 2);
                }
            break;
        default:
                print("Error: wrong light mode.");
                break;
        }
        
	}

    public LightMode getLightMode() { return lightMode; }
}
