using System.Collections;
using System.Collections.Generic;
using UnityEngine;

class FoodAttack : MonoBehaviour
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
        if (Input.GetKeyDown(KeyCode.Space))
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

            Vector3 hitPointCoord = new Vector3(hit.point.x, hit.point.y, hit.point.z);

            rb.useGravity = true;
            rb.AddForce(hitPointCoord, ForceMode.Impulse);
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