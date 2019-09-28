using Sirenix.OdinInspector;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

class FoodBehavior : MonoBehaviour
{
    [SerializeField] FoodData foodData;
    [FoldoutGroup("Food Variables")] [SerializeField] float foodMoveSpeed;
    [FoldoutGroup("Internal Variables")][SerializeField] float timeToSelfDestruct;

    Rigidbody rb;
    Vector3 hitPointCoord;

    Stopwatch timer = new Stopwatch();

    void Update()
    {
        SelfDestruct();
    }

    void MoveToPosition(Vector3 rayOrigin)
    {
        Vector3 relative = new Vector3(0, -1, 1);
        //Vector3 relative = transform.InverseTransformDirection(Vector3.forward);
        Ray ray = new Ray(transform.position, relative);

        //Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        RaycastHit hit;

        if (Physics.Raycast(ray, out hit) && hit.collider.gameObject.layer == LayerMask.NameToLayer("BoundsLayer"))
        {
            hitPointCoord = new Vector3(hit.point.x, hit.point.y, hit.point.z);
            //UnityEngine.Debug.Log(hitPointCoord);

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
        }

        //if (collision.gameObject.layer == LayerMask.NameToLayer("BoundsLayer")) Destroy(this.gameObject);
    }
}