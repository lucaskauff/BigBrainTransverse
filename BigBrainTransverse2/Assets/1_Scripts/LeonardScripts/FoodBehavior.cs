using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using Sirenix.Serialization;
using System.Diagnostics;


class FoodBehavior : MonoBehaviour
{
    [SerializeField] int meshOrderInList;
    [SerializeField] float foodMoveSpeed;
    [SerializeField] FoodData foodData;

    Rigidbody rb;
    Vector3 hitPointCoord;

    [SerializeField] float timeToSelfDestruct;
    Stopwatch timer = new Stopwatch();

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
        SelfDestruct();
    }

    void MoveToPosition()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit) && hit.collider.gameObject.layer == LayerMask.NameToLayer("BoundsLayer"))
        {
            hitPointCoord = new Vector3(hit.point.x, hit.point.y, hit.point.z);

            rb = GetComponent<Rigidbody>();
            rb.useGravity = true;
            Vector3 dir = hitPointCoord - transform.position;
            rb.AddForce(dir * foodMoveSpeed, ForceMode.Impulse);
        }
    }

    void SelfDestruct()
    {
        timer.Start();

        if (timer.Elapsed.TotalSeconds >= timeToSelfDestruct)
        {
            timer.Stop();
            Destroy(gameObject);
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
            Destroy(this.gameObject);
        }
        
        if (collision.gameObject.layer == LayerMask.NameToLayer("BoundsLayer")) Destroy(this.gameObject);
    }
}