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
    [FoldoutGroup("Internal Components")] [SerializeField] Rigidbody rb;
    [FoldoutGroup("Internal Components")] [SerializeField] Renderer cubeRenderer;
    [FoldoutGroup("Internal Components")] [SerializeField] ParticleSystem particleSystem;
    Stopwatch timer = new Stopwatch();
    Vector3 baseRotationVector;
    Vector3 theCamera;

    [FoldoutGroup("Objects to serialize")] [SerializeField] TheBubble bubble;
    [FoldoutGroup("Objects to serialize")] [SerializeField] Transform reason;

    [FoldoutGroup("Debug Variables")] public bool isScientist;

    [FoldoutGroup("Debug Variables")] [SerializeField] float currentCalories;
    [FoldoutGroup("Debug Variables")] [SerializeField] float currentMaxCalorieTol; //maximum calorie tolerance at any given time
    bool isHitByEnergy = false;

    [FoldoutGroup("Internal Variables")] bool isDead = false;
    [FoldoutGroup("Internal Variables")] [SerializeField] float baseMaxCalorieTol; //maximum calorie tolerance when game starts
    [FoldoutGroup("Internal Variables")] [SerializeField] float citizenMoveSpeed;
    [FoldoutGroup("Internal Variables")] [SerializeField] float intervalToRandomizeRotation;
    [FoldoutGroup("Internal Variables")] [SerializeField] float speedDecreaseMultiplier;
    [FoldoutGroup("Internal Variables")] [SerializeField] float timeFreezedOnHit;
    float step;
    FoodData foodData;

    [FoldoutGroup("Food Hit Values")] [SerializeField] int greasy = 0;
    [FoldoutGroup("Food Hit Values")] [SerializeField] int sweet = 0;

    int pointsForPlayerIndex;
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
        
        //particleSystem.Stop();
    }

    // Update is called once per frame
    void Update()
    {
        if (isDead) return;

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

    void OnCollisionEnter(Collision collision)
    {
        //use to detect collision against wall and change the facing direction
        if (collision.gameObject.tag == "Walls")
        {
            RandomizeRotation();
        }

        if (collision.gameObject.tag == "Food")
        {
            switch (collision.gameObject.GetComponent<FoodBehavior>().foodData.FoodType)
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
            }

            pointsForPlayerIndex = collision.gameObject.GetComponent<FoodBehavior>().shotByPlayerIndex;
            if (currentCalories >= currentMaxCalorieTol)
            {
                Dead(pointsForPlayerIndex);
                return;
            }

            StartCoroutine(FreezeOnHit());
            myAnim.SetTrigger("Hit");
            
            //if (isScientist) UnityEngine.Debug.Log("You just hit a scientist");
            //AnimationManager.DeathCamManager(this.gameObject, "Scientist");
        }
    }

    /*void OnTriggerEnter(Collider collider)
    {
        if (collider.gameObject.tag == "Food")
        {
            myAnim.SetTrigger("Hit");

            switch (collision.gameObject.GetComponent<FoodBehavior>().foodData.FoodType)
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
            }

            if(isScientist) UnityEngine.Debug.Log("You just hit a scientist");//AnimationManager.DeathCamManager(this.gameObject, "Scientist");
        }
    }*/

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
        step = citizenMoveSpeed * Time.deltaTime; // calculate distance to move
        rb.MovePosition(transform.position + transform.forward * step);
    }

    void RandomizeRotation()
    {
        baseRotationVector = transform.rotation.eulerAngles;
        //transform.rotation = Quaternion.Euler(baseRotationVector.x, Random.Range(210, 330), baseRotationVector.z);
        
        transform.rotation = Quaternion.Euler(baseRotationVector.x, Random.Range(120, 240), baseRotationVector.z);
        //intervalToRandomizeRotation = Random.Range(0f, 1f);
        intervalToRandomizeRotation = 2f;
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

    /*
    void CheckCitizenStatus()
    {
        if (currentCalories >= currentMaxCalorieTol)
        {
            Dead();
        }
    }
    */

    void Dead(int playerIndex)
    {
        isDead = true;

        gameManager.peopleKilled[playerIndex] += 1;

        SpawnManager.citizensInScene.Remove(gameObject);
        myCol.enabled = false;
        rb.constraints = RigidbodyConstraints.FreezeAll;
        
        if (greasy > sweet)
        {
            myAnim.SetInteger("DeadType", 1);
        }
        else if (greasy < sweet)
        {
            myAnim.SetInteger("DeadType", 2);
        }            
        else if (isHitByEnergy)
        {
            myAnim.SetInteger("DeadType", 3);
        }

        if (!gameManager.isDeathAnimOnGoing)
        {
            bubble.transform.LookAt(theCamera);
            reason.LookAt(theCamera);
            reason.localPosition = new Vector3(reason.localPosition.x, reason.localPosition.y, reason.localPosition.z+0.01f);
            if (greasy > sweet) bubble.whichDeath = 0;
            else if (greasy < sweet) bubble.whichDeath = 1;
            else if(isHitByEnergy) bubble.whichDeath = 2;
            bubble.GetComponent<Animator>().SetTrigger("FadeIn");
        }
    }
    #endregion

    IEnumerator FreezeOnHit()
    {
        rb.constraints = RigidbodyConstraints.FreezeAll;
        yield return new WaitForSeconds(timeFreezedOnHit);
        rb.constraints = RigidbodyConstraints.None;
        rb.constraints = RigidbodyConstraints.FreezePositionY 
            | RigidbodyConstraints.FreezeRotationX
            | RigidbodyConstraints.FreezeRotationZ;
        StopCoroutine(FreezeOnHit());
    }
}