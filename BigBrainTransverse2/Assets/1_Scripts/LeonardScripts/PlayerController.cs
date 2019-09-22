using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using Sirenix.Serialization;
using System.Diagnostics;

public class PlayerController : MonoBehaviour
{
    GameObject cloneProj;

    [FoldoutGroup("PlayerController Variables")] [SerializeField] List<GameObject> foodWeapons = new List<GameObject>();
    [FoldoutGroup("PlayerController Variables")] [SerializeField] Transform spawnPoint;
    [FoldoutGroup("PlayerController Variables")] [SerializeField] GameObject equippedFood;

    [FoldoutGroup("Internal Variables")] [SerializeField] int currentlySelectedFood;
    [FoldoutGroup("Internal Variables")] [ShowInInspector] public static bool isGreasy = true;
    [FoldoutGroup("Internal Variables")] [ShowInInspector] public static bool isSweet = false;
    [FoldoutGroup("Internal Variables")] [ShowInInspector] public static bool isEnergy = false;


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
        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            /*isGreasy = true;
            isSweet = false;
            isEnergy = false;*/
            currentlySelectedFood++;
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            /*isGreasy = false;
            isSweet = true;
            isEnergy = false;*/
            currentlySelectedFood--;
        }

        if(currentlySelectedFood > foodWeapons.Count-1 || currentlySelectedFood < 0)
        {
            currentlySelectedFood = 0;
        }

        equippedFood = foodWeapons[currentlySelectedFood];
    }
}
