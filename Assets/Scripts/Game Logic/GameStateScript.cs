using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//This script can keep track of stuff globally.

public class GameStateScript : MonoBehaviour {
    private int sceneState; //1=MAIN MENU, 2=OPTIONS, 3=FILE SELECT, 4=INGAME
	void Start () {
        sceneState = 1;
    }

    //void Update () {

    //}

    public void setSceneState(int state)
    {
        sceneState = state;
    }
    public int getSceneState()
    {
        return sceneState;
    }


}
