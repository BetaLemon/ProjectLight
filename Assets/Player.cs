using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour {

    // Player Health:
    public int life;
    public int maxLife = 100;
    private int minLife = 0;

    public GameObject lifeBar;
    private GameObject defaultLifeBar;

	// Use this for initialization
	void Start () {
        defaultLifeBar = lifeBar;
	}
	
	// Update is called once per frame
	void Update () {
		if(life > maxLife) { life = maxLife; }
        if(life < minLife) { life = minLife; }
        //Life bar:
        lifeBar.transform.localScale = new Vector3(life * defaultLifeBar.transform.localScale.x / maxLife,defaultLifeBar.transform.localScale.y, defaultLifeBar.transform.localScale.z);
	}
}
