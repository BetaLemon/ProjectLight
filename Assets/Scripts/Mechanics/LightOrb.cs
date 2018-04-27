using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightOrb : MonoBehaviour {

    private Player thePlayer; //Reference to the player
    private Trigger orbTrigger; // ! We could make it so that the light orb could contain multiple triggers.
    private GameObject OrbGeometry; 
    //--------

    public Light glow;

    private float orbIntensity;                //Either: 10(has charge enrgy) or 0(no charge enrgy), defined on startup according to orbCharge.
    public float orbCharge;                    //Current orb charge. Is public in order to be set up on level design
    private float maxOrbCharge = 10f;
    private float minOrbCharge = 0f;
    private float orbGlowRangeFactor = 6f;     //Reduces orb glow range the higher it is
    private float minOrbGlowRange = 1.5f;      //The orb starts to glow directly from this range

    //-------- COLOR RESTRICTIONS (6) ---------
    //Red: Color.red
    //Yellow: Color.red + Color.green
    //Green: Color.green
    //Blue: Color.blue
    //Purple: Color.red + Color.blue
    //Pink: Color.red + Color.white
    //-----------------------------------------

    public Color color = Color.white; //The orb's current color. Is public in order to be set up on level design
    public float autoRefillAmount = 0;
    public float refillDelay = 10;
    private float currentRefillDelay = 0;
    private bool waitingRefill = false;
    public GameObject chargeSphere;

    private float Lerp(float goal, float speed, float currentVal)
    {
        if (currentVal > goal)
        {
            if (currentVal - speed < goal) { return currentVal = goal; }
            return currentVal -= speed;
        }
        else if (currentVal < goal)
        {
            if (currentVal + speed > goal) { return currentVal = goal; }
            return currentVal += speed;
        }
        else return currentVal;
    }

    void Start ()
    {
        OrbGeometry = transform.GetChild(1).gameObject; // Assign Orb Geometry reference to the second child of this gameObject
        thePlayer = Player.instance;  // Maybe use tags instead?
        orbTrigger = GetComponent<Trigger>();
    }

    public void SubtractFromOrb()
    {
        float exchange = thePlayer.GetComponent<PlayerLight>().healthDrainAmmount; //Is the same equivalent value for subtracting as for charging (You give what you can take)
        orbCharge -= exchange; //(orb subtraction)
        if (orbCharge > 0) thePlayer.GetComponent<Player>().health += exchange; //Increase player health from orb absortion as long as there's energy (The player isn't dead)
    }
    public void ChargeOrb(Color enteringColor, float amount)
    {
        if (enteringColor == color || orbCharge == 0)
        {
            color = enteringColor;
            //float exchange = thePlayer.GetComponent<PlayerLight>().healthDrainAmmount;
            orbCharge += amount; //The orb is filled with the standard ammount, which is the same the wizard loses from straignin his mana (orb deposition)
        }
        currentRefillDelay = 0;
    }
    public void SetOrbCharge(float amount)
    {
        orbCharge = amount;
    }

    void Update()
    {
        if (chargeSphere != null) { OrbChargeSphere(); }
        else { OrbGeometry.GetComponent<MeshRenderer>().materials[0].SetColor("_MKGlowColor", color); }

        RefillSystem();

        //Update glow color:
        glow.color = color;
        //OrbGeometry.GetComponent<MeshRenderer>().materials[0].SetColor("_MKGlowColor", color);

        //Orb energy charge limits:
        if (orbCharge > maxOrbCharge) { orbCharge = maxOrbCharge; waitingRefill = false; }
        else if (orbCharge < minOrbCharge) orbCharge = minOrbCharge;

        glow.range = minOrbGlowRange + orbCharge / orbGlowRangeFactor; //Orb light extension radius starts at a minimum, and extends the same as the current charge divided by a decreasing factor

        //Adjust glow intensity according to orb energy charge
        if (orbCharge > 0) orbIntensity = Lerp(10, 1, orbIntensity);
        else orbIntensity = Lerp(0, 1, orbIntensity);
        glow.intensity = orbIntensity;

        if(orbTrigger != null) {
            if (orbTrigger.type == Trigger.TriggerType.ON_COLOUR)
            {
                orbTrigger.pleaseTrigger(orbCharge, color);
            }
            else { orbTrigger.pleaseTrigger(orbCharge); }
        }
    }

    void OrbChargeSphere()
    {
        chargeSphere.transform.localScale = new Vector3(orbCharge / maxOrbCharge, orbCharge / maxOrbCharge, orbCharge / maxOrbCharge);
        chargeSphere.GetComponent<MeshRenderer>().materials[0].SetColor("_MKGlowColor", color);
    }

    void RefillSystem()
    {
        if (autoRefillAmount <= 0) return;
        if (orbCharge <= 0) { waitingRefill = true; }
        if (waitingRefill) { currentRefillDelay += Time.deltaTime; }
        if (waitingRefill && currentRefillDelay > refillDelay) orbCharge += autoRefillAmount * Time.deltaTime;
    }
}
