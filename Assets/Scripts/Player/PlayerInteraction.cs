using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInteraction : MonoBehaviour {

    public Light StaffLight;

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
                print(hitColliders[i].gameObject.tag);
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
        print("Hit " + tmp + " triggers.");

        Physics.Raycast(StaffLight.transform.position, new Vector3(0, 0, 1), out rayHit);
        //if (rayHit.collider.gameObject.CompareTag("Mirror")) { Mirror(rayHit); }
        Vector3 reflectVec = Vector3.Reflect(rayHit.point-StaffLight.transform.position, rayHit.normal);
        Debug.DrawRay(rayHit.point, reflectVec, Color.green);
    }

    void LightOrb(Collider col)
    {
       col.GetComponent<LightOrb>().Interact(GetComponent<PlayerLight>().power);
    }

    void Mirror(RaycastHit mirrorHit)
    {
        //mirrorHit.collider.gameObject.Interact(mirrorHit.normal, mirrorHit.point);
    }
}
