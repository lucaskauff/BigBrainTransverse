using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using Sirenix.Serialization;
using System.Diagnostics;

public class CitizenController : MonoBehaviour {

    [SerializeField] float citizenSpeed;
    [SerializeField] float intervalToRandomizeRotation;

    float step;
    Stopwatch timer = new Stopwatch();
    Rigidbody rigidbody;
    Vector3 baseRotationVector;

    // Use this for initialization
    void Start()
    {
        rigidbody = GetComponent<Rigidbody>();
        intervalToRandomizeRotation = Random.Range(0f, 5f);
    }

    // Update is called once per frame
    void Update()
    {
        Movement();

        timer.Start();

        if (timer.Elapsed.TotalSeconds >= intervalToRandomizeRotation)
        {
            RandomizeRotation();
            timer.Stop();
            timer.Reset();
        }
    }

    void Movement()
    {
        step = citizenSpeed * Time.deltaTime; // calculate distance to move
        rigidbody.MovePosition(transform.position + transform.forward * step);
    }

    void RandomizeRotation()
    {
        baseRotationVector = transform.rotation.eulerAngles;
        transform.rotation = Quaternion.Euler(baseRotationVector.x, Random.Range(-180, 180), baseRotationVector.z);
        intervalToRandomizeRotation = Random.Range(0f, 1f);
    }

    void OnCollisionEnter(Collision collision)
    {
        //use to detect collision against wall and change the facing direction
        if (collision.gameObject.tag == "Walls")
        {
            RandomizeRotation();
        }

        //use to ignore collisions between citizens
        //if (collision.gameObject.tag == "Citizens") Physics.IgnoreCollision(collision.gameObject.GetComponent<Collider>(), GetComponent<Collider>());
    }
}
