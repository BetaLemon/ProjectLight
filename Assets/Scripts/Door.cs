using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour {

    public GameObject LeftHinge;
    public GameObject RightHinge;

    private Animator DoorAnimation;

    public bool openTheDoor;
    public bool closeTheDoor;
    private bool doorOpen;

    //public float doorSpeed = 0.1f; //Speed at which the door opens and closes

    void Start () {
        DoorAnimation = GetComponent<Animator>();
    }
	
	// Update is called once per frame
	void Update () {


        if (openTheDoor)
        {
            DoorAnimation.SetBool("Close", false);
            DoorAnimation.SetBool("Open", true);
        }
        else if (closeTheDoor)
        {
            DoorAnimation.SetBool("Close", true);
            DoorAnimation.SetBool("Open", false);
        }
    }

    public void getTriggered() { if (doorOpen) { closeTheDoor = true; openTheDoor = false; }
                                 else { openTheDoor = true; closeTheDoor = false; }
    }
}
