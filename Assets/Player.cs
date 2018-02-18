using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour {

    // Player Health system:
    public float health; //Current mana/health of the player. Magic and health are the same. If all health is lost, the player dies
    public float maxHealth = 100f; //Maximum playerHealth the player can reach
    private float minHealth = 0f; //Health at which the player dies
    public float respawnHealth = 15; //Health with which the player respawns after death

    //Player Economy:
    public int gemstones = 0;
    public int smallGemstones = 0;

    //FallDamage control:
    private float lastPositionY = 0f;
    private float fallDistance = 0f;
    public float fallDamageDistance = 8f;

    private CharacterController controllerRef; //Own character controller

    //Area control:
    [Tooltip("Sets where the player would respawn if not assigned to any area")]
    public Vector3 baseWorldSpawn = new Vector3(0, 0, 0);
    private GameObject currentArea = null; //Last area where the player has entered

	void Start () {
        controllerRef = FindObjectOfType<CharacterController>();
    }
	
	void Update () {

        //Fall damage system through: HeightControl Also causes player health to deplete below y0 It's kind of a bug, 
        //but it's actually usefull lol. Alternative: Use MoveDirection.y from PlayerController.cs as a velocity variation accounting for fall damage)

        if (lastPositionY > transform.position.y) //Sums the descending altitude variation to the fall distance
        {
            fallDistance += lastPositionY - transform.position.y;
        }

        lastPositionY = transform.position.y; //Update last position as current to account for next update iteration

        if (fallDistance >= fallDamageDistance && controllerRef.isGrounded) //Damage if we fell from high enough
        {
            health -= 5;
            ApplyNormal();
        }

        if (fallDistance <= fallDamageDistance && controllerRef.isGrounded) //Resets calculation values because we touched the floor and the distance wasn't enough for damage
        {
            ApplyNormal();
        }

        //Health limiters:
        if (health > maxHealth) { health = maxHealth; }
        if (health < minHealth) { health = minHealth; Die(); }
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
            transform.position = baseWorldSpawn; //Send to central world spawn
        }
        //Reset Health:
        health = respawnHealth;
    }

    private void ApplyNormal()
    {
        fallDistance = 0;
        lastPositionY = 0;
    }
}
