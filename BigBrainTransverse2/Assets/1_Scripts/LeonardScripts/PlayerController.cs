using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {

    [SerializeField] Transform spawnPoint;
    [SerializeField] GameObject food;

    GameObject cloneProj;

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update ()
    {
		if(Input.GetMouseButtonDown(0))
        {
            cloneProj = Instantiate(food, spawnPoint.position, Quaternion.identity);
            cloneProj.gameObject.SendMessage("MoveToPosition");
        }
	}
}
