using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OptionsScript : MonoBehaviour {

    public GameObject MainMenuRef;
    public GameObject MainMenuCanvasRef;

    public void GoToMainMenu()
    {
        gameObject.SetActive(false);
        MainMenuRef.SetActive(true);
        MainMenuCanvasRef.SetActive(true);
    }
}
