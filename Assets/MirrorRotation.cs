using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MirrorRotation : MonoBehaviour {

    public Transform[] VerticalAxisRotator;
    public Transform[] HorizontalAxisRotator;
    public Transform[] CrankRotator;
    public Transform[] SideGearRotator;

    public float VerticalAngle;
    public float HorizontalAngle;

    // Use this for initialization
    void Start () {
        VerticalAngle = HorizontalAngle = 0;
	}
	
	// Update is called once per frame
	void Update () {
		for(int i = 0; i < VerticalAxisRotator.Length; i++)
        {
            VerticalAxisRotator[i].Rotate(new Vector3(0, 0, VerticalAngle));
        }
        for (int i = 0; i < HorizontalAxisRotator.Length; i++)
        {
            HorizontalAxisRotator[i].Rotate(new Vector3(HorizontalAngle, 0, 0));
        }
        for(int i = 0; i < CrankRotator.Length; i++)
        {
            CrankRotator[i].Rotate(new Vector3(VerticalAngle, 0, 0));
        }
        for (int i = 0; i < SideGearRotator.Length; i++)
        {
            SideGearRotator[i].Rotate(new Vector3(0, 0, -VerticalAngle));
        }
    }
}
