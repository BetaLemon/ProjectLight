using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInteraction : MonoBehaviour {

    public Light StaffLight;
    public GameObject Kamehameha;

    private RaycastHit rayHit;
    private bool hasHitMirror;
    private bool hasHitPlatform;

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
                        LightOrb(hitColliders[i]);
                        print("Collision detected with light ORB");
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

                // If the tag of the gameObject it collided with is "Mirror" then execute Mirror for interaction, and set hasHitMirror to true:
                if (rayHit.collider.gameObject.CompareTag("Mirror")) { Mirror(rayHit); hasHitMirror = true; } //Block light going through mirror, and reflect it
                else if (rayHit.collider.gameObject.CompareTag("MovingPlatform")) { hasHitPlatform = true; } //Block light going through platform
                else if (rayHit.collider.gameObject.CompareTag("LightOrb")) { LightOrb(rayHit.collider); print("Collision detected with light RAY"); } //Interact with the light orb
                else { hasHitMirror = false; hasHitPlatform = false; }  // If it's not a mirror or a platform, all check booleans false.

                if (rayHit.collider.gameObject.CompareTag("Trigger")) { TriggerTrigger(rayHit); }

                //if(!((rayHit.collider.gameObject.CompareTag("Trigger")) || (rayHit.collider.gameObject.CompareTag("Mirror")))){ hitDumbObject(rayHit); }
            }
            else
            {
                hasHitMirror = false;   // If it didn't collide with anything, no mirror was hit.
            }
        }
        else { hasHitMirror = false; }  // If the player isn't using the Cylinder Light, then no mirrors can be hit by it.
    }

    //Light orb interacter:
    void LightOrb(Collider col) 
    {
       print("Entered light orb interaction");

            col.GetComponent<LightOrb>().Interact(GetComponent<PlayerLight>().healthDrainAmmount);
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

    public bool isHittingMirror()
    {
        return hasHitMirror;
    }
    public bool isHittingPlatform()
    {
        return hasHitPlatform;
    }

    public RaycastHit getRayHit()
    {
        return rayHit;
    }
}
