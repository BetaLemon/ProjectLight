using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour {

    // Player Health system:
    public float health; //Current mana/health of the player. Magic and health are the same. If all health is lost, the player dies
    public float maxHealth = 100f; //Maximum playerHealth the player can reach
    private float minHealth = 0f; //Health at which the player dies
    public float respawnHealth = 15; //Health with which the player respawns after death

    //Player Economy:
    public int gemstones = 0;
    public int smallGemstones = 0;
    public Text LargeGemstones;
    public Text SmallGemstones;

    //FallDamage control:
    private float lastPositionY = 0f;
    private float fallDistance = 0f;
    //public float fallDamageStartDistance = 5f;
    public float fallDamageStartVelocity = -15f; //Velocity from which fall damage starts to occur if on contact with floor and high enough MoveDirection.y
    private float prevAxisMoveDir; //Previous MoveDirection.y value for variance checking

    private CharacterController controllerRef; //Own character controller reference
    private PlayerController playerControllerRef; //Own player controller script reference

    //Area control:
    [Tooltip("Sets where the player would respawn if not assigned to any area")]
    public Vector3 baseWorldSpawn = new Vector3(0, 0, 0);
    private GameObject currentArea = null; //Last area where the player has entered

	void Start () {
        controllerRef = FindObjectOfType<CharacterController>();
        playerControllerRef = GetComponent<PlayerController>();
    }
	
	void Update () {

        //Fall damage with MoveDirection.y from PlayerController.cs as accounting for fall damage
       // print(playerControllerRef.getYAxisMoveDir());

        /*

        if (prevAxisMoveDir < fallDamageStartVelocity && playerControllerRef.getYAxisMoveDir() == 0 && controllerRef.isGrounded)
        {
            health -= 100;
        }

        prevAxisMoveDir = playerControllerRef.getYAxisMoveDir();
        */

        //Old fall damage system through: HeightControl Also causes player health to deplete below y0 if on ground It's kind of a bug, 
        /*if (lastPositionY > transform.position.y) //Sums the descending altitude variation to the fall distance
        {
            fallDistance += lastPositionY - transform.position.y;
        }

        lastPositionY = transform.position.y; //Update last position as current to account for next update iteration

        if (fallDistance >= fallDamageStartDistance && controllerRef.isGrounded) //Damage if we fell from high enough, according to further height
        {
            health -= 40 + fallDistance*10; //The higher the altitude, the higher the damage
            ApplyNormal();
        }

        if (fallDistance <= fallDamageStartDistance && controllerRef.isGrounded) //Resets calculation values because we touched the floor and the distance wasn't enough for damage
        {
            ApplyNormal();
        }*/

        //Health limiters:
        if (health > maxHealth) { health = maxHealth; }
        if (health < minHealth) { health = minHealth; Die(); }
	}

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.CompareTag("Area")) { currentArea = other.gameObject; } //Player enters area, we save the area
        else if (other.gameObject.CompareTag("BlackInsect")) { health -= other.gameObject.GetComponent<BlackInsect>().getDamageDealt() * Time.deltaTime; } //Black insect's dark areas damage the player
        else if (other.gameObject.CompareTag("Lethal")) { Die(); } //Player enters lethal area, dies and respawns
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("SmallGemstone"))
        {
            Destroy(other.gameObject); smallGemstones += 1;
            SmallGemstones.text = "SmallGemstones x" + smallGemstones.ToString(); //Update GUI
        }
        else if (other.gameObject.CompareTag("Gemstone")) {
            Destroy(other.gameObject); gemstones += 1;
            LargeGemstones.text = "LargeGemstones x" + gemstones.ToString(); //Update GUI
        }
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

    public void fallDamage(float damage) {
        health -= damage;
    }
}
