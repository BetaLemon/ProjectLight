using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trigger : MonoBehaviour {

    public enum TriggerType { ON_CHARGE, ON_HIT, ON_COLOUR };

    // Script that allows objects to react to the player's lightshaft. This allows it to trigger things, thus the name.

    #region Variables
    public GameObject[] triggeredObjects;       // Objects that will be triggered.
    public TriggerType type;
    [Range(0, 10)]

    private int triggerCount = 0;               // Times the trigger has been triggered
    [Tooltip("-1 for infinite")]
    public int maxTriggers = -1;                // Maximum ammount of times it can be triggered

    private float triggerDelay;                 // Delay before next trigger usage
    private float timer = 0;                    // Time since the trigger was last triggered
    private bool canBeTriggered = true;         // If timeSinceLastTrigger surpasses triggerDelay, this is set to true

    public Color triggerColor = Color.white;    // The color the orb should contain for it to be triggered
    public float triggerChargeThreashold;                 // Charge threashold number for a trigger to occur

    private bool previousSegmentHigh = false;   // Was the charge higher than threashold in the previous frame?
    private bool currentSegmentHigh = false;    // Is the charge over the trigger or under it?
    #endregion

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

    void TriggerAllObjects()
    {
        if (triggeredObjects.Length == 0) return;
        if (triggeredObjects[0] == null) return;
        //sDebug.Log("Triggered All Objects!");
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

            PuzzleCompletionController puzCon = triggeredObjects[i].GetComponent<PuzzleCompletionController>();
            if (puzCon != null && puzCon.getState() != PuzzleCompletionController.PuzzleState.COMPLETED)
            {
                puzCon.getTriggered();
            }
        }
    }

    public void pleaseTrigger() //Direct trigger, no checks envolved except base trigger restrictions
    {
        if (triggerCount < maxTriggers && canBeTriggered || maxTriggers == -1 && canBeTriggered)
        { //Trigger use limiter
            triggerCount++;
            canBeTriggered = false;
            TriggerAllObjects();
        }
    }

    public void pleaseTrigger(float currentCharge) //Tigger with charge checking
    {
        if (currentSegmentHigh && previousSegmentHigh == false || currentSegmentHigh == false && previousSegmentHigh) //If we go up or down the threashold
        {
            if (triggerCount < maxTriggers && canBeTriggered || maxTriggers == -1 && canBeTriggered)
            { //Trigger use limiter
                triggerCount++;
                canBeTriggered = false;
                TriggerAllObjects();
            }
        }
        if (currentCharge >= triggerChargeThreashold)
        {
            currentSegmentHigh = true;
            previousSegmentHigh = true;
        }
        else if (currentCharge < triggerChargeThreashold)
        {
            currentSegmentHigh = false;
            previousSegmentHigh = false;
        }
    }

    public void pleaseTrigger(float currentCharge, Color color) //Trigger with charge and color checking
    {
        if (currentCharge >= triggerChargeThreashold)
        {
            currentSegmentHigh = true;
            previousSegmentHigh = true;
        }
        else if (currentCharge < triggerChargeThreashold)
        {
            currentSegmentHigh = false;
            previousSegmentHigh = false;
        }

        if (currentSegmentHigh && previousSegmentHigh == false || currentSegmentHigh == false && previousSegmentHigh) //If we go up or down the threashold
        {
            if (color == triggerColor && currentCharge > triggerChargeThreashold && canBeTriggered && triggerCount < maxTriggers || color == triggerColor && currentCharge > triggerChargeThreashold && canBeTriggered && maxTriggers == -1)
            {
                triggerCount++;
                canBeTriggered = false;
                TriggerAllObjects();
            }
    }

    public bool HasPuzzleCompletionTrigger()
    {
        bool has = false;
        foreach(GameObject obj in triggeredObjects)
        {
            if(obj.GetComponent<PuzzleCompletionController>() != null) { has = true; }
        }
        return has;
    }
}
