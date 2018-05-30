using UnityEngine;

public class DarkOrbMaterialAnimation : MonoBehaviour {

    Material orbMaterial;
    Vector3 startColor;
    Vector3 differentColor;
    float pulsationDuration = 1.0f;
    Color currentColor;

    void Start () {
		orbMaterial = GetComponent<Renderer>().material;
    }
	
    void Update()
    {
        currentColor = Color.Lerp(Color.black, Color.red, Mathf.PingPong(Time.time, pulsationDuration));

        orbMaterial.SetColor("_MainColor", currentColor);
    }
}
