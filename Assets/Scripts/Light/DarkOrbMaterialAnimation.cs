using UnityEngine;

public class DarkOrbMaterialAnimation : MonoBehaviour {

    Material orbMaterial;
    Vector3 startColor;
    Vector3 differentColor;
    float rePulsateTime = 4.0f;
    float pulsationDuration = 1.0f;
    float timeSinceLastPulseEnd = 0.0f;
    Color currentColor;

    void Start () {
		orbMaterial = GetComponent<Renderer>().material;
    }
	
    void Update()
    {

        timeSinceLastPulseEnd += Time.deltaTime;
        if (timeSinceLastPulseEnd >= rePulsateTime)
        {
            currentColor = Color.Lerp(Color.black, Color.red, Mathf.PingPong(Time.time, pulsationDuration));
            if (currentColor.r <= 0.1)
            {
                timeSinceLastPulseEnd = 0.0f;
                currentColor = Color.black;
            } 
        }

        orbMaterial.SetColor("_MainColor", currentColor);
    }
}
