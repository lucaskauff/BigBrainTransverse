using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using Sirenix.Serialization;
using System.Diagnostics;

public class PlayerController : MonoBehaviour
{
    KinectManager kinectManager;
    [SerializeField] GestureListener gestureListener;

    [FoldoutGroup("Debug Variables")] [SerializeField] GameObject equippedFood;
    [FoldoutGroup("Debug Variables")] [SerializeField] int currentlySelectedFood;

    [FoldoutGroup("Internal Variables")] [SerializeField] List<GameObject> foodWeapons = new List<GameObject>();
    [FoldoutGroup("Internal Variables")] [SerializeField] Transform spawnPoint;
    GameObject cloneProj;

    void Start()
    {
        kinectManager = KinectManager.Instance;

        currentlySelectedFood = 0;
    }

    void Update()
    {
        if (kinectManager && kinectManager.IsInitialized() && kinectManager.IsUserDetected())
        {
            //if (gestureListener.IsSwipeDown())
                //ShootProjectile();
        }
        else
        {
            if (Input.GetMouseButtonDown(0))
                ShootProjectile();
        }

        WeaponSwitch();
    }

    void ShootProjectile()
    {
        cloneProj = Instantiate(equippedFood, spawnPoint.position, Quaternion.identity);
        cloneProj.gameObject.SendMessage("MoveToPosition");
    }

    void WeaponSwitch() 
    {
        if (Input.GetKeyDown(KeyCode.LeftShift)) currentlySelectedFood++;
        if (Input.GetKeyDown(KeyCode.Space)) currentlySelectedFood--;
        if (currentlySelectedFood > foodWeapons.Count - 1 || currentlySelectedFood < 0) currentlySelectedFood = 0;

        equippedFood = foodWeapons[currentlySelectedFood];
    }
}
