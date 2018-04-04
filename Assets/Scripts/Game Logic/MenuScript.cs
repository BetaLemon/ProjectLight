using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuScript : MonoBehaviour {

    public GameStateScript gameStateDataScriptRef; //Reference to the Game/Global World Scene State

    void Start() //The following always happens when the main menu appears:
    {
        //Reference Initializations:
        gameStateDataScriptRef = GameObject.Find("GameState").GetComponent<GameStateScript>();
    }

    public void FileSelectionMode()
    {
        gameObject.SetActive(false);
        gameStateDataScriptRef.SetSceneState(2); //Informs the GameState that we are now heading to file selector
    }
    public void GoToOptions()
    {
        gameStateDataScriptRef.SetSceneState(1);
    }
    public void Quit()
    {
        Application.Quit();
    }
}
