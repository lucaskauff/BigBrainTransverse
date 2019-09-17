using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using Sirenix.Serialization;

public class SpawnManager : MonoBehaviour {

    [SerializeField] List<GameObject> spawnPointsList = new List<GameObject>();
    [SerializeField] GameObject people;
    [SerializeField] float numberToSpawn;

    // Use this for initialization
    void Start ()
    {
        for (int i = 0; i < numberToSpawn; i++)
        {
            foreach (GameObject citizenSpawner in spawnPointsList)
            {
                Instantiate(people, citizenSpawner.transform.position, Quaternion.identity);
            }
        }
    }
	
	// Update is called once per frame
	void Update ()
    {
        
	}
}
