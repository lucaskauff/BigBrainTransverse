using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using Sirenix.Serialization;
using System.Diagnostics;

public class CitizenController : MonoBehaviour
{
    #region VARIABLE DECLARATIONS
    //GameManager
    NewGameManager gameManager;

    [FoldoutGroup("Internal Components")] [SerializeField] Collider myCol;
    [FoldoutGroup("Internal Components")] [SerializeField] Animator myAnim;

    [FoldoutGroup("Objects to serialize")] [SerializeField] Animator bubble;
    //[FoldoutGroup("Objects to serialize")] [SerializeField] Animator reason;

    [FoldoutGroup("Debug Variables")] public bool isScientist;

    [FoldoutGroup("Debug Variables")] [SerializeField] float currentCalories;
    [FoldoutGroup("Debug Variables")] [SerializeField] float currentMaxCalorieTol; //maximum calorie tolerance at any given time
    bool isHitByEnergy = false;

    [FoldoutGroup("Internal Variables")] [SerializeField] float baseMaxCalorieTol; //maximum calorie tolerance when game starts
    [FoldoutGroup("Internal Variables")] [SerializeField] float citizenMoveSpeed;
    [FoldoutGroup("Internal Variables")] [SerializeField] float intervalToRandomizeRotation;
    [FoldoutGroup("Internal Variables")] [SerializeField] float speedDecreaseMultiplier;
    float step;
    FoodData foodData;

    [FoldoutGroup("Food Hit Values")] [SerializeField] int greasy = 0;
    [FoldoutGroup("Food Hit Values")] [SerializeField] int sweet = 0;

    //Components
    Renderer cubeRenderer;
    Stopwatch timer = new Stopwatch();
    Rigidbody rb;
    Vector3 baseRotationVector;
    #endregion

    #region //BASE UNITY CALLBACKS
    // Use this for initialization
    void Start()
    {
        NewGameManager gameManager;

        SpawnManager.citizensInScene.Add(this.gameObject);
        currentMaxCalorieTol = baseMaxCalorieTol;
        rb = GetComponent<Rigidbody>();
        intervalToRandomizeRotation = Random.Range(2f, 5f);
        cubeRenderer = GetComponentInChildren<Renderer>();
    }

    // Update is called once per frame
    void Update()
    {
        Movement();

        timer.Start();

        if (timer.Elapsed.TotalSeconds >= intervalToRandomizeRotation)
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
        if (collision.gameObject.tag == "Walls")
        {
            RandomizeRotation();
        }

        if (collision.gameObject.tag == "Food")
        {
            switch (foodData.FoodType)
            {
                case FoodType.greasy:
                    GreaseEffect(foodData.CalorieGainOnHit);
                    cubeRenderer.material.SetColor("_Color", Color.red);
                    greasy++;
                    break;
                case FoodType.sweet:
                    SweetEffect(foodData.CalorieGainOnHit, foodData.CalorieToleranceDecrease);
                    cubeRenderer.material.SetColor("_Color", Color.blue);
                    sweet++;
                    break;
                case FoodType.energy:
                    EnergyEffect(foodData.CalorieGainOnHit, foodData.CalorieGainOverTime);
                    cubeRenderer.material.SetColor("_Color", Color.green);
                    isHitByEnergy = true;
                    break;
                default:
                    break;
            }

            if(isScientist) UnityEngine.Debug.Log("You just hit a scientist");//AnimationManager.DeathCamManager(this.gameObject, "Scientist");
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
        step = citizenMoveSpeed * Time.deltaTime; // calculate distance to move
        rb.MovePosition(transform.position + transform.forward * step);
    }

    void RandomizeRotation()
    {
        baseRotationVector = transform.rotation.eulerAngles;
        transform.rotation = Quaternion.Euler(baseRotationVector.x, Random.Range(-180, 180), baseRotationVector.z);
        intervalToRandomizeRotation = Random.Range(0f, 1f);
    }

    void GreaseEffect(int caloriesGained)
    {
        currentCalories += caloriesGained;
        citizenMoveSpeed /= speedDecreaseMultiplier;
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
        //if(greasy > sweet) play Greasy anim;
        //if(greasy < sweet) play Sweet Anim;
        //if(isHitByEnergy) play Energy kill anim;
        SpawnManager.citizensInScene.Remove(this.gameObject);
        myCol.enabled = false;

        if (!gameManager.isDeathAnimOnGoing)
            bubble.SetTrigger("FadeIn");

        //Lock Animation on last frame
    }
    #endregion
}