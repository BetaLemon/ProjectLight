using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trigger : MonoBehaviour {

    public enum TriggerType { ON_CHARGE, ON_HIT };

    // Script that allows objects to react to the player's lightshaft. This allows it to trigger things, thus the name.

    public GameObject[] triggeredObjects;   // Objects that will be triggered.
    public TriggerType type;
    [Range(0, 10)]
    public float triggerCharge;

    private int triggerCount = 0;    //Times the trigger has been triggered
    public int maxTriggers = 1;     //Maximum ammount of times it can be triggered

    private float triggerDelay;      //Delay before next trigger usage
    private float timer = 0;        //Time since the trigger was last triggered
    private bool canBeTriggered = true; //If timeSinceLastTrigger surpasses triggerDelay, this is set to true

    void Start () {
        triggerDelay = Time.deltaTime * 120; //Setting the trigger delay between triggers on start, since unity recommends this.
    }
	
	void Update () {

        //Time control for the trigger delays:
        if (canBeTriggered == false)
        {
            timer += Time.deltaTime;
        }
        if (triggerDelay < timer)
        {
            timer = 0;
            canBeTriggered = true; //Goes back to being triggerable if the timer reaches it's delay threashold
        }
	}

    // Function that is called when the player hits the trigger. The player asks the Trigger object to pleaseTrigger():
    public void pleaseTrigger()
    {
        if (triggerCount < maxTriggers && canBeTriggered) { //Trigger use limiter
            triggerCount++;
            canBeTriggered = false;
            for (int i = 0; i < triggeredObjects.Length; i++)   // For all the objects in the array that need to be triggered:
            {
                switch (triggeredObjects[i].tag)    // For the type of object that is triggered, we have each of the actions to be done:
                {
                    case "MovingPlatform":
                        MovingPlatform platform = triggeredObjects[i].GetComponent<MovingPlatform>();
                        platform.getTriggered();
                        break;
                    case "Door":
                        Door door = triggeredObjects[i].GetComponent<Door>();
                        door.getTriggered();
                        break;
                }
            }
        }
    }

    public void pleaseTrigger(float currentCharge)
    {
        if (triggerCount < maxTriggers && canBeTriggered)
        { //Trigger use limiter
            for (int i = 0; i < triggeredObjects.Length; i++)   // For all the objects in the array that need to be triggered:
            {
                if(currentCharge > triggerCharge)
                {
                    triggerCount++;
                    canBeTriggered = false;

                    switch (triggeredObjects[i].tag)    // For the type of object that is triggered, we have each of the actions to be done:
                    {
                        case "MovingPlatform":
                            MovingPlatform platform = triggeredObjects[i].GetComponent<MovingPlatform>();
                            platform.getTriggered();
                            break;
                        case "Door":
                            Door door = triggeredObjects[i].GetComponent<Door>();
                            door.getTriggered();
                            break;
                    }
                }
            }
        }
    }
}
