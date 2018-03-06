using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RayFilter : MonoBehaviour {

    // Filters process light rays and convert them to another colour

    public GameObject Kamehameha;   // Stores the cylinder that represents the player's light ray. Internally called Kamehameha.

    private bool processing;
    // Vectors that store: the incoming light, the normal vector of the mirror, the position at which the light enters and leaves, the direction at which it leaves:
    private Vector3 incomingVec, hitPoint, processVec;

    private RaycastHit rayHit;      // Saves the hit when the raycast intersects with a collider.

    // Use this for initialization
    void Start()
    {

    }

    // Function that is called when a raycast has hit our filter and a colour change is expected:
    public void Process(Vector3 inVec, Vector3 point)   // Parameters actually come from the Raycast.
    {
        // We update our vector to the values of the raycastHit:
        incomingVec = inVec;
        hitPoint = point;
        processing = true;
    }

    // Update is called once per frame
    void FixedUpdate () {
        if (processing)
        {    // If we are reflecting:
            Kamehameha.transform.position = hitPoint;                       // We set the Kamehameha's position to where the light hit.
            Kamehameha.transform.forward = incomingVec;                     // We make the Kamehameha look in the direction of the reflected vector.
            processing = false;                                             // After this execution we won't be processing anymore.

            if (Physics.Raycast(hitPoint, processVec, out rayHit))      // If our casted ray hits something:
            {
                Debug.DrawRay(hitPoint, processVec * 1000, Color.blue);     // For debugging reasons, we display the ray.
                float distCylPosHitPos = Vector3.Distance(hitPoint, Kamehameha.transform.position);

                if (distCylPosHitPos < 15)
                {
                    if (rayHit.collider.gameObject.CompareTag("Mirror")) { Mirror(rayHit); }    // If we have hit a Mirror -> Mirror(). Hit mirror!
                    if (rayHit.collider.gameObject.CompareTag("Trigger")) { TriggerTrigger(rayHit); }   // If we hit a Trigger, then we trigger it -> TriggerTrigger().
                    if (rayHit.collider.gameObject.CompareTag("LightOrb")) { rayHit.collider.GetComponentInParent<LightOrb>().ChargeOrb(); } //Charge the light orb
                }
                Kamehameha.transform.localScale = new Vector3(8, 8, Vector3.Distance(hitPoint, rayHit.point) / 2);    // The length is the distance between the point of entering light
                                                                                                                      // and where the raycast hits on the other object.
            }
            else   // If our ray didn't hit shit...
            {
                // ... then, well, nothing was hit:
                Kamehameha.transform.localScale = new Vector3(8, 8, 15);  // Set to max length.
            }
        }
        else    // If we're not reflecting:
        {
            Kamehameha.transform.localScale = new Vector3(0, 0, 0); // We make the Kamehameha suuuuuuper tiny.
        }
    }
    // Function that is called when a mirror is hit:
    void Mirror(RaycastHit mirrorHit)
    {
        print("Mirror was hit by filtered ray");
        Vector3 inVec = mirrorHit.point - hitPoint; // The incoming vector for the receiving mirror is the point where we were hit minus the point where it was hit.
        mirrorHit.collider.GetComponentInParent<Mirror>().Reflect(inVec, mirrorHit.normal, mirrorHit.point);    // We tell that mirror to reflect.
        Kamehameha.transform.localScale = new Vector3(8, 8, Vector3.Distance(mirrorHit.point, Kamehameha.transform.position) / 2);    // We make Kamehameha the length of the distance.
    }
    // Function that is called when a trigger is hit:
    void TriggerTrigger(RaycastHit rh)
    {
        rh.collider.gameObject.GetComponentInParent<Trigger>().pleaseTrigger(); // Tell the trigger to please trigger. Thanks.
    }
}
