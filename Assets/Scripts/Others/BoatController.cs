using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoatController : MonoBehaviour {

    public struct BoatData
    {
        public string areaName;
        public int completedPuzzles;
        public int day;
        public int month;
        public int year;
        public int hour;
        public int minutes;
        public bool isEmpty;
        public BoatData(string nam, int puz, int d, int m, int y, int h, int min, bool empty)
        {
            areaName = nam; completedPuzzles = puz; day = d; month = m; year = y; hour = h; minutes = min; isEmpty = empty;
        }
    }

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

    [Header("Text fields:")]
    public TextMesh areaText;
    public TextMesh puzzleText;
    public TextMesh dateText;

    public int totalPuzzleCount = 5;
    private BoatData[,] boatData = new BoatData[2, 2];

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

        LoadPlayerData();
    }
	
	// Update is called once per frame
	void Update () {
        if (stateScript.GetSceneState() != GameStateScript.SceneState.FILESELECT) { DisableBoats(); return; }
        else if(boatUI.gameObject.activeInHierarchy == false) { if(!active) EnableBoats(); }
                
        ProcessInput(keyboardInputAxis);
        //ProcessInput(gamepadInputAxis);
        LimitBoatIndex();
        UpdateBoats();
        SetUIPosition();

        LoadPlayerData();
        //if (prevBoat != currentBoat) { }

        //BoatData bd = new BoatData("Here comes the area name.", 4, 15, 5, 2018, 15, 25, false);
        //BoatData bd1 = new BoatData("Kill me, plz.", 0, 15, 5, 2019, 09, 24, false);
        //BoatData bd2 = new BoatData("Dylan, Fear the Dark.", 5, 15, 2, 2016, 12, 35, false);
        //BoatData bd3 = new BoatData("Mom's spaghetti!", 3, 6, 1, 2017, 14, 26, false);
        
        UpdateBoatText();

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
        LoadPlayerData();
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

    void UpdateBoatText()
    {
        BoatData bd = boatData[currentBoat[0], currentBoat[1]];

        if (bd.isEmpty)
        {
            areaText.text = "";
            puzzleText.text = "Empty File";
            dateText.text = "";
        }
        else
        {
            areaText.text = bd.areaName;
            puzzleText.text = "Completed " + bd.completedPuzzles + " / " + totalPuzzleCount + " puzzles.";
            dateText.text = bd.day + " / " + bd.month + " / " + bd.year + " - " + bd.hour + ":" + bd.minutes;
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
        if (input.isPressed(axis)) { LoadBoat(currentBoat[0], currentBoat[1]); }
        if(Input.GetKeyDown(KeyCode.F12)) { IngameProgressScript.instance.DeletePlayer(GetPlayerIndex(currentBoat[0], currentBoat[1])); LoadPlayerData(); }
    }

    void SetUIPosition()
    {
        Vector3 newPos = boats[currentBoat[0], currentBoat[1]].position;
        newPos.y += verticalOffset;

        newPos += Vector3.Normalize(cam.transform.position - newPos)*cameraProximity;
        newPos += cam.transform.right * horizontalOffset;

        boatUI.transform.position = newPos;
    }

    void LoadBoat(int x, int y)
    {
        DisableBoats();
        stateScript.SetSceneState(GameStateScript.SceneState.INGAME);

        IngameProgressScript.instance.SetPlayer(GetPlayerIndex(x, y));

        //Player.instance.transform.position = BaseWorldSpawn.transform.position;
        active = false;
    }

    void LoadPlayerData()
    {
        boatData[0, 0] = IngameProgressScript.instance.GetBoatData(0);
        boatData[1, 0] = IngameProgressScript.instance.GetBoatData(1);
        boatData[1, 1] = IngameProgressScript.instance.GetBoatData(2);
        boatData[0, 1] = IngameProgressScript.instance.GetBoatData(3);
    }

    int GetPlayerIndex(int x, int y)
    {
        if (x == 0 && y == 0) { return 0; }
        else if (x == 1 && y == 0) { return 1; }
        else if (x == 1 && y == 1) { return 2; }
        else if (x == 0 && y == 1) { return 3; }
        else return -1;
    }
}
