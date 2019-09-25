using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using Sirenix.Serialization;
using System.Diagnostics;

public class PlayerController : MonoBehaviour
{
    //GameManager
    NewInputManager inputManager;

    [Header("Public variables")]
    public bool ultimateEnabled;

    [Header("Serializable variables")]
    [SerializeField] bool isPlayerOne;

    [FoldoutGroup("Debug Variables")] [SerializeField] GameObject equippedFood;
    [FoldoutGroup("Debug Variables")] [SerializeField] int currentlySelectedFood;

    [FoldoutGroup("Internal Variables")] [SerializeField] List<GameObject> foodWeapons = new List<GameObject>();
    [FoldoutGroup("Internal Variables")] //[SerializeField] Transform spawnPoint;
    GameObject cloneProj;

    void Start()
    {
        inputManager = NewGameManager.Instance.inputManager;

        currentlySelectedFood = 0;
    }

    void Update()
    {
        //UnityEngine.Debug.DrawRay(Camera.main.transform.position, transform.position);

        UpdatePosition();

        WeaponSwitch();

        if (inputManager.mouseLeftClick)
            ShootProjectile();
        else
        {
            if (isPlayerOne && inputManager.swipeDownP1)
                ShootProjectile();
            else if (!isPlayerOne && inputManager.swipeDownP2)
                ShootProjectile();
        }
    }

    void UpdatePosition()
    {
        if (isPlayerOne)
            transform.position = Vector3.Lerp(transform.position, inputManager.cursor1Pos, inputManager.smoothFactor * Time.deltaTime);
        else
            transform.position = Vector3.Lerp(transform.position, inputManager.cursor2Pos, inputManager.smoothFactor * Time.deltaTime);
    }

    void ShootProjectile()
    {
        cloneProj = Instantiate(equippedFood, transform.position, Quaternion.identity);
        cloneProj.gameObject.SendMessage("MoveToPosition", transform.position);
    }

    void WeaponSwitch()
    {
        if (inputManager.weaponMinusKey)
            currentlySelectedFood--;
        else
        {
            if (isPlayerOne && inputManager.swipeLeftP1)
                currentlySelectedFood--;
            else if (!isPlayerOne && inputManager.swipeLeftP2)
                currentlySelectedFood--;
        }

        if (inputManager.weaponPlusKey)
            currentlySelectedFood++;
        else
        {
            if (isPlayerOne && inputManager.swipeRightP1)
                currentlySelectedFood++;
            else if (!isPlayerOne && inputManager.swipeRightP2)
                currentlySelectedFood++;
        }

        if (!ultimateEnabled)
        {
            if (currentlySelectedFood < 0)
                currentlySelectedFood = foodWeapons.Count - 2;
            else if (currentlySelectedFood > foodWeapons.Count - 2)
                currentlySelectedFood = 0;
        }
        else
        {
            if (currentlySelectedFood < 0)
                currentlySelectedFood = foodWeapons.Count - 1;
            else if (currentlySelectedFood > foodWeapons.Count - 1)
                currentlySelectedFood = 0;
        }

        equippedFood = foodWeapons[currentlySelectedFood];
    }
}