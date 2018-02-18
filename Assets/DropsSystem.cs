using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DropsSystem : MonoBehaviour {

    [Tooltip("Add prefab drops for this enemy here")]
    public GameObject[] drops;

    public void Drop(Vector3 position, Quaternion rotation)
    {
        for (int i = 0; i < drops.Length; i++)
        {
            Instantiate(drops[i], position, rotation);
        }
    }
}
