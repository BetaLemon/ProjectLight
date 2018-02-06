using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour {

    // Player Health system:
    public float health; //Current mana/health of the player. Magic and health are the same. If all health is lost, the player dies
    public float maxHealth = 100f; //Maximum playerHealth the player can reach
    private float minHealth = 0f; //Health at which the player dies

    public GameObject lifeBar;
    private GameObject defaultLifeBar;

	// Use this for initialization
	void Start () {
        defaultLifeBar = lifeBar;
	}
	
	// Update is called once per frame
	void Update () {
		if(health > maxHealth) { health = maxHealth; }
        if(health < minHealth) { health = minHealth; }
        //Life bar:
        lifeBar.transform.localScale = new Vector3(health * defaultLifeBar.transform.localScale.x / maxHealth, defaultLifeBar.transform.localScale.y, defaultLifeBar.transform.localScale.z);
	}
}
