using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using Sirenix.Serialization;
using System.Diagnostics;

public class CitizenController : MonoBehaviour {

    [SerializeField] float citizenLifePoints;
    [SerializeField] float citizenSpeed;
    [SerializeField] float intervalToRandomizeRotation;
    [SerializeField] float lifepoints;

    float step;
    Stopwatch timer = new Stopwatch();
    Rigidbody rb;
    Vector3 baseRotationVector;

    // Use this for initialization
    void Start()
    {
        rb = GetComponent<Rigidbody>();
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

        //if (lifepoints <= 0) Dead();
    }

    void Movement()
    {
        step = citizenSpeed * Time.deltaTime; // calculate distance to move
        rb.MovePosition(transform.position + transform.forward * step);
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

        if(collision.gameObject.tag == "Food")
        {
            Physics.IgnoreCollision(collision.gameObject.GetComponent<Collider>(), GetComponent<Collider>()); //ignore the collision between food and
            Dead();
        }

        //use to ignore collisions between citizens
        //if (collision.gameObject.tag == "Citizens") Physics.IgnoreCollision(collision.gameObject.GetComponent<Collider>(), GetComponent<Collider>());
    }
    
    void GreaseEffect()
    {
        UnityEngine.Debug.Log("i've been triggerd by your greasy-assed croissant, nigg");
    }

    void SweetEffect()
    {
        UnityEngine.Debug.Log("i've been triggerd by your sweet mothafuckin croissant, nigg");
    }

    void EnergyEffect()
    {
        UnityEngine.Debug.Log("FUCK NIGG, i've been triggerd by your croissant");
    }

    void Dead()
    {
        Destroy(gameObject);
    }
}
