using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OptionsScript : MonoBehaviour {

    public GameStateScript gameStateDataScriptRef; //Reference to the Game/Global World Scene State
    public GameObject MainMenuRef;
    public GameObject MainMenuCanvasRef;

    void Start()
    {
        //Reference Initializations:
        gameStateDataScriptRef = GameObject.FindGameObjectWithTag("GameState").GetComponent<GameStateScript>();
    }

    public void GoToMainMenu()
    {
        gameObject.SetActive(false);
        MainMenuRef.SetActive(true);
        MainMenuCanvasRef.SetActive(true);
    }
}
