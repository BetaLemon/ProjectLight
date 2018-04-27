using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TempCanvasScrip : MonoBehaviour {

    public bool gameRunning;
    public GameObject player;

	// Use this for initialization
	void Start () {
        gameRunning = false;
	}
	
	// Update is called once per frame
	void Update () {

        if(Input.GetAxis("Jump") != 0)
        {
            gameRunning = true;
        }

        if (gameRunning)
        {
            //Time.timeScale = 1;
            player.SetActive(true);
            GetComponentInParent<Canvas>().gameObject.SetActive(false);
        }
        else
        {
            //Time.timeScale = 0;
            player.SetActive(false);
        }
	}
}
