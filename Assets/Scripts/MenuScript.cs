using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuScript : MonoBehaviour {

    public GameObject OptionsRef;
    public GameObject PlayerRef;

    void Start() //The following always happens when the main menu appears:
    {
        //Reference Initializations:
        PlayerRef = GameObject.FindGameObjectWithTag("Player");

        //Player gets disabled
        PlayerRef.SetActive(false);
    }

    public void FileSelectionMode()
    {
        gameObject.SetActive(false);
        //Player activation and file selector coordinates:
        PlayerRef.SetActive(true);
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
