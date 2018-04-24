using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightRay : MonoBehaviour {

    public GameObject LightRayGeometry;
    public float rotateSpeed;
    public Color color = Color.white; //Color of the light ray, is white by default
    public bool Interactive = false;

    private RaycastHit rayHit;

    float amount;

    void Start()
    {
        amount = 0.05f; //GetComponent<PlayerLight>().healthDrainAmmount;
    }

    void Update () {
        //renderer.material.SetFloat("_Blend", someFloatValue);
        LightRayGeometry.GetComponent<MeshRenderer>().materials[0].SetColor("_MKGlowColor", color);
        LightRayGeometry.transform.Rotate(0, 0, rotateSpeed);

        if (Interactive)
        {
            Debug.DrawRay(LightRayGeometry.transform.position, LightRayGeometry.transform.forward * LightRayGeometry.transform.localScale.z * 2, Color.red);
            if (Physics.Raycast(LightRayGeometry.transform.position, LightRayGeometry.transform.forward, out rayHit, LightRayGeometry.transform.localScale.z * 2))  //(vec3 Origin, vec3direction, vec3 output on intersection) If Raycast hits a collider.
            {
                // Specific game object interactions with light cylinder:
                if (rayHit.collider.gameObject.CompareTag("Mirror")) { Mirror(rayHit); } //Reflect mirror light
                if (rayHit.collider.gameObject.CompareTag("Filter")) { Filter(rayHit); } //Process light ray
                if (rayHit.collider.gameObject.CompareTag("LightOrb")) { rayHit.collider.GetComponentInParent<LightOrb>().ChargeOrb(color, amount); } //Charge the light orb (Default white from player white ray)
                if (rayHit.collider.gameObject.CompareTag("Trigger")) { TriggerTrigger(rayHit); }
                if (rayHit.collider.gameObject.CompareTag("BlackInsect")) { BlackInsect(rayHit.collider); }
                if (rayHit.collider.gameObject.CompareTag("Prism")) { Prism(rayHit); }
            }
        }
	}

    public void SetRayScale(Vector3 newScale)
    {
        LightRayGeometry.transform.localScale = newScale;
    }

    #region InteractionFunctions
    void BlackInsect(Collider col)
    {
        col.gameObject.GetComponent<BlackInsect>().Hurt();
    }

    void Mirror(RaycastHit mirrorHit)
    {
        Vector3 inVec = mirrorHit.point - LightRayGeometry.transform.position;
        mirrorHit.collider.GetComponentInParent<Mirror>().Reflect(inVec, mirrorHit.normal, mirrorHit.point, Color.white);
        LightRayGeometry.transform.localScale = new Vector3(8, 8, Vector3.Distance(mirrorHit.point, LightRayGeometry.transform.position) / 2); // Limit the light ray's length to the object
    }

    void Filter(RaycastHit filterHit)
    {
        Vector3 inVec = filterHit.point - LightRayGeometry.transform.position;
        filterHit.collider.GetComponentInParent<RayFilter>().Process(inVec, filterHit.point);
        LightRayGeometry.transform.localScale = new Vector3(8, 8, Vector3.Distance(filterHit.point, LightRayGeometry.transform.position) / 2); // Limit the light ray's length to the object
    }

    void TriggerTrigger(RaycastHit rh)
    {
        rh.collider.gameObject.GetComponentInParent<Trigger>().pleaseTrigger();
    }

    void Prism(RaycastHit rh)
    {
        Vector3 inVec = rh.point - LightRayGeometry.transform.position;
        rh.collider.GetComponentInParent<Prism>().Process(inVec, rh.point, rh.normal);
        LightRayGeometry.transform.localScale = new Vector3(8, 8, Vector3.Distance(rh.point, LightRayGeometry.transform.position));
    }
    #endregion
}
