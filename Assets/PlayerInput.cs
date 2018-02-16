using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInput : MonoBehaviour {

    // The idea behind this script is that it should control all the player input, so that other scripts
    // can read the info processed here so they can do whatever they need, but input is isolated here.

    /* Input that needs to be ported:
     * -> PlayerInteraction:
     *   - BaseInteraction
     *   - LightMax
     *   
     * -> PlayerController:
     *      - Horizontal
     *      - Vertical
     *      - Jump
     *      
     * -> PlayerLight:
     *      - LightSwitch
     *      - LightMax
     * */

    private Dictionary<string, bool> input = new Dictionary<string, bool>();

	// Use this for initialization
	void Start () {
        input.Add("Horizontal", false);
        input.Add("Vertical", false);
        input.Add("Jump", false);

        input.Add("BaseInteraction", false);
        input.Add("LightMax", false);
        input.Add("LightSwitch", false);
    }
	
	// Update is called once per frame
	void Update () {
        input["Horizontal"] = Input.GetAxis("Horizontal") != 0;
        input["Vertical"] = Input.GetAxis("Vertical") != 0;
        input["Jump"] = Input.GetAxis("Jump") != 0;

        input["BaseInteraction"] = Input.GetAxis("BaseInteraction") != 0;
        input["LightMax"] = Input.GetAxis("LightMax") != 0;
        input["LightSwitch"] = Input.GetAxis("LightSwitch") != 0;
    }

    public bool getInput(string id)
    {
        return input[id];
    }
}
