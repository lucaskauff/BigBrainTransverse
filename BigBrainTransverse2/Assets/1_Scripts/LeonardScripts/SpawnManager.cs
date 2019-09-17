using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using Sirenix.Serialization;
using System.Diagnostics;

public class SpawnManager : MonoBehaviour {

    [SerializeField] List<GameObject> spawnPointsList = new List<GameObject>();
    [SerializeField] GameObject people;
    [SerializeField] float numberToSpawn;
    [SerializeField] float intervalToSpawnInSeconds;
    [SerializeField] Stopwatch timer = new Stopwatch();

    // Use this for initialization
    void Start ()
    {
        
    }
	
	// Update is called once per frame
	void Update ()
    {
        timer.Start();

        if (timer.Elapsed.TotalSeconds >= intervalToSpawnInSeconds)
        {
            for (int i = 0; i < numberToSpawn; i++)
            {
                foreach (GameObject citizenSpawner in spawnPointsList)
                {
                    Instantiate(people, citizenSpawner.transform.position, Quaternion.identity);
                }
            }
            timer.Stop();
            timer.Reset();
        }
    }
}
