using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using Sirenix.Serialization;
using System.Diagnostics;

public class SpawnManager : MonoBehaviour
{
    [FoldoutGroup("Gameplay Variables")][SerializeField] int maxNumberOfEntitiesToSpawn;
    [FoldoutGroup("Gameplay Variables")][SerializeField] List<GameObject> spawnPointsList = new List<GameObject>();
    [FoldoutGroup("Gameplay Variables")][SerializeField] GameObject[] people;
    [FoldoutGroup("Gameplay Variables")][SerializeField] float simultaneousSpawnNumber;
	[FoldoutGroup("Gameplay Variables")][SerializeField] float baseSpawnRate; //in seconds
    [FoldoutGroup("Gameplay Variables")][SerializeField] float currentSpawnRate;

    [SerializeField] Stopwatch spawnTimer = new Stopwatch();

    //Scientist Spawning Values
    [FoldoutGroup("Scientist Debugging")][SerializeField] GameObject scientist;
    [FoldoutGroup("Scientist Debugging")][SerializeField] Stopwatch spawnScientistTimer = new Stopwatch();
    [FoldoutGroup("Scientist Debugging")][SerializeField] float timeToSpawnScientist;
    [FoldoutGroup("Scientist Debugging")][SerializeField] int minTimeToScientist;
    [FoldoutGroup("Scientist Debugging")][SerializeField] int maxTimeToScientist;
    
    [FoldoutGroup("Debugging")][ShowInInspector] public static List<GameObject> citizensInScene = new List<GameObject>();
	[FoldoutGroup("Debugging")][SerializeField]  float currentSpawnRateMultiplier;
	[FoldoutGroup("Debugging")][SerializeField] [Range(0, 100)] float maxNumberToSpawnInScene;
	[FoldoutGroup("Debugging")][SerializeField][Range(0, 100)] float currentNumberInScene;
	[FoldoutGroup("Debugging")][SerializeField][Range(0, 100)] float percentInScene;	
	[FoldoutGroup("Debugging")][SerializeField][Range(0, 100)] float percentSpawnSpeed;

    void Start()
    {
        citizensInScene.Clear();
        currentSpawnRate = baseSpawnRate;
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
