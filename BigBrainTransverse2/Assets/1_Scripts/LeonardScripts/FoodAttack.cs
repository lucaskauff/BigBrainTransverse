using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FoodAttack : MonoBehaviour
{

    [SerializeField] int meshOrderInList;
    [SerializeField] float foodMoveSpeed;
    [SerializeField] FoodData foodData;
    Rigidbody rb;

    // Use this for initialization
    void Start()
    {
        Setup();
        rb = GetComponent<Rigidbody>();
    }

    void Setup()
    {

        foodData.FoodMesh = foodData.FoodMeshList[meshOrderInList];
        GetComponent<MeshFilter>().mesh = foodData.FoodMesh;
    }

    // Update is called once per frame
    void Update()
    {
        //MoveToPosition();
        if (Input.GetMouseButtonDown(0))
        {
            MoveToPosition();
        }
    }

    void MoveToPosition()
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
            Vector3 dir = hit.point - this.transform.position;
            Vector3 hitPointCoord = new Vector3(hit.point.x, hit.point.y, hit.point.z);

            //Now move your character in world space 
            //transform.Translate(dir * Time.deltaTime * foodMoveSpeed, Space.World);
            float step = foodMoveSpeed * Time.deltaTime; // calculate distance to move
            transform.position = Vector3.MoveTowards(transform.position, hitPointCoord, step);
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        //use to detect collision against wall and change the facing direction
        if (collision.gameObject.tag == "Citizens")
        {
            switch (foodData.FoodType)
            {
                case FoodType.greasy:
                    collision.gameObject.SendMessage("GreaseEffect");
                    break;
                case FoodType.sweet:
                    collision.gameObject.SendMessage("SweetEffect");
                    break;
                case FoodType.energy:
                    collision.gameObject.SendMessage("EnergyEffect");
                    break;
                default:
                    break;
            }
        }
    }
}