using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestSpawnFormula : MonoBehaviour 
{
	public float baseSpawnRateInSeconds; //in seconds
	[Range(0, 2)] public float currentSpawnRateMultiplier;

	[Range(0, 100)] public float maxNumberToSpawnInScene;
	[Range(0, 100)] public float currentNumberInScene;

	[Range(0, 100)] public float percentInScene;	
	[Range(0, 100)] public float percentSpawnSpeed;

	// Use this for initialization
	void Start () 
	{
		percentInScene = currentNumberInScene/maxNumberToSpawnInScene * 100f;
		percentSpawnSpeed = 100f - percentInScene;

		currentSpawnRateMultiplier = 1 + percentSpawnSpeed * 1f/100;

		baseSpawnRateInSeconds /= currentSpawnRateMultiplier;		
	}
	
	// Update is called once per frame
	void Update () 
	{
	}
}
