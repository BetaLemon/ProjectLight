using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialController : MonoBehaviour {

    public TutorialScript ts;
    public static TutorialController instance;

    private void Awake()
    {
        if(instance == null) { instance = this; }
    }

    void Start()
    {
        if(ts == null) { Debug.Log("No TutorialScript was attached."); }
        else if (ts.gameObject.activeInHierarchy) { ts.gameObject.SetActive(false); }
    }

	// Update is called once per frame
	void Update () {
        if (Input.GetKey(KeyCode.M)) { StartTutorial(); }
    }

    public void StartTutorial()
    {
        if(ts == null) { Debug.LogError("Tutorial not started. Script missing."); }
        ts.gameObject.SetActive(true);
        ts.SetupTutorial();
        Player.instance.transform.position = ts.getPlayerSpawn().position;
    }
}
