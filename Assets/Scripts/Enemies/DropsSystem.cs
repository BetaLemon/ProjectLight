using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DropsSystem : MonoBehaviour {

    [Tooltip("Add prefab drops for this enemy here")]
    public GameObject[] drops;
    private GameObject justInstantiated; //Reference to the last instaned object
    [Tooltip("These ammounts correspond to each item to spawn respectively")]
    public int[] ammounts;

    public void Drop(Vector3 position, Quaternion rotation)
    {
        for (int i = 0; i < drops.Length; i++)
        {
            for (int j = 0; j < ammounts[i]; j++)
            {
                justInstantiated = Instantiate(drops[i], position, rotation);
                if (justInstantiated.tag == "SmallGemstone" || justInstantiated.tag == "ManaCharge") {
                    float generatedRandomX = Random.Range(-3.0f, 3.0f);
                    float generatedRandomZ = Random.Range(-3.0f, 3.0f);
                    justInstantiated.GetComponent<Rigidbody>().AddForce(generatedRandomX, 0, generatedRandomZ, ForceMode.Impulse); //Sparces the drops randomly
                }
            }
        }
    }

    public void Drop(Vector3 position)
    {
        for (int i = 0; i < drops.Length; i++)
        {
            for (int j = 0; j < ammounts[i]; j++)
            {
                justInstantiated = Instantiate(drops[i], position, Quaternion.identity);
                if (justInstantiated.tag == "SmallGemstone" || justInstantiated.tag == "ManaCharge") {
                    float generatedRandomX = Random.Range(-3.0f, 3.0f);
                    float generatedRandomZ = Random.Range(-3.0f, 3.0f);
                    justInstantiated.GetComponent<Rigidbody>().AddForce(generatedRandomX, 0, generatedRandomZ, ForceMode.Impulse); //Sparces the drops randomly
                }
            }
        }
    }
}
