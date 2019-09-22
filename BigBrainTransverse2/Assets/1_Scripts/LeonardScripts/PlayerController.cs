using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using Sirenix.Serialization;
using System.Diagnostics;

public class PlayerController : MonoBehaviour
{
    [FoldoutGroup("Debug Variables")] [SerializeField] GameObject equippedFood;
    [FoldoutGroup("Debug Variables")] [SerializeField] int currentlySelectedFood;

    [FoldoutGroup("Internal Variables")] [SerializeField] List<GameObject> foodWeapons = new List<GameObject>();
    [FoldoutGroup("Internal Variables")] [SerializeField] Transform spawnPoint;
    GameObject cloneProj;

    // Use this for initialization
    void Start()
    {
        currentlySelectedFood = 0;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (Input.GetMouseButtonDown(0))
        {
            cloneProj = Instantiate(equippedFood, spawnPoint.position, Quaternion.identity);
            cloneProj.gameObject.SendMessage("MoveToPosition");
        }
    }

    void Update()
    {
        WeaponSwitch();
    }

    void WeaponSwitch() 
    {
        if (Input.GetKeyDown(KeyCode.LeftShift)) currentlySelectedFood++;
        if (Input.GetKeyDown(KeyCode.Space)) currentlySelectedFood--;
        if (currentlySelectedFood > foodWeapons.Count - 1 || currentlySelectedFood < 0) currentlySelectedFood = 0;

        equippedFood = foodWeapons[currentlySelectedFood];
    }
}
