using Sirenix.OdinInspector;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;


class FoodBehavior : MonoBehaviour
{
    [SerializeField] FoodData foodData;
    [FoldoutGroup("Food Variables")] [SerializeField] float foodMoveSpeed;
    [FoldoutGroup("Internal Variables")] [SerializeField] float timeToSelfDestruct;

    Rigidbody rb;
    Vector3 hitPointCoord;

    Stopwatch timer = new Stopwatch();

    // Use this for initialization
    void Start()
    {

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
            collision.gameObject.SendMessage("WhichFoodType", foodData);
            Destroy(this.gameObject);
        }

        if (collision.gameObject.layer == LayerMask.NameToLayer("BoundsLayer")) Destroy(this.gameObject);
    }
}