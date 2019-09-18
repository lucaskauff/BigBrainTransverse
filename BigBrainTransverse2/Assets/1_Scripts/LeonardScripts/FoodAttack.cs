using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FoodAttack : MonoBehaviour {

    bool isGreasy;
    bool isSweet;
    bool isEnergy;

    [SerializeField]
    int meshOrderInList;
    [SerializeField]
    float foodMoveSpeed;
    Rigidbody rigidbody;

    // Use this for initialization
    void Start ()
    {
        Setup();
        rigidbody = GetComponent<Rigidbody>();
    }
	
	// Update is called once per frame
	void Update ()
    {
        //Yeet();
        if (Input.GetMouseButtonDown(0))
        {
            // Cast a ray from screen point
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            // Save the info
            RaycastHit hit;
            // You successfully hit
            if (Physics.Raycast(ray, out hit))
            {
                Debug.Log(hit.collider.name + " says YOOOOOOOOOOOOOOOOO");
                //Find the direction to move in
                Vector3 dir = hit.point - transform.position;

                //Now move your character in world space 
                //transform.Translate(dir * Time.deltaTime * foodMoveSpeed, Space.World);

                float step = foodMoveSpeed * Time.deltaTime; // calculate distance to move
                rigidbody.MovePosition(dir * step);
            }
        }
    }

    [SerializeField]
    private FoodData foodData; // 1

    /*private void Yeet() // 2
    {
        Debug.Log(foodData.FoodType); // 3
        Debug.Log(foodData.FoodName); // 3
        Debug.Log(foodData.FoodMeshList.Count); // 3
        Debug.Log(foodData.foodMesh.name); // 3
        Debug.Log(foodData.GoldCost); // 3
        Debug.Log(foodData.AttackDamage); // 3
    }*/

    void Setup()
    {
        switch(foodData.FoodType)
        {
            case FoodType.greasy:
                isGreasy = true;
                isSweet = false;
                isEnergy = false;
                break;
            case FoodType.sweet:
                isGreasy = false;
                isSweet = true;
                isEnergy = false;
                break;
            case FoodType.energy:
                isGreasy = false;
                isSweet = false;
                isEnergy = true;
                break;
            default:
                break;
        }

        foodData.FoodMesh = foodData.FoodMeshList[meshOrderInList];
        GetComponent<MeshFilter>().mesh = foodData.FoodMesh;
    }
}
