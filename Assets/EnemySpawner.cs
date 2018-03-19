using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour {

    public BlackInsect[] blackInsects;
    public float spawnDelay = 3; // Seconds

    struct Insect
    {
        public BlackInsect insect;
        public float passedSpawnTime;
        public bool waitsForRespawn;

        public Insect(BlackInsect ins, float t, bool res)
        {
            insect = ins; passedSpawnTime = t; waitsForRespawn = res;
        }
    }
    
    private Insect[] insects;

	// Use this for initialization
	void Start () {
        insects = new Insect[blackInsects.Length];
		for(int i = 0; i < blackInsects.Length; i++)
        {
            insects[i] = new Insect(blackInsects[i], 0, false);
            if(blackInsects[i].enemySpawner == null) { blackInsects[i].enemySpawner = this; }
        }
	}
	
	// Update is called once per frame
	void Update () {
        for(int i = 0; i < insects.Length; i++)
        {
            if (insects[i].waitsForRespawn)
            {
                insects[i].passedSpawnTime = insects[i].passedSpawnTime + Time.deltaTime;
                print(insects[i].passedSpawnTime);
            }

            if(insects[i].passedSpawnTime > spawnDelay)
            {
                insects[i].passedSpawnTime = 0;
                insects[i].insect.Spawn(transform.position);
                insects[i].waitsForRespawn = false;
            }
        }
	}

    public void PleaseRespawn(BlackInsect ins)
    {
        int index = 0;
        for(index = 0; index < insects.Length; index++)
        {
            if(insects[index].insect == ins) { break; }
        }

        insects[index].waitsForRespawn = true;
    }
}
