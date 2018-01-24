using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour {

    public GameObject LeftHinge;
    public GameObject RightHinge;

    private Animator DoorAnimation;

    public bool doorOpen;

    //public float doorSpeed = 0.1f; //Speed at which the door opens and closes

    void Start () {
        DoorAnimation = GetComponent<Animator>();
    }
	
	// Update is called once per frame
	void Update () {


        if (doorOpen)
        {
            DoorAnimation.SetBool("Close", false);
            DoorAnimation.SetBool("Open", true);
        }
        else if (!doorOpen)
        {
            DoorAnimation.SetBool("Close", true);
            DoorAnimation.SetBool("Open", false);
        }
    }

    public void getTriggered() { if (doorOpen) { doorOpen = false; }
                                 else { doorOpen = true; }
    }
}
