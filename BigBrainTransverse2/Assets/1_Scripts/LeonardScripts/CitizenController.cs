using Sirenix.OdinInspector;
using System.Diagnostics;
using UnityEngine;

public class CitizenController : MonoBehaviour
{

    #region VARIABLE DECLARATIONS
    
    [FoldoutGroup("Debug Variables")] [SerializeField] float currentCalories;
    [FoldoutGroup("Debug Variables")] [SerializeField] float currentMaxCalorieTol; //maximum calorie tolerance at any given time

    [FoldoutGroup("Internal Variables")] [SerializeField] float baseMaxCalorieTol; //maximum calorie tolerance when game starts
    [FoldoutGroup("Internal Variables")] [SerializeField] float citizenMoveSpeed;
    [FoldoutGroup("Internal Variables")] [SerializeField] float intervalToRandomizeRotation;
    [FoldoutGroup("Internal Variables")] [SerializeField] float speedDecreaseMultiplier;
    float step;
    FoodData foodData;

    //Components
    Stopwatch timer = new Stopwatch();
    Rigidbody rb;
    Vector3 baseRotationVector;
    #endregion

    #region //BASE UNITY CALLBACKS
    // Use this for initialization
    void Start()
    {
        currentMaxCalorieTol = baseMaxCalorieTol;
        rb = GetComponent<Rigidbody>();
        intervalToRandomizeRotation = Random.Range(0f, 5f);
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
            Physics.IgnoreCollision(collision.gameObject.GetComponent<Collider>(), GetComponent<Collider>()); //ignore the collision between food and

            switch (foodData.FoodType)
            {
                case FoodType.greasy:
                    GreaseEffect(foodData.CalorieGainOnHit);
                    break;
                case FoodType.sweet:
                    SweetEffect(foodData.CalorieGainOnHit, foodData.CalorieToleranceDecrease);
                    break;
                case FoodType.energy:
                    EnergyEffect(foodData.CalorieGainOnHit, foodData.CalorieGainOverTime);
                    break;
                default:
                    break;
            }
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

        currentCalories += caloriesGained;
    }

    void CheckCitizenStatus()
    {
        if (currentCalories > currentMaxCalorieTol)
        {
            Dead();
        }
    }

    void Dead()
    {
        //Destroy(gameObject);
        //Destroy Colliders
        //Lock Animation on last frame
    }
    #endregion
}
