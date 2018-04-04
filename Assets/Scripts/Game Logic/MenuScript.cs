using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuScript : MonoBehaviour {

    public GameStateScript gameStateDataScriptRef; //Reference to the Game/Global World Scene State
    public GameObject OptionsRef; //Options menu object reference

    void Start() //The following always happens when the main menu appears:
    {
        //Reference Initializations:
        gameStateDataScriptRef = GameObject.FindGameObjectWithTag("GameState").GetComponent<GameStateScript>();
    }

    public void ActiveSelf(bool what)
    {
        gameObject.SetActive(what);
    }
    public void FileSelectionMode()
    {
        gameStateDataScriptRef.SetSceneState(3); //Informs the GameState that we are now heading to file selector

    }
    public void GoToOptions()
    {

        gameObject.transform.GetChild(2).gameObject.SetActive(false);
        OptionsRef.SetActive(true);
    }
    public void Quit()
    {
        Application.Quit();
    }
}
