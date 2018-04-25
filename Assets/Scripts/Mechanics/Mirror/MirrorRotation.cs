using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MirrorRotation : MonoBehaviour
{
    private bool receivingInput = true; /// !!! THIS NEEDS TO BE MODIFIED!!

    public Transform MirrorCenter;

    public bool forw = true;
    public float test = 0;
    private float[] verticalClamp = { -80, 80};

    void Start()
    {
        verticalClamp[0] += MirrorCenter.localEulerAngles.y;
        verticalClamp[1] += MirrorCenter.localEulerAngles.y;
    }

    void Update()
    {
        //RotateVerticalAxis(forw);
        MirrorCenter.localEulerAngles = new Vector3(MirrorCenter.localEulerAngles.x, test, MirrorCenter.localEulerAngles.z);
        Debug.Log(MirrorCenter.localEulerAngles.y);
    }

    public void RotateVerticalAxis(bool forward)
    {
        float angle = 0.5f;
        if (forward)
        {
            float clamp = Clamp(MirrorCenter.localEulerAngles.y + angle, verticalClamp[0], verticalClamp[1]);
            MirrorCenter.localEulerAngles = new Vector3(MirrorCenter.localEulerAngles.x, clamp, MirrorCenter.localEulerAngles.z);
            //Debug.Log(clamp);
        }
        else
        {
            float clamp = Clamp(MirrorCenter.localEulerAngles.y - angle, verticalClamp[0], verticalClamp[1]);
            MirrorCenter.localEulerAngles = new Vector3(MirrorCenter.localEulerAngles.x, clamp, MirrorCenter.localEulerAngles.z);
            //Debug.Log(clamp);
        }
    }

    float Clamp(float value, float min, float max)
    {
        if(value > max) { value = max; }
        if(value < min) { value = min; }
        return value;
    }
}