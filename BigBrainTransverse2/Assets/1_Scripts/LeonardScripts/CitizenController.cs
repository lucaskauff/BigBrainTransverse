using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using Sirenix.Serialization;
using System.Diagnostics;

public class CitizenController : MonoBehaviour
{

    #region VARIABLE DECLARATIONS
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
        SpawnManager.citizensInScene.Add(this.gameObject);
        currentMaxCalorieTol = baseMaxCalorieTol;
        rb = GetComponent<Rigidbody>();
        intervalToRandomizeRotation = Random.Range(0f, 5f);
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
                    break;
                case FoodType.sweet:
                    SweetEffect(foodData.CalorieGainOnHit, foodData.CalorieToleranceDecrease);
                    cubeRenderer.material.SetColor("_Color", Color.blue);
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
        UnityEngine.Debug.Log("I just gained " + caloriesGained + " calories");

        currentCalories += caloriesGained;

        citizenMoveSpeed /= speedDecreaseMultiplier;
    }

    void SweetEffect(int caloriesGained, int toleranceDecrease)
    {
        UnityEngine.Debug.Log("I just gained " + caloriesGained + " calories and my tolerance went down by " + toleranceDecrease);

        currentCalories += caloriesGained;
        currentMaxCalorieTol -= toleranceDecrease;
    }

    void EnergyEffect(int caloriesGained, int overTimeCalGain)
    {
        UnityEngine.Debug.Log("I just gained " + caloriesGained + " calories and i'm gaining " + overTimeCalGain + " calories over time");

        currentCalories += overTimeCalGain * Time.deltaTime;
    }

    void CheckCitizenStatus()
    {
        if (currentCalories >= currentMaxCalorieTol)
        {
            Dead();
        }
    }

    void FoodHits(string whichFood)
    {
        int greasy = 0;
        int sweet = 0;
        float greasePercentage = greasy / (greasy + sweet);
        float sweetPercentage = sweet / (greasy + sweet);
        float highestPercentage = Mathf.Max(greasePercentage, sweetPercentage);
    }

    void Dead()
    {
        SpawnManager.citizensInScene.Remove(this.gameObject);
        Destroy(gameObject);
        //Destroy Colliders
        //Lock Animation on last frame
    }
    #endregion
}
