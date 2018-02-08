using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightOrb : MonoBehaviour {

    private GameObject thePlayer; //Reference to the player
    //--------

    public Light glow;

    private float orbIntensity;           //Either: 10(has charge enrgy) or 0(no charge enrgy), defined on startup according to orbCharge.
    public float orbCharge;               //Current orb charge. Is public in order to be set up on level design
    private float maxOrbCharge = 10f;
    private float minOrbCharge = 0f;
    private float minOrbRange = 1.5f;
    private float maxOrbRange = 4.5f;

    //-------- POSSIBLE ORB COLORS ---------
    //Red: Color.red
    //Orange: ???
    //Yellow: ???
    //Green: Color.blue + Color.green
    //Blue: Color.blue
    //Purple: Color.red + Color.blue
    //Pink: ???

    public Color glowColor = Color.white; //Start orb color. Is public in order to be set up on level design

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

    void Start () {

        thePlayer = GameObject.Find("Player");  // Maybe use tags instead?
        //Assign start glow color:
        glow.color = glowColor;
    }

    public void Interact(float healthDrainLossAmmount) //Used for external interaction from the player. 
    {
        print("debugging your ass.");
        //The health drainage and deposition from and towards light orbs is equal to the health loss ammount of the wizard when straining his magic
        if (Input.GetAxis("BaseInteraction") != 0) {
            orbCharge -= healthDrainLossAmmount; //(orb subtraction)
            if(orbCharge > 0) thePlayer.GetComponent<Player>().health += healthDrainLossAmmount; //Increase player health from orb absortion as long as there's energy
        }
        if (Input.GetAxis("LightMax") != 0) {
            print("Increasing orb charge by: " + healthDrainLossAmmount);
            orbCharge += healthDrainLossAmmount; //The orb is filled with the same ammount of mana the wizard loses (orb deposition)
        }
    }

    void Update()
    {
        //DEBUG SECTION:
        //print(GetComponent<Light>().intensity);

        //Orb energy charge limits:
        if (orbCharge > maxOrbCharge) orbCharge = maxOrbCharge;
        else if (orbCharge < minOrbCharge) orbCharge = minOrbCharge;

        //Glow range limits:
        if (glow.range > maxOrbRange) glow.range = maxOrbRange;
        else if (glow.range < minOrbRange) glow.range = minOrbRange;

        glow.range = 1.5f + orbCharge / 10; //Orb light extension radius starts at 1.5, and extends the same as the current charge divided by a decreasing factor

        //Adjust glow intensity according to orb energy charge
        if (orbCharge > 0) orbIntensity = Lerp(10, 1, orbIntensity);
        else orbIntensity = Lerp(0, 1, orbIntensity);
        glow.intensity = orbIntensity;
    }

    /*private void OnCollisionStay(Collision other)
    {
        if(other.collider.gameObject.tag == "Player")
        {
            print("shit");
        }
    }*/
}
