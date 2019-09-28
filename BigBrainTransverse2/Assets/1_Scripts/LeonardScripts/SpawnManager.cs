using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using Sirenix.Serialization;
using System.Diagnostics;

public class SpawnManager : MonoBehaviour
{
    [FoldoutGroup("Gameplay")][SerializeField] List<GameObject> spawnPointsList = new List<GameObject>();
    [FoldoutGroup("Gameplay")][SerializeField] GameObject[] people;
    [FoldoutGroup("Gameplay")][SerializeField] int maxNumberOfEntitiesToSpawn;
    [FoldoutGroup("Gameplay")][SerializeField] float simultaneousSpawnNumber;
	[FoldoutGroup("Gameplay")][SerializeField] float baseSpawnRate; //in seconds

    [SerializeField] Stopwatch spawnTimer = new Stopwatch();

    //Scientist Spawning Values
    [FoldoutGroup("Scientist")][SerializeField] GameObject scientist;
    [FoldoutGroup("Scientist")][SerializeField] Stopwatch spawnScientistTimer = new Stopwatch();
    [FoldoutGroup("Scientist")][SerializeField] float baseWaitForScientist;
    [FoldoutGroup("Scientist")][SerializeField] float timeToSpawnScientist;
    [FoldoutGroup("Scientist")][SerializeField] int minTimeToScientist;
    [FoldoutGroup("Scientist")][SerializeField] int maxTimeToScientist;
    
    [FoldoutGroup("Debugging")][ShowInInspector] public static List<GameObject> citizensInScene = new List<GameObject>();
	[FoldoutGroup("Debugging")][SerializeField] float currentSpawnRateMultiplier;
    [FoldoutGroup("Gameplay")][SerializeField] float currentSpawnRate;
	[FoldoutGroup("Debugging")][SerializeField][Range(0, 100)] float maxNumberToSpawnInScene;
	[FoldoutGroup("Debugging")][SerializeField][Range(0, 100)] float currentNumberInScene;
	[FoldoutGroup("Debugging")][SerializeField][Range(0, 100)] float percentInScene;	
	[FoldoutGroup("Debugging")][SerializeField][Range(0, 100)] float percentSpawnSpeed;

    void Start()
    {
        citizensInScene.Clear();
        currentSpawnRate = baseSpawnRate;
        timeToSpawnScientist = baseWaitForScientist;
    }
	void Update ()
    {
        if (citizensInScene.Count < maxNumberOfEntitiesToSpawn) SpawnMobs();

        UpdateVariables();
    }

    void UpdateVariables()
    {
        currentNumberInScene = citizensInScene.Count;
        maxNumberToSpawnInScene = maxNumberOfEntitiesToSpawn;
    }

    void SpawnMobs() 
    {
        spawnTimer.Start();
        spawnScientistTimer.Start();

        if (spawnTimer.Elapsed.TotalSeconds >= currentSpawnRate) {
            for (int i = 0; i < simultaneousSpawnNumber; i++) 
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

                    percentInScene = currentNumberInScene/maxNumberToSpawnInScene * 100f;
                    percentSpawnSpeed = 100f - percentInScene;

                    currentSpawnRateMultiplier = percentSpawnSpeed * 1f/100;
                }
            }
            spawnTimer.Stop();
            spawnTimer.Reset();
            currentSpawnRate = baseSpawnRate;
            currentSpawnRate /= currentSpawnRateMultiplier;	
        }
    }
}
