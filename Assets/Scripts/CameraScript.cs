using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraScript : MonoBehaviour {

    public GameObject player;
    private Vector3 offset;
    public float speed = 4.0f;

	// Use this for initialization
	void Start () {
        offset = transform.position - player.transform.position;
	}

    // LateUpdate is called after Update each frame
    void FixedUpdate () {

        float interpolation = speed * Time.deltaTime;

        Vector3 position = transform.position;
        position.y = Mathf.Lerp(transform.position.y, player.transform.position.y + offset.y, interpolation);
        position.x = Mathf.Lerp(transform.position.x, player.transform.position.x + offset.x, interpolation);
        position.z = Mathf.Lerp(transform.position.z, player.transform.position.z + offset.z, interpolation);

        transform.position = position ;

        /*
        // Set the position of the camera's transform to be the same as the player's, but offset by the calculated offset distance.
        transform.position = player.transform.position + offset;*/
    }
}
