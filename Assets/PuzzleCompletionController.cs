using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/* This scripts checks if obligatory elements were triggered.
 * REQUIREMENTS:
 *  - Puzzle Object containing all elements of the puzzle needs to have this script.
 *  - All triggers need to have this parent GameObject ("Puzzle X" for example) as triggeredObject.
 */

public class PuzzleCompletionController : MonoBehaviour {

    public enum PuzzleState { TRIED, UNTOUCHED, COMPLETED };

    #region Variables
    private PuzzleState state;
    private int completedTriggers;
    private int neededTriggers;
    #endregion

    // Use this for initialization
    void Start () {
        state = PuzzleState.UNTOUCHED;
        completedTriggers = 0;

        // We need to get all triggers necessary for completing the level. They will have ourselves as triggeredObject.
        Trigger[] triggers = GetComponentsInChildren<Trigger>();
        neededTriggers = 0;
        foreach(Trigger trigger in triggers)
        {
            if (trigger.HasPuzzleCompletionTrigger()) { neededTriggers++; }
        }
	}
	
	// Update is called once per frame
	void Update () {
		if(completedTriggers >= neededTriggers && state != PuzzleState.COMPLETED) { state = PuzzleState.COMPLETED; }
	}

    public void getTriggered() { completedTriggers++; }

    public PuzzleState getState() { return state; }
}
