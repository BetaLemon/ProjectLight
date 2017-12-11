﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightOrb : MonoBehaviour {

    public Light glow;

    private float orbIntensity;           //Either: 10(has charge enrgy) or 0(no charge enrgy), defined on startup according to orbCharge.
    public float orbCharge;               //Current orb charge. Is public in order to be set up on level design
    private float maxOrbCharge = 20f;
    private float minOrbCharge = 0;
    private float minOrbRange = 1.5f;
    private float maxOrbRange = 3.5f;

    //-------- POSSIBLE COLORS ---------
    //Red: Color.red
    //Orange: ???
    //Yellow: ???
    //Green: Color.blue + Color.green
    //Blue: Color.blue
    //Purple: Color.red + Color.blue
    //Pink: ???

    public Color glowColor = Color.white; //Start orb color. Is public in order to be set up on level design

    void Start () {
        //Assign start glow color:
        glow.color = glowColor;

        //Assign start glow intensity according to start orb energy charge:
        if (orbCharge > 0) orbIntensity = 10;
        else orbIntensity = 0;
        glow.intensity = orbIntensity;
    }
	
	void Update () {
        //DEBUG SECTION:
        //print(GetComponent<Light>().intensity);

        //Orb energy charge limits:
        if (orbCharge > maxOrbCharge) orbCharge = maxOrbCharge;
        else if (orbCharge < minOrbCharge) orbCharge = minOrbCharge;

        //Glow range limits:
        if (glow.range > maxOrbRange) glow.range = maxOrbRange;
        else if (glow.range < minOrbRange) glow.range = minOrbRange;

        glow.range = 1.5f + orbCharge / 10;

        //Adjust glow intensity according to orb energy charge
        if (orbCharge > 0) orbIntensity = 10;
        else orbIntensity = 0;
        glow.intensity = orbIntensity;
    }
}
