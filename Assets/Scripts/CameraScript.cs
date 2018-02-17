using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraScript : MonoBehaviour {

    // This script controls camera movement

    enum CameraMode { GAME_START, FOCUSED, PLAYER };
    private CameraMode mode;
    private GameObject focused;

    public GameObject player;   // Stores the player, to use his position later on.
    private Vector3 offset;     // Vector that stores the offset between the player and the camera.
    public float speed = 4.0f;  // Speed at which the camera lerps.
    
    public GameObject[] panNodes;
    public GameObject lookAtPoint;
    private int currentNode;
    public float tolerance; //needs to be private.

	// Use this for initialization
	void Start () {
        offset = transform.position - player.transform.position;    // The offset is read at start. Might be changed at some point in time.
        if (panNodes.Length == 0 || lookAtPoint == null) { mode = CameraMode.PLAYER; print("No nodes for camera are set, and/or no look at point set."); }
        else { mode = CameraMode.GAME_START; }
        focused = null;

        currentNode = 0;

        if(mode == CameraMode.GAME_START) { transform.position = panNodes[currentNode].transform.position; }
	}

    // LateUpdate is called after Update each frame
    void FixedUpdate () {

        //if(focused == null) { mode = CameraMode.PLAYER; }

        switch (mode)
        {
            case CameraMode.GAME_START:
                GameStartCamera();
                break;
            case CameraMode.PLAYER:
                PlayerCamera();
                break;
            case CameraMode.FOCUSED:
                FocusedCamera();
                break;
        }
    }

    void PlayerCamera()
    {
        float interpolation = speed * Time.deltaTime;   // Calculates the interpolation for a smooth lerp.

        Vector3 position = transform.position;  // stores the camera's position.
                                                // We change the camera's coordinates following the player's position and applying interpolation:
        position.y = Mathf.Lerp(transform.position.y, player.transform.position.y + offset.y, interpolation);
        position.x = Mathf.Lerp(transform.position.x, player.transform.position.x + offset.x, interpolation);
        position.z = Mathf.Lerp(transform.position.z, player.transform.position.z + offset.z, interpolation);

        transform.position = position; // The position of the camera is finally modified.

        /*
        // Set the position of the camera's transform to be the same as the player's, but offset by the calculated offset distance.
        transform.position = player.transform.position + offset;*/
    }

    void FocusedCamera()
    {
        float interpolation = speed * Time.deltaTime;   // Calculates the interpolation for a smooth lerp.

        Vector3 position = transform.position;  // stores the camera's position.
                                                // We change the camera's coordinates following the player's position and applying interpolation:
        position.y = Mathf.Lerp(transform.position.y, focused.transform.position.y + 3, interpolation);
        position.x = Mathf.Lerp(transform.position.x, focused.transform.position.x, interpolation);
        position.z = Mathf.Lerp(transform.position.z, focused.transform.position.z - 3, interpolation);

        transform.position = position; // The position of the camera is finally modified.
    }

    void GameStartCamera()
    {
        //Vector3 moveVec, camNextNode, camNextNextNode;
        //camNextNode = transform.position - panNodes[currentNode].transform.position;
        //camNextNextNode = transform.position - panNodes[currentNode+1].transform.position;
        //moveVec = (camNextNode + camNextNextNode) * -1;

        //transform.position = transform.position + (moveVec.normalized * 0.2f);

        //transform.Translate(moveVec.normalized * 10);

        transform.position = Vector3.MoveTowards(transform.position, panNodes[currentNode].transform.position, 0.2f);
        transform.LookAt(lookAtPoint.transform.position);

        //Vector3 middlePointBetweenNodes = panNodes[currentNode].transform.position + panNodes[currentNode + 1].transform.position;
        //middlePointBetweenNodes = middlePointBetweenNodes / 2;

        if (Vector3.Distance(transform.position, panNodes[currentNode].transform.position) < tolerance)
        {
            currentNode = (currentNode + 1) % panNodes.Length;
        }
        //if(Vector3.Distance(transform.position, middlePointBetweenNodes) < tolerance)
        //{
        //    currentNode = (currentNode + 1) % panNodes.Length;
        //}
    }

    public void setFocus(GameObject target)
    {
        if(target == focused)
        {
            stopFocus();
        }
        else
        {
            focused = target;
            mode = CameraMode.FOCUSED;
        }
    }

    public void stopFocus()
    {
        focused = null;
        mode = CameraMode.PLAYER;
    }
}