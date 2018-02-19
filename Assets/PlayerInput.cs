using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInput : MonoBehaviour {

    // The idea behind this script is that it should control all the player input, so that other scripts
    // can read the info processed here so they can do whatever they need, but input is isolated here.

    private Dictionary<string, float> input = new Dictionary<string, float>();

	// Use this for initialization
	void Start () {
        input.Add("Horizontal", 0);
        input.Add("Vertical", 0);
        input.Add("Jump", 0);

        input.Add("BaseInteraction", 0);
        input.Add("LightMax", 0);
        input.Add("LightSwitch", 0);
    }
	
	// Update is called once per frame
	void Update () {
        input["Horizontal"] = Input.GetAxis("Horizontal");
        input["Vertical"] = Input.GetAxis("Vertical");
        //input["Jump"] = Input.GetAxis("Jump");
        input["Jump"] = 0;  // Jump has been disabled.

        input["BaseInteraction"] = Input.GetAxis("BaseInteraction");
        input["LightMax"] = Input.GetAxis("LightMax");
        input["LightSwitch"] = Input.GetAxis("LightSwitch");
    }

    public bool isPressed(string id)
    {
        return (input[id] > 0 || input[id] < 0);
    }

    public float getInput(string id)
    {
        return input[id];
    }
}
