using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour {

    // Player Health system:
    public float health; //Current mana/health of the player. Magic and health are the same. If all health is lost, the player dies
    public float maxHealth = 100f; //Maximum playerHealth the player can reach
    private float minHealth = 0f; //Health at which the player dies
    public float respawnHealth = 15; //Health with which the player respawns after death

    public int gemstones = 0;
    public int smallGemstones = 0;

    private GameObject currentArea = null; //La ultima area en la que el jugador ha entrado

	// Use this for initialization
	void Start () {

	}
	
	// Update is called once per frame
	void Update () {
        //Health limits:
		if(health > maxHealth) { health = maxHealth; }
        if(health < minHealth) { health = minHealth; Die(); }
	}

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.CompareTag("Area")) { currentArea = other.gameObject; } //Player enters area, we save the area
        else if (other.gameObject.CompareTag("BlackInsect")) { health -= 1 * Time.deltaTime; } //Black insect's dark areas damage the player
        else if (other.gameObject.CompareTag("Lethal")) { Die(); } //Player enters lethal area, dies and respawns
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("SmallGemstone")) { Destroy(other.gameObject); smallGemstones += 1; }
        else if (other.gameObject.CompareTag("Gemstone")) { Destroy(other.gameObject); gemstones += 1; }
        else if (other.gameObject.CompareTag("ManaCharge")) { Destroy(other.gameObject); health += 5; }
    }
    

    private void Die() {
        print("Player died");
        //Send to spawn point:
        if (currentArea != null) {
            //transform.position = currentArea.GetComponentInChildren<GameObject>().transform.position;
            transform.position = currentArea.transform.GetChild(0).transform.position;
        }
        else {
            transform.position = new Vector3(0,0,0); //Base Spawn
        }
        //Reset Health:
        health = respawnHealth;
    }
}
