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
        gameStateDataScriptRef.SetSceneState(GameStateScript.SceneState.FILESELECT); //Informs the GameState that we are now heading to file selector
    }
    public void GoToOptions()
    {
        gameStateDataScriptRef.SetSceneState(GameStateScript.SceneState.OPTIONS);
    }
    public void Quit()
    {
        Application.Quit();
    }
}
