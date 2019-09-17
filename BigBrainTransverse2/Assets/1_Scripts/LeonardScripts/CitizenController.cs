using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using Sirenix.Serialization;

public class CitizenController : MonoBehaviour {

    float step;
    [SerializeField] float citizenSpeed;
    [SerializeField] Rigidbody rb;

    // Use this for initialization
    void Start ()
    {
		
	}
	
	// Update is called once per frame
	void Update ()
    {
        Movement();
    }

    void Movement()
    {
        step = citizenSpeed * Time.deltaTime; // calculate distance to move
        rb.MovePosition(transform.position + transform.forward * step);
    }

    void OnCollisionEnter(Collision collision)
    {
        //use to detect collision against wall and change the facing direction
        if (collision.gameObject.tag == "Walls")
        {
            Vector3 baseRotationVector = transform.rotation.eulerAngles;
            transform.rotation = Quaternion.Euler(baseRotationVector.x, Random.Range(-360, 360), baseRotationVector.z);
        }

        //use to ignore collisions between citizens
        //if (collision.gameObject.tag == "Citizens") Physics.IgnoreCollision(collision.gameObject.GetComponent<Collider>(), GetComponent<Collider>());
    }
}
