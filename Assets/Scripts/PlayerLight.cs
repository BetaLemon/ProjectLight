using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerLight : MonoBehaviour {

    enum LightMode { NEAR, FAR };

    public Light pointLight;
    public Light spotLight;

    public float defaultPointLightRange;
    public float defaultSpotLightIntensity;

    private LightMode lightMode;
    private float prevLightAxis = 0;

    float Lerp(float goal, float speed, float currentVal)
    {
        if (currentVal > goal)
        {
            if(currentVal - speed < goal) { return currentVal = goal; }
            return currentVal -= speed;
        }
        else if (currentVal < goal)
        {
            if (currentVal + speed > goal) { return currentVal = goal; }
            return currentVal += speed;
        }
        else return currentVal;
    }

	// Use this for initialization
	void Start () {
        lightMode = LightMode.NEAR;
        defaultPointLightRange = pointLight.range;
        defaultSpotLightIntensity = spotLight.intensity;
	}
	
	// Update is called once per frame
	void Update () {

        if(Input.GetAxis("LightSwitch") != 0 && prevLightAxis == 0)
        {
            if(lightMode == LightMode.NEAR) { lightMode = LightMode.FAR; }
            else if(lightMode == LightMode.FAR) { lightMode = LightMode.NEAR; }
            print(lightMode);
        }

        prevLightAxis = Input.GetAxis("LightSwitch");
        
        if(Input.GetAxis("LightMax") != 0)
        {
            pointLight.range += 2;
        }

        switch (lightMode)
        {
            case LightMode.NEAR:
                /*
                pointLight.range = defaultPointLightRange;
                spotLight.intensity = 0.0f;*/
                pointLight.range = Lerp(defaultPointLightRange, 1.5f, pointLight.range);
                spotLight.intensity = Lerp(0.0f, 5.0f, spotLight.intensity);
                break;
            case LightMode.FAR:
                /*
                pointLight.range = 8.0f;
                spotLight.intensity = defaultSpotLightIntensity;*/
                pointLight.range = Lerp(3.0f, 5f, pointLight.range);
                spotLight.intensity = Lerp(defaultSpotLightIntensity, 2f, spotLight.intensity);
                break;
            default:
                print("Error: wrong light mode.");
                break;
        }
	}
}
