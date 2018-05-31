using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialScript : MonoBehaviour {

    // Steps:
    enum Steps
    {
        WELCOME, SHOW_BASICS, MOVEMENT, LIGHT_INTRO, LIGHT_INTRO2, LIGHT_E, LIGHT_E_DONE, LIGHT_Q, LIGHT_Q_DONE, LIGHT_R_INTRO, LIGHT_R_INTRO2,
        LIGHT_R, LIGHT_R_DONE, ENLIGHTENED, TERRIBLE_JOKE, FINISHED
    }

    enum CamID { DEFAULT, MOVE, RAY, DONE }

    #region Variables
    public Transform playerSpawn;
    public Transform maxOrbSpawn;
    public Transform rayOrbSpawn;
    public Transform gameStartSpawn;
    public string KeyboardInputAxis;
    public string GamepadInputAxis;

    public TextMesh text;
    [TextArea(2, 20)]   // (min, max)
    public List<string> textLines;

    public Transform PressQ;
    private bool qState;
    private bool playerMove;
    private bool prevPlayerMove;

    private Steps step;
    private Steps prevStep;

    private float stepDelay = 1f;
    private float dt;

    private PlayerInput input;
    public GameObject OrbPrefab;
    private LightOrb orb;

    //public Cinemachine.CinemachineVirtualCamera defaultCamera;
    //public Cinemachine.CinemachineVirtualCamera playerMoveCamera;
    //public Cinemachine.CinemachineVirtualCamera rayCamera;
    public List<Cinemachine.CinemachineVirtualCamera> cameras;
    private CamID currentID;
    #endregion

    // Use this for initialization
    void Start () {
        SetupTutorial();
	}

    // Update is called once per frame
    void Update () {

        if (Input.GetAxis(KeyboardInputAxis) != 0 || Input.GetAxis(GamepadInputAxis) != 0)   // Button was pressed.
        {
            if (qState && dt > stepDelay) { NextStep(); }
        }

        if (step != Steps.MOVEMENT) { dt += Time.deltaTime; }

        //if (step == prevStep) return;
        if (step == Steps.FINISHED) FinishTutorial();

        text.text = textLines[(int)step];   // Text for that step.
        // Custom behaviour;
        switch (step)
        {
            case Steps.WELCOME:
                qState = true;
                playerMove = false;
                SetCamera(CamID.DEFAULT);
                break;
            case Steps.SHOW_BASICS:
                qState = true;
                playerMove = false;
                break;
            case Steps.MOVEMENT:
                qState = false;
                playerMove = true;
                SetCamera(CamID.MOVE);
                if(input.isPressed("Horizontal") || input.isPressed("Vertical"))
                {
                    if (dt > stepDelay) { NextStep(); }
                    dt += Time.deltaTime;
                }
                break;
            case Steps.LIGHT_INTRO:
                qState = true;
                playerMove = false;
                SetCamera(CamID.DEFAULT);
                break;
            case Steps.LIGHT_INTRO2:
                qState = true;
                playerMove = false;
                break;
            case Steps.LIGHT_E:
                qState = false;
                playerMove = true;
                if (orb == null) {
                    orb = GameObject.Instantiate(OrbPrefab).GetComponent<LightOrb>();
                    orb.transform.parent = transform;
                    orb.transform.position = maxOrbSpawn.position;
                    orb.orbCharge = 0;
                }
                if(orb.orbCharge > 2 && dt > stepDelay*2) { NextStep(); }
                break;
            case Steps.LIGHT_E_DONE:
                qState = true;
                playerMove = false;
                break;
            case Steps.LIGHT_Q:
                qState = false;
                playerMove = true;
                if (orb.orbCharge <= 0 && dt > stepDelay) { NextStep(); }
                break;
            case Steps.LIGHT_Q_DONE:
                qState = true;
                playerMove = false;
                break;
            case Steps.LIGHT_R_INTRO:
                qState = true;
                playerMove = false;
                break;
            case Steps.LIGHT_R_INTRO2:
                qState = true;
                playerMove = false;
                orb.orbCharge = 0;
                break;
            case Steps.LIGHT_R:
                qState = false;
                playerMove = false;
                SetCamera(CamID.RAY);
                Player.instance.transform.position = playerSpawn.position;
                orb.transform.position = rayOrbSpawn.position;
                Vector3 forward = Player.instance.transform.forward;
                Vector3 position = Player.instance.transform.position;
                forward.x = orb.transform.position.x - position.x;
                forward.z = orb.transform.position.z - position.z;
                Player.instance.transform.forward = forward;
                if (orb.orbCharge > 2 && dt > stepDelay) { NextStep(); }
                break;
            case Steps.LIGHT_R_DONE:
                qState = true;
                playerMove = false;
                SetCamera(CamID.DONE);
                break;
            case Steps.ENLIGHTENED:
                qState = true;
                playerMove = false;
                break;
            case Steps.TERRIBLE_JOKE:
                qState = true;
                playerMove = false;
                text.fontSize = 50;
                break;
            default:
                Debug.Log("Default."); step = 0; break;
        }

        SetPressQState(qState);
        SetPlayerMove();

        Player.instance.health = Player.instance.maxHealth;

        prevStep = step;
	}

    void SetPressQState(bool isEnabled)
    {
        if(PressQ.gameObject.activeSelf != isEnabled)
            PressQ.gameObject.SetActive(isEnabled);
    }

    void SetPlayerMove()
    {
        if (playerMove == prevPlayerMove) return;
        if (playerMove) PlayerController.instance.AllowMovement();
        else PlayerController.instance.StopMovement();
        prevPlayerMove = playerMove;
    }

    void NextStep() { step++; dt = 0; }

    void SetCamera(CamID id)
    {
        if (currentID == id) return;
        for(int i = 0; i < cameras.Count; i++)
        {
            if(i == (int)id) { cameras[i].Priority = 20; }
            else { cameras[i].Priority = 1; }
        }
        currentID = id;
    }

    public void SetupTutorial()
    {
        step = 0; prevStep = step - 1;
        dt = 0;
        input = PlayerInput.instance;
        playerMove = false;
        prevPlayerMove = !playerMove;
        currentID = CamID.DONE;
    }

    void FinishTutorial()
    {
        Player.instance.transform.position = gameStartSpawn.position;
        IngameProgressScript.instance.PlayerNotEmptyAnymore();
        playerMove = true;
        SetPlayerMove();
        gameObject.SetActive(false);
    }

    public Transform getPlayerSpawn()
    {
        return playerSpawn;
    }
}
