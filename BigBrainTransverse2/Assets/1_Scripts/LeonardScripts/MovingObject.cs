using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingObject : MonoBehaviour
{
    Vector3 position;
    public float speed;
    public Transform target;
    public Vector3 direction;
    public Rigidbody rb;


    private Quaternion quaternion;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //Movement();

        /*RaycastHit hit;
        // Does the ray intersect any objects excluding the player layer
        if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.forward), out hit, Mathf.Infinity, 1))
        {
            Debug.DrawRay(transform.position, transform.TransformDirection(Vector3.forward) * hit.distance, Color.yellow);
            Debug.Log("Did Hit");
        }
        else
        {
            Debug.DrawRay(transform.position, transform.TransformDirection(Vector3.forward) * 1000, Color.white);
            Debug.Log("Did not Hit");
        }*/
               
        float step = speed * Time.deltaTime; // calculate distance to move
        rb.MovePosition(transform.position + transform.forward * step);
    }

    void DetectWalls()
    {
        RaycastHit hit;
        Ray ray = new Ray(transform.position, transform.forward);

        if (Physics.Raycast(ray, out hit))
        {
            if (hit.collider != null)
            {
                Debug.DrawRay(transform.position, transform.TransformDirection(Vector3.forward) * hit.distance, Color.yellow);
                Debug.Log("Did Hit");
                MoveToWall(hit.collider.transform.position);
                //float step = speed * Time.deltaTime; // calculate distance to move
                //transform.position = Vector3.MoveTowards(transform.position, hit.collider.transform.position, step);
            }
        }
    }

    void MoveToWall(Vector3 targetPosition)
    {
        float step = speed * Time.deltaTime; // calculate distance to move
        Vector3 movement = new Vector3(Mathf.Floor(targetPosition.x) + speed * Time.deltaTime, 0, Mathf.Floor(targetPosition.y) + speed * Time.deltaTime);
        Debug.Log(movement);
        rb.MovePosition(transform.forward);
    }

    void OnCollisionEnter(Collision coll)
    {
        if(coll.gameObject.tag == "Walls")
        {
            quaternion = Random.rotation;
            Vector3 v = transform.rotation.eulerAngles;
            transform.rotation = Quaternion.Euler(v.x, Random.Range(-90, 180), v.z);
            Debug.Log("yeetus");
        }
    }
}
