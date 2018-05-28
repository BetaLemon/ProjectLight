﻿using UnityEngine;

public class OptionsScript : MonoBehaviour {

    [FMODUnity.EventRef]
    public string clicksound;

    private GameStateScript gameStateDataScriptRef; //Reference to the Game/Global World Scene State

    void Start()
    {
        //Reference Initializations:
        gameStateDataScriptRef = GameStateScript.instance;
    }

    public void GoToMainMenu()
    {
        FMODUnity.RuntimeManager.PlayOneShot(clicksound);
        if (gameStateDataScriptRef.GetSceneSide() == true) //Came from ingame side
        {
            OptionsMode(true);
        }
        else
        {
            SendToMenu();
        }
    }
    public void BackToGame() //This button only appears if you just came from ingame and not from the main menu
    {
        FMODUnity.RuntimeManager.PlayOneShot(clicksound);
        gameStateDataScriptRef.SetSceneState(GameStateScript.SceneState.INGAME);
    }
    public void SendToMenu()
    {
        OptionsMode(false);
        gameStateDataScriptRef.SetSceneState(0);
    }
    public void OptionsMode(bool tf) //Activates(true)/Disactivates(false) back to menu confirmation so the player doesn't leave by mistake (Options divided in two states basically)
    {
            transform.GetChild(0).gameObject.SetActive(!tf); //Volume
            transform.GetChild(1).gameObject.SetActive(!tf); //Exit
            transform.GetChild(2).gameObject.SetActive(!tf); //BackToGame
            transform.GetChild(3).gameObject.SetActive(tf); //AreYouSureYouWantToLeave
            transform.GetChild(4).gameObject.SetActive(tf); //YES
            transform.GetChild(5).gameObject.SetActive(tf); //NO
    }

}
