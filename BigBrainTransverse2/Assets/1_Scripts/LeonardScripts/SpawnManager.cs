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
    [SerializeField] float intervalToSpawnInSeconds;
    [SerializeField] Stopwatch timer = new Stopwatch();

	void Update ()
    {
        if (citizensInScene.Count < maxNumberOfEntitiesToSpawn) SpawnMobs();
        else return;
    }

    void SpawnMobs() 
    {
        timer.Start();

        if (timer.Elapsed.TotalSeconds >= intervalToSpawnInSeconds) {
            for (int i = 0; i < numberToSpawn; i++) 
            {
                foreach (GameObject SpawnPoint in spawnPointsList) 
                {
                    Vector3 baseRotationVector = SpawnPoint.transform.rotation.eulerAngles;
                    SpawnPoint.transform.rotation = Quaternion.Euler(baseRotationVector.x, Random.Range(-180, 180), baseRotationVector.z);
                }

                foreach (GameObject citizenSpawner in spawnPointsList) {
                    Instantiate(people[Random.Range(0, people.Length-1)], citizenSpawner.transform.position, Quaternion.identity);
                }
            }
            timer.Stop();
            timer.Reset();
        }
    }

    void RandomizeSpawnerRotations() 
    {
        
    }
}
