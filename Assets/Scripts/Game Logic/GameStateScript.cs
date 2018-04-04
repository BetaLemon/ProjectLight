using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//This script can keep track of stuff globally.


public class GameStateScript : MonoBehaviour {

    private enum SceneState { MAINMENU, OPTIONS, FILESELECT, INGAME };
    SceneState state = SceneState.MAINMENU;

    public bool gamePaused;

    public GameObject PlayerRef;

    //WORLD SCENE START EVENTS
    void Start()
    {
        //Reference Initializations:
        PlayerRef = GameObject.FindGameObjectWithTag("Player");

        //First state considering the first thing we see on scene start is the menu:
        state = SceneState.MAINMENU;
    }

    void Update() //Simple state machine according to state
    {
        if (state == SceneState.MAINMENU)
        {
            //Player gets disabled
            PlayerRef.SetActive(false);
        }
        else if (state == SceneState.OPTIONS)
        {

        }
        else if (state == SceneState.FILESELECT)
        {
            //Player activation and file selector coordinates:
            PlayerRef.SetActive(true);
        }
        else if (state == SceneState.INGAME)
        {
            //Player activation and file selector coordinates:
            PlayerRef.SetActive(true);
        }

        if (gamePaused) Time.timeScale = 0; else Time.timeScale = 1;
    }

    public void SetSceneState(int theState)
    {
        state = (SceneState)theState;
    }
    public int GetSceneState()
    {
        return (int)state;
    }
    public void PauseGame(bool what)
    {
        gamePaused = what;
    }


}
