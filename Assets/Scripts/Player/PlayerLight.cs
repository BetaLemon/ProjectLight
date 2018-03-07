using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerLight : MonoBehaviour {

    public enum LightMode { NEAR, FAR, MAX };

    public Light lightSphere;
    public GameObject lightCylinder;

    private LightMode lightMode; //Near or Far light modes
    private float prevLightAxis = 0;

    //Light Orb variables
    public float defaultLightSphereRange = 2.5f; //Orb light base extension radius to which the update tends
    public float lerpSpeed = 0.2f;
    public float maxExpandingLight;
    public float expandingLightSpeed;

    //Light Cylinder variables
    public float lightSphereRangeInFarMode = 1.5f; //Orb light radius in far mode
    public float maxLightCylinderScale = 5f; //maximum local Z scale for the extended cylinder
    private float defaultLightCylinderScale;

    //Self health drainage system due to light expansion
    public float healthDrainAmmount = 0.05f; //Health points from Player.cs substracted from mana consumption strain on light production
    private int drainDelay = 10000; //Time between health drain losses

    private PlayerInput input;

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
        input = GetComponent<PlayerInput>();
	}
	
	// Update is called once per frame
	void Update () {

        if(input.getInput("LightSwitch") != 0 && prevLightAxis == 0)
        {
            if(lightMode == LightMode.NEAR) { lightMode = LightMode.FAR; }
            else if(lightMode == LightMode.MAX) { lightMode = LightMode.FAR; }
            else if(lightMode == LightMode.FAR) { lightMode = LightMode.NEAR; }
            //print(lightMode);
        }

        prevLightAxis = input.getInput("LightSwitch");

        /* To be done:
         * - Millorar els noms de les variables.
         * 
         */

        if (input.isPressed("LightMax"))
        {
            if (lightMode == LightMode.NEAR || lightMode == LightMode.MAX) { lightMode = LightMode.MAX; }
        }
        else if (lightMode != LightMode.FAR) { lightMode = LightMode.NEAR; }

        switch (lightMode)
        {
            case LightMode.NEAR:
                lightSphere.range = Lerp(defaultLightSphereRange, lerpSpeed, lightSphere.range); //Light Orb radius to it's default range at LerpSpeed
                lightCylinder.transform.localScale = new Vector3(8, 8, Lerp(defaultLightCylinderScale, 2f, lightCylinder.transform.localScale.z)); //Light cylinder back to 0 length
                if (lightCylinder.transform.localScale.z == 0) { lightCylinder.SetActive(false); } //Cilinder activity off since we are on near mode
                break;
            case LightMode.MAX:
                GetComponent<Player>().health -= healthDrainAmmount; //Decrease player health for doing this action
                lightSphere.range += expandingLightSpeed; //Expand the light on input at expansion speed
                if (lightSphere.range > maxExpandingLight) { lightSphere.range = maxExpandingLight; } //Light orb expansion limit
                break;
            case LightMode.FAR:

                GetComponent<Player>().health -= healthDrainAmmount; //Decrease player health for being in this mode

                lightCylinder.SetActive(true);

                lightSphere.range = Lerp(lightSphereRangeInFarMode, lerpSpeed, lightSphere.range);

                RaycastHit tmpHit = GetComponent<PlayerInteraction>().getRayHit();
                if(tmpHit.collider != null) // If something was hit:
                {
                    //Check at what distance the intersection happened:
                    float distCylPosHitPos = Vector3.Distance(GetComponent<PlayerInteraction>().getRayHit().point, lightCylinder.transform.position);
                    if (distCylPosHitPos / 2 > maxLightCylinderScale)
                    {
                        lightCylinder.transform.localScale = new Vector3(8, 8, maxLightCylinderScale);
                    }
                    else
                    {
                        lightCylinder.transform.localScale = new Vector3(8, 8, distCylPosHitPos / 2);
                    }
                    
                }
                else    // Else, if nothing was hit:
                {
                    lightCylinder.transform.localScale = new Vector3(8, 8, Lerp(maxLightCylinderScale, 2f, lightCylinder.transform.localScale.z));
                }

                break;
        default:
                print("Error: wrong light mode.");
                break;
        }
        
	}

    public LightMode getLightMode() { return lightMode; }
}

