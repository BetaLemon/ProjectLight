﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trigger : MonoBehaviour {

    public GameObject[] triggeredObjects;

    public int triggerCount = 0; //Times the trigger has been triggered
    public int maxTriggers = 1; //Maximum ammount of times it can be triggered

    public float triggerDelay; //Delay before next trigger usage
    private float timer = 0; //Time since the trigger was last triggered
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

    public void pleaseTrigger()
    {
        if (triggerCount < maxTriggers && canBeTriggered) { //Trigger use limiter
            triggerCount++;
            canBeTriggered = false;
            for (int i = 0; i < triggeredObjects.Length; i++)
            {
                switch (triggeredObjects[i].tag)
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
