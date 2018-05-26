using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialScript : MonoBehaviour {

    // Steps:
    enum Steps
    {
        WELCOME, SHOW_BASICS, MOVEMENT, LIGHT_INTRO, LIGHT_INTRO2, LIGHT_E, LIGHT_E_DONE
    }

    #region Variables
    public Transform playerSpawn;
    public string KeyboardInputAxis;
    public string GamepadInputAxis;

    public TextMesh text;
    [TextArea(2, 20)]   // (min, max)
    public List<string> textLines;
    private int currentText;

    public Transform PressQ;
    private bool qState;

    private Steps step;
    private Steps prevStep;

    private float stepDelay = 1f;
    private float dt;

    private PlayerInput input;
    public GameObject OrbPrefab;
    private LightOrb orb;
    #endregion

    // Use this for initialization
    void Start () {
        step = 0; prevStep = step-1;
        currentText = 0;
        dt = 0;
        input = PlayerInput.instance;
	}
	
	// Update is called once per frame
	void Update () {
        if (Input.GetKey(KeyCode.M)) { Player.instance.transform.position = playerSpawn.position; }

        if(Input.GetAxis(KeyboardInputAxis) != 0 || Input.GetAxis(GamepadInputAxis) != 0)   // Button was pressed.
        {
            if (qState && dt > stepDelay) { step++; dt = 0; }
        }

        dt += Time.deltaTime;

        //if (step == prevStep) return;

        text.text = textLines[(int)step];   // Text for that step.
        // Custom behaviour;
        switch (step)
        {
            case Steps.WELCOME:
                qState = true;
                break;
            case Steps.SHOW_BASICS:
                qState = true;
                break;
            case Steps.MOVEMENT:
                qState = false;
                if(input.isPressed("Horizontal") || input.isPressed("Vertical"))
                {
                    if(dt > stepDelay) step++;
                }
                break;
            case Steps.LIGHT_INTRO:
                qState = true;
                break;
            case Steps.LIGHT_INTRO2:
                qState = true;
                break;
            case Steps.LIGHT_E:
                qState = false;
                if (orb == null) {
                    orb = GameObject.Instantiate(OrbPrefab).GetComponent<LightOrb>();
                    orb.transform.parent = transform;
                    orb.transform.position = playerSpawn.position;
                    orb.orbCharge = 0;
                }
                if(orb.orbCharge > 2) { if (dt > stepDelay) step++; }
                break;
            case Steps.LIGHT_E_DONE:
                qState = true;
                break;
            default:
                Debug.Log("Default."); step = 0; break;
        }

        SetPressQState(qState);

        prevStep = step;
	}

    void SetPressQState(bool isEnabled)
    {
        if(PressQ.gameObject.activeSelf != isEnabled)
            PressQ.gameObject.SetActive(isEnabled);
    }
}
