using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialScript : MonoBehaviour {

    public Transform playerSpawn;
    public string KeyboardInputAxis;
    public string GamepadInputAxis;

    public TextMesh text;
    [TextArea(5, 20)]
    public List<string> textLines;
    private int currentText;

    private int step;
    private int prevStep;

	// Use this for initialization
	void Start () {
        step = 0; prevStep = -1;
        currentText = 0;
	}
	
	// Update is called once per frame
	void Update () {
        if (Input.GetKey(KeyCode.M)) { Player.instance.transform.position = playerSpawn.position; }

        if(Input.GetAxis(KeyboardInputAxis) != 0 || Input.GetAxis(GamepadInputAxis) != 0)   // Button was pressed.
        {
            step++;
        }

        if (step == prevStep) return;

        switch (step)
        {
            case 0:
                text.text = textLines[0];
                break;
            case 1:
                text.text = textLines[1];
                break;
            default:
                Debug.Log("Default."); step = 0; break;
        }

        prevStep = step;
	}
}
