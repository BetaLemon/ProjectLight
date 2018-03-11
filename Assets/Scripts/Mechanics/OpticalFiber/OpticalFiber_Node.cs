using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpticalFiber_Node : MonoBehaviour {

    public float charge = 0;
    private float maxCharge = 100;
    public Transform chargeLight;

	// Use this for initialization
	void Start () {
        Transform[] children = GetComponentsInChildren<Transform>();
        for(int i = 0; i < children.Length; i++)
        {
            if(children[i].name[0] != 'n' && children[i].name[0] != 'N')
            {
                chargeLight = children[i];
                break;
            }
        }
	}
	
	// Update is called once per frame
	void Update () {
		if(charge > maxCharge) { charge = maxCharge; }
        if(charge < 0) { charge = 0; }

        chargeLight.localScale = (new Vector3(charge, charge, charge) / 10);
	}
}
