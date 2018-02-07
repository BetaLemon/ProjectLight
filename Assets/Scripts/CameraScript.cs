using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraScript : MonoBehaviour {

    // This script controls camera movement

    public GameObject player;   // Stores the player, to use his position later on.
    private Vector3 offset;     // Vector that stores the offset between the player and the camera.
    public float speed = 4.0f;  // Speed at which the camera lerps.

	// Use this for initialization
	void Start () {
        offset = transform.position - player.transform.position;    // The offset is read at start. Might be changed at some point in time.
	}

    // LateUpdate is called after Update each frame
    void FixedUpdate () {

        float interpolation = speed * Time.deltaTime;   // Calculates the interpolation for a smooth lerp.

        Vector3 position = transform.position;  // stores the camera's position.
        // We change the camera's coordinates following the player's position and applying interpolation:
        position.y = Mathf.Lerp(transform.position.y, player.transform.position.y + offset.y, interpolation);
        position.x = Mathf.Lerp(transform.position.x, player.transform.position.x + offset.x, interpolation);
        position.z = Mathf.Lerp(transform.position.z, player.transform.position.z + offset.z, interpolation);

        transform.position = position ; // The position of the camera is finally modified.

        /*
        // Set the position of the camera's transform to be the same as the player's, but offset by the calculated offset distance.
        transform.position = player.transform.position + offset;*/
    }
}
