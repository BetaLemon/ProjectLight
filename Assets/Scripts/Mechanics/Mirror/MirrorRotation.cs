using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MirrorRotation : MonoBehaviour {

    public Transform[] MirrorCenter;
    public Transform[] Frame;
    public Transform[] BottomGear;
    public Transform[] SideGear;

    private Vector2 currentAngle;
    private float frameYAngle;

    private Vector2 angleLimitX;
    private Vector2 angleLimitY;

    private Vector3 sideGearRotCenter;

    enum RotatingAxis { X, Y};
    private RotatingAxis axis;

    void Start()
    {
        currentAngle = new Vector2(transform.localEulerAngles.x, transform.localEulerAngles.y);
        angleLimitY = new Vector2(-80 + currentAngle.y, 80 + currentAngle.y);
        angleLimitX = new Vector2(-40 + currentAngle.x, -40 + currentAngle.x);
        sideGearRotCenter = MirrorCenter[0].position;
        frameYAngle = Frame[0].localEulerAngles.y;
    }

    void Update()
    {
        float InputX = Input.GetAxis("Mouse Y");
        float InputY = Input.GetAxis("Mouse X");
        if (Mathf.Abs(InputY) > Mathf.Abs(InputX)) RotateY(InputY);
        if (Mathf.Abs(InputX) > Mathf.Abs(InputY)) RotateX(InputX);
    }

    public void RotateY(float angle)
    {
        float clamp = angle + currentAngle.y;
        clamp = Mathf.Clamp(clamp, angleLimitY.x, angleLimitY.y);

        foreach(Transform t in MirrorCenter)
        {
            t.localEulerAngles = new Vector3(t.localEulerAngles.x, clamp, t.localEulerAngles.z);
        }
        foreach (Transform t in Frame)
        {
            t.localEulerAngles = new Vector3(t.localEulerAngles.x, clamp, t.localEulerAngles.z);
        }
        foreach (Transform t in BottomGear)
        {
            t.localEulerAngles = new Vector3(t.localEulerAngles.x, clamp, t.localEulerAngles.z);
        }
        foreach (Transform t in SideGear)
        {
            t.RotateAround(sideGearRotCenter, Vector3.up, clamp - frameYAngle);
        }
        currentAngle.y = MirrorCenter[0].localEulerAngles.y;
        frameYAngle = Frame[0].localEulerAngles.y;
    }

    public void RotateX(float angle)
    {
        float clamp = angle + currentAngle.x;
        //clamp = Mathf.Clamp(clamp, angleLimitX.x, angleLimitX.y);

        foreach (Transform t in MirrorCenter)
        {
            //t.localEulerAngles = new Vector3(clamp, t.localEulerAngles.y, t.localEulerAngles.z);
            //t.RotateAroundLocal(t.position, angle);
        }
       /* foreach (Transform t in SideGear)
        {
            t.localEulerAngles = new Vector3(clamp, t.localEulerAngles.y, t.localEulerAngles.z);
        }
        */
        currentAngle.x = MirrorCenter[0].localEulerAngles.x;
    }
}
