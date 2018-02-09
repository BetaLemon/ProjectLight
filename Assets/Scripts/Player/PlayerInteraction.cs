﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInteraction : MonoBehaviour {

    public Light StaffLight;
    public GameObject Kamehameha;

    private RaycastHit rayHit;

    void Start () {
		
	}
	
	void FixedUpdate () {

        /// PASSIVE INTERACTION (Sphere Light)
        Collider[] hitColliders = Physics.OverlapSphere(StaffLight.transform.position, GetComponent<PlayerLight>().lightSphere.range); //(Sphere center, Radius)
        int tmp = 0;
        for (int i = 0; i < hitColliders.Length; i++)
        {
            if (hitColliders[i].isTrigger)
            {
                if (hitColliders[i].gameObject.CompareTag("PlayerLight")) { continue; }
                switch (hitColliders[i].gameObject.tag)
                {
                    case "LightOrb":
                        LightOrb(hitColliders[i], false);
                        break;
                    default:break;
                }
                tmp++;
            }
        }

        /// ACTIVE INTERACTION (Cylinder Light)
        //print("Hit " + tmp + " triggers.");
        if(GetComponent<PlayerLight>().getLightMode() == PlayerLight.LightMode.FAR) // If the player uses the Cylinder Light.
        {
            if (Physics.Raycast(StaffLight.transform.position, transform.forward, out rayHit))  //(vec3 Origin, vec3direction, vec3 output on intersection) If Raycast hits a collider.
            {   //debug
                Vector3 reflectVec = Vector3.Reflect(rayHit.point - StaffLight.transform.position, rayHit.normal);  // For drawing the reflected line. (only debugging)
                //print(reflectVec.x + " " + reflectVec.y + " " + reflectVec.z);
                Debug.DrawRay(rayHit.point, reflectVec * 1000, Color.green); //Drawing the reflection ray as debug
                //end debug

                // Specific game object interactions with light cylinder:
                if (rayHit.collider.gameObject.CompareTag("Mirror")) { Mirror(rayHit);} //Reflect mirror light
                if (rayHit.collider.gameObject.CompareTag("LightOrb")) { LightOrb(rayHit.collider, true); } //Interact with the light orb
                if (rayHit.collider.gameObject.CompareTag("Trigger")) { TriggerTrigger(rayHit); }
            }
        }
    }

    //Light orb interacter:
    void LightOrb(Collider col, bool isInCylinderMode) 
    {
        if(!isInCylinderMode) col.GetComponent<LightOrb>().Interact(GetComponent<PlayerLight>().healthDrainAmmount, isInCylinderMode);
        else col.GetComponentInParent<LightOrb>().Interact(GetComponent<PlayerLight>().healthDrainAmmount, isInCylinderMode);

    }

    void Mirror(RaycastHit mirrorHit)
    {
        Vector3 inVec = mirrorHit.point - StaffLight.transform.position;
        mirrorHit.collider.GetComponentInParent<Mirror>().Reflect(inVec, mirrorHit.normal, mirrorHit.point);
        Kamehameha.transform.localScale = new Vector3(16, 16, Vector3.Distance(mirrorHit.point, Kamehameha.transform.position)/2);
    }

    void TriggerTrigger(RaycastHit rh)
    {
        rh.collider.gameObject.GetComponentInParent<Trigger>().pleaseTrigger();
    }

    public RaycastHit getRayHit()
    {
        return rayHit;
    }
}
