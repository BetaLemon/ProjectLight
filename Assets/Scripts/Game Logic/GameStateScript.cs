using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//This script can keep track of stuff globally.


public class GameStateScript : MonoBehaviour {

    private enum SceneState { MAINMENU, OPTIONS, FILESELECT, INGAME };
    SceneState prevFrameState = SceneState.MAINMENU;
    SceneState state = SceneState.MAINMENU;

    public bool gamePaused;

    public GameObject FileSelectorSpawnRef;
    public GameObject PlayerRef;
    public GameObject MainMenuRef;
    public GameObject MainMenuCanvasRef;
    public GameObject OptionsRef;

    //WORLD SCENE START EVENTS
    void Start()
    {
        //Reference Initializations:
        PlayerRef = GameObject.FindGameObjectWithTag("Player");
        PlayerRef.SetActive(false);
        MainMenuCanvasRef = MainMenuRef.transform.GetChild(2).gameObject;
        FileSelectorSpawnRef = GameObject.Find("FileSelectorSpawn");

        //The state on the previous frame. Used for detecting state changes:
        prevFrameState = SceneState.MAINMENU;
        //First state considering the first thing we see on scene start is the menu:
        state = SceneState.MAINMENU;
    }

    void Update()
    {
        print("Current Scene State: " + state);

        prevFrameState = state; //Reference to current state for next frame
    }

    public void StateChanged(bool cameFromIngame)
    {
        if (state == SceneState.MAINMENU)
        {
            PauseGame(false);
            //Player gets sent to file selector
            PlayerRef.transform.position = FileSelectorSpawnRef.transform.position;
            //Player gets disabled
            PlayerRef.SetActive(false);

            OptionsRef.SetActive(false);
            MainMenuRef.SetActive(true);
            MainMenuCanvasRef.SetActive(true);
        }
        else if (state == SceneState.OPTIONS)
        {
            MainMenuCanvasRef.SetActive(false);
            OptionsRef.SetActive(true);
            if (prevFrameState == SceneState.INGAME) //Stuff that happens only if you come from the game
            {
                OptionsRef.transform.GetChild(2).gameObject.SetActive(true); //Activates the back to game button
            }
        }
        else if (state == SceneState.FILESELECT)
        {
            //Player activation and file selector coordinates:
            PlayerRef.SetActive(true);
        }
        else if (state == SceneState.INGAME)
        {
            OptionsRef.SetActive(false);
            PauseGame(false);
        }
    }

    public void SetSceneState(int theState)
    {
        state = (SceneState)theState;
        print("State Set To: " + state);

        if (prevFrameState != state) //Scene change checker
        {
            StateChanged(true);
            
        }
    }
    public int GetSceneState()
    {
        return (int)state;
    }
    public void PauseGame(bool to)
    {
        if (state == SceneState.INGAME) //You can only pause the game ingame
        {
            gamePaused = to;
            if (gamePaused)
            {
                Time.timeScale = 0;
                print("Game Paused");
            }
            else
            {
                Time.timeScale = 1;
                print("Game Unpaused");
            }
        }
    }

}
