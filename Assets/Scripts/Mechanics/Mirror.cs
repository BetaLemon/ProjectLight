using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mirror : MonoBehaviour {

    // Mirror is a script that enables light reflection for mirrors and other objects. To differentiate the
    // light source from others, it is internally called Kamehameha. It owns such a Kamehameha and points it
    // in the reflected angles direction. Mirrors can reflect with each other.

    public GameObject Kamehameha;   // Stores the cylinder that represents the player's light ray. Internally called Kamehameha.

    private bool reflecting;    // Controls whether it needs to make calculations and show the Kamehameha.
    // Vectors that store: the incoming light, the normal vector of the mirror, the position at which the light enters and leaves, the direction at which it leaves:
    private Vector3 incomingVec, normalVec, hitPoint, reflectVec;   
    
    private RaycastHit rayHit;          // Saves the hit when the raycast intersects with a collider.
    private bool hitOtherMirror;        // Stores specifically if a mirror has been hit.
    private bool hitMovingPlatform;     // Stores specifically if a moving platform was hit.

    // Use this for initialization
    void Start () {
        // We set the initial values for the two hitting bools:
        hitOtherMirror = false;
        hitMovingPlatform = false;
	}
    
    // Function that is called when a raycast has hit our mirror and a reflection is expected:
    public void Reflect(Vector3 inVec, Vector3 normal, Vector3 point)   // Parameters actually come from the Raycast.
    {
        // We update our vector to the values of the raycastHit:
        incomingVec = inVec;
        normalVec = normal;
        hitPoint = point;
        reflecting = true;  // We are now reflecting!
    }
	
	// Update is called once per frame
	void FixedUpdate () {
        if (reflecting){    // If we are reflecting:
            Kamehameha.transform.localScale = new Vector3(16, 16, 16);  // Set the size of our mirror's Kamehameha to the preliminary defaults: 16,16,16.
            Kamehameha.transform.position = hitPoint;                   // We set the Kamehameha's position to where the light hit.
            reflectVec= Vector3.Reflect(incomingVec, normalVec);        // We calculate the reflection vector, using our incoming and normal vectors.
            Kamehameha.transform.forward = reflectVec;                  // We make the Kamehameha look in the direction of the reflected vector.
            reflecting = false;                                         // After this execution we won't be reflecting anymore.

            if (Physics.Raycast(hitPoint, reflectVec, out rayHit))      // If our casted ray hits something:
            {   
                Debug.DrawRay(hitPoint, reflectVec * 1000, Color.blue);     // For debugging reasons, we display the ray.
                if (rayHit.collider.gameObject.CompareTag("Mirror")) { OtherMirror(rayHit); hitOtherMirror = true; }    // If we have hit a Mirror -> OtherMirror(). Hit mirror!
                else if (rayHit.collider.gameObject.CompareTag("MovingPlatform")) { hitMovingPlatform = true; }     // If we hit a moving platform, well we hit it... (?)
                else { hitOtherMirror = false; hitMovingPlatform = false; }     // Else, we ain't hitting anything, guys.

                if (rayHit.collider.gameObject.CompareTag("Trigger")) { TriggerTrigger(rayHit); }   // If we hit a Trigger, then we trigger it -> TriggerTrigger().
            }
            else   // If our ray didn't hit shit...
            {
                // ... then, well, nothing was hit:
                hitOtherMirror = false;
                hitMovingPlatform = false;
            }
        }
        else    // If we're not reflecting:
        {
            Kamehameha.transform.localScale = new Vector3(0, 0, 0); // We make the Kamehameha suuuuuuper tiny.
        }
        //transform.Rotate(new Vector3(0,1,0));
	}

    // Function that is called when another mirror is hit:
    void OtherMirror(RaycastHit mirrorHit)
    {
        Vector3 inVec = mirrorHit.point - hitPoint; // The incoming vector for the receiving mirror is the point where we were hit minus the point where it was hit.
        mirrorHit.collider.GetComponentInParent<Mirror>().Reflect(inVec, mirrorHit.normal, mirrorHit.point);    // We tell that mirror to reflect.
        Kamehameha.transform.localScale = new Vector3(16, 16, Vector3.Distance(mirrorHit.point, Kamehameha.transform.position) / 2);    // We make Kamehameha the length of the distance.
    }
    // Function that is called when a trigger is hit:
    void TriggerTrigger(RaycastHit rh)
    {
        rh.collider.gameObject.GetComponentInParent<Trigger>().pleaseTrigger(); // Tell the trigger to please trigger. Thanks.
    }
}
