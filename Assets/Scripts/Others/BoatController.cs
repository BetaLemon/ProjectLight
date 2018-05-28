using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoatController : MonoBehaviour {

    #region Variables
    [Header("Input Axis, Horizontal then Vertical:")]
    public string[] keyboardInputAxis = new string[2];
    public string[] gamepadInputAxis = new string[2];
    public string keyboardSubmitKey;
    public string gamepadSubmitKey;

    public Transform boatUI;

    [Header("Boats:")]
    public Transform upleft;
    public Transform upright;
    public Transform downleft;
    public Transform downright;

    [Header("Others:")]
    public Transform BaseWorldSpawn;
    //private Dictionary<Vector2, Transform> boats;
    public Transform[,] boats = new Transform[2,2];
    public float verticalOffset = 4.0f;
    public float horizontalOffset = -6.0f;
    public float cameraProximity = 2f;

    private int[] currentBoat = new int[2];
    private int[] prevBoat = new int[2];
    private PlayerInput input;
    private GameStateScript stateScript;
    private Camera cam;
    private bool active;
    #endregion

    // Use this for initialization
    void Start() {
        if (boatUI == null) { boatUI = GetComponentInChildren<Canvas>().transform; Debug.Log("BoatUI fetched forcedly."); }
        currentBoat[0] = 0; currentBoat[1] = 0;
        prevBoat = currentBoat; prevBoat[0] = 0;
        input = PlayerInput.instance;
        stateScript = GameStateScript.instance;
        cam = Camera.main;
        // Boats:
        boats[0, 0] = upleft;
        boats[1, 0] = upright;
        boats[1, 1] = downright;
        boats[0, 1] = downleft;
    }
	
	// Update is called once per frame
	void Update () {
        if (stateScript.GetSceneState() != GameStateScript.SceneState.FILESELECT) { DisableBoats(); return; }
        else if(boatUI.gameObject.activeInHierarchy == false) { if(!active) EnableBoats(); }
        //if (prevBoat == currentBoat) return;
        
        ProcessInput(keyboardInputAxis);
        //ProcessInput(gamepadInputAxis);
        LimitBoatIndex();
        UpdateBoats();
        SetUIPosition();

        CheckSubmit(keyboardSubmitKey);

        prevBoat = currentBoat;
	}

    void DisableBoats()
    {
        boatUI.gameObject.SetActive(false);
        upleft.GetComponentInChildren<cakeslice.Outline>().enabled = false;
        upright.GetComponentInChildren<cakeslice.Outline>().enabled = false;
        downright.GetComponentInChildren<cakeslice.Outline>().enabled = false;
        downleft.GetComponentInChildren<cakeslice.Outline>().enabled = false;
        active = false;
    }

    void EnableBoats()
    {
        boatUI.gameObject.SetActive(true);
        upleft.GetComponentInChildren<cakeslice.Outline>().enabled = true;
        upright.GetComponentInChildren<cakeslice.Outline>().enabled = true;
        downright.GetComponentInChildren<cakeslice.Outline>().enabled = true;
        downleft.GetComponentInChildren<cakeslice.Outline>().enabled = true;
        active = true;
        currentBoat[0] = 0; currentBoat[1] = 0;
        prevBoat = currentBoat; prevBoat[0] = 0;
    }

    void UpdateBoats()
    {
        for(int i = 0; i <= 1; i++)
        {
            for (int j = 0; j <= 1; j++)
            {
                if(currentBoat[0] == i && currentBoat[1] == j) { boats[i,j].GetComponentInChildren<cakeslice.Outline>().enabled = true; }
                else { boats[i,j].GetComponentInChildren<cakeslice.Outline>().enabled = false; }
            }
        }
    }

    void LimitBoatIndex() {
        if (currentBoat[0] > 1) currentBoat[0] = 1;
        if (currentBoat[1] > 1) currentBoat[1] = 1;
        if (currentBoat[0] < 0) currentBoat[0] = 0;
        if (currentBoat[1] < 0) currentBoat[1] = 0;
    }

    void ProcessInput(string[] axis)
    {
        //currentBoat[0] += (int)input.getInput(axis[0]);
        //currentBoat[1] += (int)input.getInput(axis[1]);
        if (input.getInput(axis[0]) > 0) currentBoat[0]++;
        if (input.getInput(axis[0]) < 0) currentBoat[0]--;
        if (input.getInput(axis[1]) > 0) currentBoat[1]--;
        if (input.getInput(axis[1]) < 0) currentBoat[1]++;
    }

    void CheckSubmit(string axis)
    {
        if (input.isPressed(axis)) { LoadBoat(); }
    }

    void SetUIPosition()
    {
        Vector3 newPos = boats[currentBoat[0], currentBoat[1]].position;
        newPos.y += verticalOffset;

        newPos += Vector3.Normalize(cam.transform.position - newPos)*cameraProximity;
        newPos += cam.transform.right * horizontalOffset;

        boatUI.transform.position = newPos;
    }

    void LoadBoat()
    {
        DisableBoats();
        stateScript.SetSceneState(GameStateScript.SceneState.INGAME);
        Player.instance.transform.position = BaseWorldSpawn.transform.position;
        active = false;
    }
}
