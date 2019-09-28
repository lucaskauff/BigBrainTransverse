﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using Sirenix.Serialization;
using System.Diagnostics;

public class CitizenController : MonoBehaviour
{
    #region VARIABLE DECLARATIONS
    //GameManager
    [FoldoutGroup("Components")][SerializeField] Collider myCol;
    [FoldoutGroup("Components")][SerializeField] Animator myAnim;
    [FoldoutGroup("Components")][SerializeField] Rigidbody rb;
    [FoldoutGroup("Components")][SerializeField] Renderer cubeRenderer;
    [FoldoutGroup("Components")][SerializeField] ParticleSystem particleSystem;

    [FoldoutGroup("UI")][SerializeField] TheBubble bubble;
    [FoldoutGroup("UI")][SerializeField] Transform reason;

    [FoldoutGroup("Gameplay")][SerializeField] float citizenMoveSpeed;
    [FoldoutGroup("Gameplay")][SerializeField] float speedDecreaseMultiplier;
    [FoldoutGroup("Gameplay")][SerializeField] float baseMaxCalorieTol; //maximum calorie tolerance when game starts   

    [FoldoutGroup("Debugging")] public bool isScientist;
    [FoldoutGroup("Debugging")][SerializeField] bool hitWall = false;
    [FoldoutGroup("Debugging")][SerializeField] bool isHitByEnergy = false; 
    [FoldoutGroup("Debugging")][SerializeField] bool isDead = false;
    [FoldoutGroup("Debugging")][SerializeField] float currentMaxCalorieTol; //maximum calorie tolerance at any given time
    [FoldoutGroup("Debugging")][SerializeField] float currentCalories;
    [FoldoutGroup("Debugging")][SerializeField] float intervalToRandomizeRotation;
    [FoldoutGroup("Debugging")][SerializeField] int greasyHits = 0;
    [FoldoutGroup("Debugging")][SerializeField] int sweetHits = 0;
    
    NewGameManager gameManager;
    FoodData foodData;
    Stopwatch timer = new Stopwatch();
    Vector3 baseRotationVector;
    Vector3 theCamera;
    #endregion

    #region //BASE UNITY CALLBACKS
    // Use this for initialization
    void Start()
    {
        gameManager = NewGameManager.Instance;
        particleSystem = GetComponentInChildren<ParticleSystem>();

        SpawnManager.citizensInScene.Add(this.gameObject);
        currentMaxCalorieTol = baseMaxCalorieTol;
        intervalToRandomizeRotation = Random.Range(2f, 5f);
        cubeRenderer = GetComponentInChildren<Renderer>();

        if(isScientist) this.gameObject.name = "SCIENTIST";

        theCamera = Camera.main.transform.position;
        
        particleSystem.Stop();
    }

    // Update is called once per frame
    void Update()
    {
        if (isDead) return;

        Movement();

        timer.Start();
        if (timer.Elapsed.TotalSeconds >= intervalToRandomizeRotation && !hitWall)
        {
            RandomizeRotation();
            timer.Stop();
            timer.Reset();
        }

        if (isHitByEnergy) EnergyEffect(foodData.CalorieGainOnHit, foodData.CalorieGainOverTime);
    }

    void LateUpdate()
    {
        CheckCitizenStatus();
    }

    void OnCollisionEnter(Collision collision)
    {
        //use to detect collision against wall and change the facing direction
        if (collision.gameObject.tag == "Walls" || collision.gameObject.tag == "Buildings")
        {
            hitWall = true;
            WallHitRotation();
        }

        if (collision.gameObject.tag == "Food")
        {
            rb.constraints = RigidbodyConstraints.FreezeAll;
            myAnim.SetTrigger("Hit");

            switch (foodData.FoodType)
            {
                case FoodType.greasy:
                    GreaseEffect(foodData.CalorieGainOnHit);
                    cubeRenderer.material.SetColor("_Color", Color.red);
                    greasyHits++;
                break;
                case FoodType.sweet:
                    SweetEffect(foodData.CalorieGainOnHit, foodData.CalorieToleranceDecrease);
                    cubeRenderer.material.SetColor("_Color", Color.blue);
                    sweetHits++;
                break;
                case FoodType.energy:
                    EnergyEffect(foodData.CalorieGainOnHit, foodData.CalorieGainOverTime);
                    cubeRenderer.material.SetColor("_Color", Color.green);
                    isHitByEnergy = true;
                break;
            }

            if(isScientist) UnityEngine.Debug.Log("You just hit a scientist");//AnimationManager.DeathCamManager(this.gameObject, "Scientist");
        }
    }

    void OnCollisionExit(Collision collision)
    {   
        if (collision.gameObject.tag == "Food")
        {
            rb.constraints = RigidbodyConstraints.None;
        }
    }
    #endregion

    #region //CUSTOM FUNCTIONS
    //Message Receiver - Sender is in FoodBehavior script
    void WhichFoodType(FoodData receivedFoodData)
    {
        foodData = receivedFoodData;
    }

    void Movement()
    {
        float step = citizenMoveSpeed * Time.deltaTime; // calculate distance to move
        rb.MovePosition(transform.position + transform.forward * step);
    }

    void RandomizeRotation()
    {
        baseRotationVector = transform.rotation.eulerAngles;
        transform.rotation = Quaternion.Euler(baseRotationVector.x, Random.Range(-90, 90), baseRotationVector.z);
        intervalToRandomizeRotation = Random.Range(1f, 3f);
    }

    void WallHitRotation()
    {
        baseRotationVector = transform.rotation.eulerAngles;        
        transform.rotation = Quaternion.Euler(baseRotationVector.x, Random.Range(200, 160), baseRotationVector.z);
        hitWall = false;
    }

    void GreaseEffect(int caloriesGained)
    {
        currentCalories += caloriesGained;
        citizenMoveSpeed /= speedDecreaseMultiplier;
        //particleSystem.Play();
    }

    void SweetEffect(int caloriesGained, int toleranceDecrease)
    {
        currentCalories += caloriesGained;
        currentMaxCalorieTol -= toleranceDecrease;
    }

    void EnergyEffect(int caloriesGained, int overTimeCalGain)
    {
        currentCalories += overTimeCalGain * Time.deltaTime;
    }

    void CheckCitizenStatus()
    {
        if (currentCalories >= currentMaxCalorieTol)
        {
            Dead();
        }
    }

    void Dead()
    {
        isDead = true;

        SpawnManager.citizensInScene.Remove(this.gameObject);
        myCol.enabled = false;
        rb.constraints = RigidbodyConstraints.FreezeAll;
        
        if (greasyHits > sweetHits) myAnim.SetInteger("DeadType", 1);
        else if (greasyHits < sweetHits) myAnim.SetInteger("DeadType", 2);
        else if (isHitByEnergy) myAnim.SetInteger("DeadType", 3);

        if (!gameManager.isDeathAnimOnGoing)
        {
            bubble.transform.LookAt(theCamera);
            reason.LookAt(theCamera);
            reason.localPosition = new Vector3(reason.localPosition.x, reason.localPosition.y, reason.localPosition.z+0.01f);
            if (greasyHits > sweetHits) bubble.whichDeath = 0;
            else if (greasyHits < sweetHits) bubble.whichDeath = 1;
            else if(isHitByEnergy) bubble.whichDeath = 2;
            bubble.GetComponent<Animator>().SetTrigger("FadeIn");
        }
    }
    #endregion
}