﻿using UnityEngine;

public class MenuScript : MonoBehaviour {

    [FMODUnity.EventRef]
    public string clicksound;

    private GameStateScript gameStateDataScriptRef; //Reference to the Game/Global World Scene State

    void Start() //The following always happens when the main menu appears:
    {
        //Reference Initializations:
        gameStateDataScriptRef = GameStateScript.instance;
    }

    public void FileSelectionMode()
    {
        FMODUnity.RuntimeManager.PlayOneShot(clicksound);

        gameStateDataScriptRef.SetSceneState(GameStateScript.SceneState.FILESELECT); //Informs the GameState that we are now heading to file selector
    }
    public void GoToOptions()
    {
        FMODUnity.RuntimeManager.PlayOneShot(clicksound);
        gameStateDataScriptRef.SetSceneState(GameStateScript.SceneState.OPTIONS);
    }
    public void Quit()
    {
        FMODUnity.RuntimeManager.PlayOneShot(clicksound);
        Application.Quit();
    }
}
