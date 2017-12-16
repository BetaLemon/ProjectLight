using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInteraction : MonoBehaviour {

    public Light StaffLight;

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
                switch (hitColliders[i].gameObject.tag)
                {
                    case "LightOrb":
                        LightOrb(hitColliders[i]);
                        break;
                    default:break;
                }
                tmp++;
            }

        }
        print("Hit " + tmp + " triggers.");
    }

    void LightOrb(Collider col)
    {
       col.GetComponent<LightOrb>().Interact("shit");
    }
}
