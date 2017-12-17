using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInteraction : MonoBehaviour {

    public Light StaffLight;
    public GameObject Kamehameha;

    private RaycastHit rayHit;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void FixedUpdate () {
        Collider[] hitColliders = Physics.OverlapSphere(StaffLight.transform.position, 2);
        int tmp = 0;
        for (int i = 0; i < hitColliders.Length; i++)
        {
            if (hitColliders[i].isTrigger)
            {
                if (hitColliders[i].gameObject.CompareTag("PlayerLight")) { continue; }
                //print(hitColliders[i].gameObject.tag);
                switch (hitColliders[i].gameObject.tag)
                {
                    case "LightOrb":
                        print("Hello");
                        LightOrb(hitColliders[i]);
                        break;
                    default:break;
                }
                tmp++;
            }

        }
        //print("Hit " + tmp + " triggers.");
       
        if(Physics.Raycast(StaffLight.transform.position, transform.forward, out rayHit))
        {   //debug
            Vector3 reflectVec = Vector3.Reflect(rayHit.point - StaffLight.transform.position, rayHit.normal);
            //print(reflectVec.x + " " + reflectVec.y + " " + reflectVec.z);
            Debug.DrawRay(rayHit.point, reflectVec * 1000, Color.green);
            //end debug
            if (rayHit.collider.gameObject.CompareTag("Mirror")) { Mirror(rayHit); }
        }
    }

    void LightOrb(Collider col)
    {
       col.GetComponent<LightOrb>().Interact(GetComponent<PlayerLight>().power);
    }

    void Mirror(RaycastHit mirrorHit)
    {
        Vector3 inVec = mirrorHit.point - StaffLight.transform.position;
        mirrorHit.collider.GetComponentInParent<Mirror>().Reflect(inVec, mirrorHit.normal, mirrorHit.point);
        Kamehameha.transform.localScale = new Vector3(16, 16, Vector3.Distance(mirrorHit.point, Kamehameha.transform.position)/2);
    }
}
