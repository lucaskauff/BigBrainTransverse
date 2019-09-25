using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using Sirenix.Serialization;
using System.Diagnostics;

public class SpawnManager : MonoBehaviour
{
    [SerializeField] int maxNumberOfEntitiesToSpawn;
    [SerializeField] List<GameObject> spawnPointsList = new List<GameObject>();
    [ShowInInspector] public static List<GameObject> citizensInScene = new List<GameObject>();
    [SerializeField] GameObject[] people;
    [SerializeField] float numberToSpawn;

    [SerializeField] Stopwatch spawnTimer = new Stopwatch();
    [SerializeField] float intervalToSpawnInSeconds;

    //Scientist Spawning Values
    [SerializeField] GameObject scientist;
    [SerializeField] Stopwatch spawnScientistTimer = new Stopwatch();
    [SerializeField] float timeToSpawnScientist;
    [SerializeField] int minTimeToScientist;
    [SerializeField] int maxTimeToScientist;

    void Start()
    {
        citizensInScene.Clear();
    }
	void Update ()
    {
        if (citizensInScene.Count < maxNumberOfEntitiesToSpawn) SpawnMobs();
        else return;
    }

    void SpawnMobs() 
    {
        spawnTimer.Start();
        spawnScientistTimer.Start();

        if (spawnTimer.Elapsed.TotalSeconds >= intervalToSpawnInSeconds) {
            for (int i = 0; i < numberToSpawn; i++) 
            {
                foreach (GameObject citizenSpawner in spawnPointsList) 
                {
                    if (spawnScientistTimer.Elapsed.TotalSeconds >= timeToSpawnScientist)
                    {
                        scientist = Instantiate(people[Random.Range(0, people.Length-1)], citizenSpawner.transform.position, Quaternion.identity);
                        scientist.GetComponent<CitizenController>().isScientist = true;
                        spawnScientistTimer.Stop();
                        spawnScientistTimer.Reset();
                        timeToSpawnScientist = Random.Range(minTimeToScientist, maxTimeToScientist);
                    }
                    else Instantiate(people[Random.Range(0, people.Length-1)], citizenSpawner.transform.position, Quaternion.identity);
                }
            }
            spawnTimer.Stop();
            spawnTimer.Reset();
        }
    }
}
