using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerLight : MonoBehaviour {

    public enum LightMode { NEAR, FAR };

    public Light lightOrb;
    public GameObject lightCylinder;

    private LightMode lightMode; //Near or Far light modes
    private float prevLightAxis = 0;

    //Light Orb variables
    private float defaultLightOrbRange = 3.0f; //Orb light base extension radius to which the update tends
    public float lerpSpeed = 0.2f;
    public float farStaffRange = 2.5f;
    public float maxExpandingLight;
    public float expandingLightSpeed;

    //Light Cylinder variables
    private float defaultLightCylinderScale;

    //Self health drainage system due to light expansion
    public float healthDrainLossAmmount = 0.05f; //Health points from Player.cs substracted from mana consumption strain on light production
    private int drainDelay = 10000; //Time between health drain losses

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
        
        /* To be done:
         * - Separar en dos modes el NEAR, per a que no es resti una cosa amb l'altra (LightMax). !!!
         * - Canviar els noms de les variables.
         * 
         */

        switch (lightMode)
        {
            case LightMode.NEAR:

                lightOrb.range = Lerp(defaultLightOrbRange, lerpSpeed, lightOrb.range); //Light Orb radius to it's default range at LerpSpeed
                lightCylinder.transform.localScale = new Vector3(16, 16, Lerp(defaultLightCylinderScale, 2f, lightCylinder.transform.localScale.z)); //Light cylinder back to 0 length
                if (lightCylinder.transform.localScale.z == 0) { lightCylinder.SetActive(false); } //Cilinder activity off since we are on near mode

                if (Input.GetAxis("LightMax") != 0) //If expand light sphere input is detected
                {
                    GetComponent<Player>().health -= healthDrainLossAmmount; //Decrease player health for doing this action
                    lightOrb.range += expandingLightSpeed; //Expand the light on input at expansion speed
                    if (lightOrb.range > maxExpandingLight) { lightOrb.range = maxExpandingLight; } //Light orb expansion limit
                }

                break;
            case LightMode.FAR:
                //NASTY BUGS IN THIS SECTION
                if (!(GetComponent<PlayerInteraction>().isHittingMirror()) && !(GetComponent<PlayerInteraction>().isHittingPlatform())) // Needs to be enhanced.
                {
                    GetComponent<Player>().health -= healthDrainLossAmmount; //Decrease player health for being in this mode

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
