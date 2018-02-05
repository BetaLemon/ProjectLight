using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mirror : MonoBehaviour {

    public GameObject Kamehameha;

    private bool reflecting;
    private Vector3 incomingVec, normalVec, hitPoint, reflectVec;

    private RaycastHit rayHit;
    private bool hitOtherMirror;
    private bool hitMovingPlatform;

    // Use this for initialization
    void Start () {
        hitOtherMirror = false;
        hitMovingPlatform = false;
	}

    public void Reflect(Vector3 inVec, Vector3 normal, Vector3 point)
    {
        incomingVec = inVec;
        normalVec = normal;
        hitPoint = point;
        reflecting = true;
    }
	
	// Update is called once per frame
	void FixedUpdate () {
        if (reflecting){
            Kamehameha.transform.localScale = new Vector3(16, 16, 16);
            Kamehameha.transform.position = hitPoint;
            reflectVec= Vector3.Reflect(incomingVec, normalVec);
            Kamehameha.transform.forward = reflectVec;
            reflecting = false;

            if (Physics.Raycast(hitPoint, reflectVec, out rayHit))
            {   
                Debug.DrawRay(hitPoint, reflectVec * 1000, Color.blue);
                if (rayHit.collider.gameObject.CompareTag("Mirror")) { OtherMirror(rayHit); hitOtherMirror = true; }
                else if (rayHit.collider.gameObject.CompareTag("MovingPlatform")) { hitMovingPlatform = true; }
                else { hitOtherMirror = false; hitMovingPlatform = false; }

                if (rayHit.collider.gameObject.CompareTag("Trigger")) { TriggerTrigger(rayHit); }
            }
            else
            {
                hitOtherMirror = false;
                hitMovingPlatform = false;
            }
        }
        else
        {
            Kamehameha.transform.localScale = new Vector3(0, 0, 0);
        }
        //transform.Rotate(new Vector3(0,1,0));
	}

    void OtherMirror(RaycastHit mirrorHit)
    {
        Vector3 inVec = mirrorHit.point - hitPoint;
        mirrorHit.collider.GetComponentInParent<Mirror>().Reflect(inVec, mirrorHit.normal, mirrorHit.point);
        Kamehameha.transform.localScale = new Vector3(16, 16, Vector3.Distance(mirrorHit.point, Kamehameha.transform.position) / 2);
    }

    void TriggerTrigger(RaycastHit rh)
    {
        rh.collider.gameObject.GetComponentInParent<Trigger>().pleaseTrigger();
    }
}
