using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using Sirenix.Serialization;

public class Spawner : MonoBehaviour {

    public GameObject spawner;
    public GameObject people;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update ()
    {
        Instantiate(people, spawner.transform.position, Quaternion.identity);
    }
}
