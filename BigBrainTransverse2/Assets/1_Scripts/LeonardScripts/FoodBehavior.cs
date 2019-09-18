using System.Collections;
using System.Collections.Generic;
using UnityEngine;

class FoodBehavior : MonoBehaviour
{
    [SerializeField] int meshOrderInList;
    [SerializeField] float foodMoveSpeed;
    [SerializeField] FoodData foodData;
    Rigidbody rb;
    Vector3 hitPointCoord;

    // Use this for initialization
    void Start()
    {
        Setup();
    }

    void Setup()
    {
        foodData.FoodMesh = foodData.FoodMeshList[meshOrderInList];
        GetComponent<MeshFilter>().mesh = foodData.FoodMesh;
    }

    // Update is called once per frame
    void Update()
    {

    }

    void MoveToPosition()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit))
        {
            hitPointCoord = new Vector3(hit.point.x, hit.point.y, hit.point.z);

            rb = GetComponent<Rigidbody>();
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

        if(collision.gameObject.tag == "Floor") Destroy(this.gameObject);
        if (collision.gameObject.tag == "Food") Physics.IgnoreCollision(collision.gameObject.GetComponent<Collider>(), GetComponent<Collider>());
    }
}