using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {

    public enum GameMode { TITLE_SCREEN, SAVE_SELECT, GAME };
    public GameMode mode;

    private PlayerInput input;
    private CameraScript cameraScript;

	// Use this for initialization
	void Start () {
        input = FindObjectOfType<PlayerInput>();
        cameraScript = FindObjectOfType<CameraScript>();
	}
	
	// Update is called once per frame
	void Update () {
        switch (mode)
        {
            case GameMode.TITLE_SCREEN:
                TitleScreen();
                break;
            case GameMode.SAVE_SELECT:
                SaveSelect();
                break;
            case GameMode.GAME:
                Game();
                break;
        }
	}

    void TitleScreen()
    {
        //if (input.isPressed("Jump")) { mode = GameMode.SAVE_SELECT; }
        if (Input.anyKeyDown) { mode = GameMode.SAVE_SELECT; }
        cameraScript.setMode(CameraScript.CameraMode.GAME_START);
        cameraScript.GetComponentInChildren<Canvas>().gameObject.SetActive(true);
    }

    void SaveSelect()
    {
        //if (input.isPressed("Jump")) { mode = GameMode.GAME; }
        if (Input.GetKeyDown(KeyCode.Space)) { mode = GameMode.GAME; }
        cameraScript.setFocus(GameObject.Find("CameraFocus"));
        cameraScript.setMode(CameraScript.CameraMode.FOCUSED);
        cameraScript.GetComponentInChildren<Canvas>().gameObject.SetActive(false);
    }

    void Game()
    {
        if (Input.GetKeyDown(KeyCode.Escape)) { mode = GameMode.TITLE_SCREEN; }
        cameraScript.setMode(CameraScript.CameraMode.PLAYER);
    }
}
