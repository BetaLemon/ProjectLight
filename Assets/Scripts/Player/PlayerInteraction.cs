using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInteraction : MonoBehaviour {

    public GameStateScript gameStateDataScriptRef; //Reference to the Game/Global World Scene State

    public GameObject CylindricLight;
    public GameObject LightRayGeometry;

    private RaycastHit rayHit;
    float prevBaseInteraction;
    float pressedBaseInteraction;

    private PlayerInput input;
    private PlayerLight light;

    void Start () {
        //Reference Initializations:
        gameStateDataScriptRef = GameObject.FindGameObjectWithTag("GameState").GetComponent<GameStateScript>();

        input = GetComponent<PlayerInput>();
        light = GetComponent<PlayerLight>();
    }
	
	void FixedUpdate () {

        //Pause input:
        if (input.getInput("Pause") != 0)
        {
            gameStateDataScriptRef.PauseGame(true);
            gameStateDataScriptRef.SetSceneState(2);
        }

        pressedBaseInteraction = input.getInput("BaseInteraction");

        float amount = GetComponent<PlayerLight>().healthDrainAmmount;

        /// PASSIVE INTERACTION (Sphere Light)
        Collider[] hitColliders = Physics.OverlapSphere(CylindricLight.transform.position, GetComponent<PlayerLight>().lightSphere.range-5); //(Sphere center, Radius)
        int tmp = 0;
        for (int i = 0; i < hitColliders.Length; i++)
        {
            if (hitColliders[i].isTrigger)
            {
                if (hitColliders[i].gameObject.CompareTag("PlayerLight")) { continue; }
                switch (hitColliders[i].gameObject.tag)
                {
                    case "LightOrb":
                        if (input.isPressed("LightMax")) hitColliders[i].GetComponent<LightOrb>().ChargeOrb(Color.white, amount); //Attempt to charge the light orb if we are expanding the player light sphere radius (Default white from player white ray)
                        else if (input.isPressed("BaseInteraction")) hitColliders[i].GetComponent<LightOrb>().SubtractFromOrb(); //Attempt to subtract energy from the light orb if we press Q
                        break;
                    case "BlackInsect":
                        BlackInsect(hitColliders[i]);
                        break;
                    case "Mirror":
                        if (pressedBaseInteraction != 0 && prevBaseInteraction == 0) { FindObjectOfType<CameraScript>().setFocus(hitColliders[i].gameObject); }
                        break;
                    case "OpticalFiber":
                        // Make closest node work (switching reverse or not):
                        hitColliders[i].GetComponentInParent<OpticalFiber>().SetClosestNode(transform);

                        // Charge optical fiber:
                        
                        if (input.isPressed("LightMax")) hitColliders[i].GetComponent<OpticalFiber_Node>().AddCharge(amount);
                        else if (input.isPressed("BaseInteraction")) hitColliders[i].GetComponentInParent<OpticalFiber>().StartPlayerMode(transform);
                        break;
                    default:break;
                }
                tmp++;
            }
        }

        /// ACTIVE INTERACTION (Cylinder LightRay)
        if(GetComponent<PlayerLight>().getLightMode() == PlayerLight.LightMode.FAR) // If the player uses the Cylinder Light.
        {
            Debug.DrawRay(CylindricLight.transform.position, LightRayGeometry.transform.forward * light.maxLightCylinderScale * 2, Color.red);
            if (Physics.Raycast(CylindricLight.transform.position, LightRayGeometry .transform.forward, out rayHit, light.maxLightCylinderScale * 2))  //(vec3 Origin, vec3direction, vec3 output on intersection) If Raycast hits a collider.
            {
                float distCylPosHitPos = Vector3.Distance(getRayHit().point, CylindricLight.transform.position);

                // Specific game object interactions with light cylinder:
                if (rayHit.collider.gameObject.CompareTag("Mirror")) { Mirror(rayHit); } //Reflect mirror light
                if (rayHit.collider.gameObject.CompareTag("Filter")) { Filter(rayHit); } //Process light ray
                if (rayHit.collider.gameObject.CompareTag("LightOrb")) { rayHit.collider.GetComponentInParent<LightOrb>().ChargeOrb(Color.white,amount); } //Charge the light orb (Default white from player white ray)
                if (rayHit.collider.gameObject.CompareTag("Trigger")) { TriggerTrigger(rayHit); }
                if (rayHit.collider.gameObject.CompareTag("BlackInsect")) { BlackInsect(rayHit.collider); }
            }
        }

        prevBaseInteraction = pressedBaseInteraction;
    }

    void BlackInsect(Collider col)
    {
        col.gameObject.GetComponent<BlackInsect>().Hurt();
    }

    void Mirror(RaycastHit mirrorHit)
    {
        Vector3 inVec = mirrorHit.point - CylindricLight.transform.position;
        mirrorHit.collider.GetComponentInParent<Mirror>().Reflect(inVec, mirrorHit.normal, mirrorHit.point, Color.white);
        LightRayGeometry.transform.localScale = new Vector3(8, 8, Vector3.Distance(mirrorHit.point, LightRayGeometry.transform.position) / 2); // Limit the light ray's length to the object
    }

    void Filter(RaycastHit filterHit)
    {
        Vector3 inVec = filterHit.point - CylindricLight.transform.position;
        filterHit.collider.GetComponentInParent<RayFilter>().Process(inVec, filterHit.point);
        LightRayGeometry.transform.localScale = new Vector3(8, 8, Vector3.Distance(filterHit.point, LightRayGeometry.transform.position) / 2); // Limit the light ray's length to the object
    }

    void TriggerTrigger(RaycastHit rh)
    {
        rh.collider.gameObject.GetComponentInParent<Trigger>().pleaseTrigger();
    }

    public RaycastHit getRayHit()
    {
        return rayHit;
    }
}
