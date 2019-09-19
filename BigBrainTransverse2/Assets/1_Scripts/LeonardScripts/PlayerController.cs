using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{

    [SerializeField] Transform spawnPoint;
    [SerializeField] GameObject food;

    [SerializeField] int equipedItemNumber;
    GameObject cloneProj;
    [SerializeField] public static FoodData equipedFood;
    [SerializeField] List<FoodData> foodObjectsList = new List<FoodData>();

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (Input.GetMouseButtonDown(0))
        {
            cloneProj = Instantiate(food, spawnPoint.position, Quaternion.identity);
            cloneProj.gameObject.SendMessage("MoveToPosition");
        }
    }

    void Update()
    {
        //Debug.Log(Input.GetAxis("Mouse ScrollWheel"));

        if (Input.GetAxis("Mouse ScrollWheel") > 0f)
        {
            equipedItemNumber++;
            equipedFood = foodObjectsList[equipedItemNumber];
            // scroll up
        }

        else if (Input.GetAxis("Mouse ScrollWheel") < 0f)
        {
            equipedItemNumber--;
            equipedFood = foodObjectsList[equipedItemNumber];
            // scroll down
        }

        if(equipedItemNumber > 3 || equipedItemNumber < 0)
        {
            equipedItemNumber = 0;
        }
    }
}
