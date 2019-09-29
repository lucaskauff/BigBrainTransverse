using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using Sirenix.Serialization;
using System.Diagnostics;

public class PlayerController : MonoBehaviour
{
    //GameManager
    NewGameManager gameManager;
    NewInputManager inputManager;

    KinectManager kinectManager;

    [Header("Public variables")]
    public bool ultimateEnabled;

    [Header("Objects to serialize")]
    public GameObject yet;
    [SerializeField] UIManager uIManager;
    [SerializeField] GameObject[] allFoodWeapons;

    [Header("Serializable variables")]
    [SerializeField] bool isPlayerOne;
    [SerializeField] float decalWithoutKinect;

    [FoldoutGroup("Debug Variables")] [SerializeField] GameObject equippedFood;
    [FoldoutGroup("Debug Variables")] [SerializeField] int currentlySelectedFood;

    Vector3 startPos;
    GameObject[] foodWeapons;
    GameObject cloneProj;

    void Start()
    {
        gameManager = NewGameManager.Instance;
        inputManager = gameManager.inputManager;

        kinectManager = KinectManager.Instance;

        startPos = transform.position;

        ChooseWeaponsForRightLobby();
        currentlySelectedFood = 0;
    }

    void ChooseWeaponsForRightLobby()
    {
        foodWeapons = new GameObject[3];
        foodWeapons[2] = allFoodWeapons[4];

        if (isPlayerOne)
        {
            if (gameManager.selectedLobbyPlayers[0] == 0)
            {
                foodWeapons[0] = allFoodWeapons[0];
                foodWeapons[1] = allFoodWeapons[2];
            }
            else
            {
                foodWeapons[0] = allFoodWeapons[1];
                foodWeapons[1] = allFoodWeapons[3];
            }
        }
        else
        {
            if (gameManager.selectedLobbyPlayers[1] == 0)
            {
                foodWeapons[0] = allFoodWeapons[0];
                foodWeapons[1] = allFoodWeapons[2];
            }
            else
            {
                foodWeapons[0] = allFoodWeapons[1];
                foodWeapons[1] = allFoodWeapons[3];
            }
        }
    }

    void Update()
    {
        UpdatePosition();

        WeaponSwitch();

        if (isPlayerOne && inputManager.mouseLeftClick)
            ShootProjectile();
        else if (!isPlayerOne && inputManager.mouseRightClick)
            ShootProjectile();
        else
            if (isPlayerOne && (/*inputManager.swipeDownP1 ||*/ inputManager.pushP1 || inputManager.throwP1))
                ShootProjectile();
            else if (!isPlayerOne && (inputManager.swipeDownP2 || inputManager.pushP2 || inputManager.throwP2))
                ShootProjectile();

        if (isPlayerOne)
            InfosToUI(0);
        else
            InfosToUI(1);
    }

    void UpdatePosition()
    {
        if (isPlayerOne)
            if (kinectManager)
                transform.position = Vector3.Lerp(transform.position, inputManager.cursor1Pos, inputManager.smoothFactor * Time.deltaTime);
            else
                transform.position = new Vector3(startPos.x + decalWithoutKinect, startPos.y, startPos.z);
        else
            if (kinectManager)
                transform.position = Vector3.Lerp(transform.position, inputManager.cursor2Pos, inputManager.smoothFactor * Time.deltaTime);
            else
                transform.position = new Vector3(startPos.x + decalWithoutKinect, startPos.y, startPos.z);
    }

    void ShootProjectile()
    {
        if (currentlySelectedFood == 2)
            ultimateEnabled = false;

        cloneProj = Instantiate(equippedFood, transform.position, Quaternion.identity);

        if (isPlayerOne)
            cloneProj.gameObject.GetComponent<FoodBehavior>().shotByPlayerIndex = 0;
        else
            cloneProj.gameObject.GetComponent<FoodBehavior>().shotByPlayerIndex = 1;

        cloneProj.gameObject.SendMessage("MoveToPosition", yet.transform.position);
    }

    void WeaponSwitch()
    {
        if ((isPlayerOne && inputManager.weaponMinusKeyP1)
            || (!isPlayerOne && inputManager.weaponMinusKeyP2))
            currentlySelectedFood--;
        else
        {
            if (isPlayerOne && inputManager.swipeLeftP1)
                currentlySelectedFood--;
            else if (!isPlayerOne && inputManager.swipeLeftP2)
                currentlySelectedFood--;
        }

        if ((isPlayerOne && inputManager.weaponPlusKeyP1)
            || (!isPlayerOne && inputManager.weaponPlusKeyP2))
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
                currentlySelectedFood = foodWeapons.Length - 2;
            else if (currentlySelectedFood > foodWeapons.Length - 2)
                currentlySelectedFood = 0;
        }
        else
        {
            if (currentlySelectedFood < 0)
                currentlySelectedFood = foodWeapons.Length - 1;
            else if (currentlySelectedFood > foodWeapons.Length - 1)
                currentlySelectedFood = 0;
        }

        equippedFood = foodWeapons[currentlySelectedFood];
    }

    void InfosToUI(int playerIndex)
    {
        uIManager.doesPlayerHaveUlt[playerIndex] = ultimateEnabled;
        uIManager.playerSelection[playerIndex] = currentlySelectedFood;
    }
}