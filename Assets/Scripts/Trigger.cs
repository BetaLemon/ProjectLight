using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trigger : MonoBehaviour {

    public GameObject[] triggeredObjects;

	// Use this for initialization
	void Start () {

	}
	
	// Update is called once per frame
	void Update () {

	}

    public void pleaseTrigger()
    {
        for(int i = 0; i < triggeredObjects.Length; i++)
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
