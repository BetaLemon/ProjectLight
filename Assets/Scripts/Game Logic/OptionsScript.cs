using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OptionsScript : MonoBehaviour {

    public GameStateScript gameStateDataScriptRef; //Reference to the Game/Global World Scene State

    void Start()
    {
        //Reference Initializations:
        gameStateDataScriptRef = GameObject.Find("GameState").GetComponent<GameStateScript>();
    }

    public void GoToMainMenu()
    {
        gameStateDataScriptRef.SetSceneState(0);
    }

    public void BackToGame() //This button only appears if you just came from ingame and not from the main menu
    {
        gameStateDataScriptRef.SetSceneState(3);
    }
}
