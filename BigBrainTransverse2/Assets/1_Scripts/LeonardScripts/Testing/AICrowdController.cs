using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AICrowdController : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    /*public static RaycastHit RayCastManger(Vector3 startPosition, Vector3 targetPosition)
    {
        RaycastHit hit;
        Ray ray = new Ray(startPosition, targetPosition);

        if (Physics.Raycast(ray, out hit))
        {
            if (hit.collider != null)
            {
                Debug.DrawRay(transform.position, transform.TransformDirection(Vector3.forward) * hit.distance, Color.yellow);
                Debug.Log("Did Hit");
                float step = speed * Time.deltaTime; // calculate distance to move
                transform.position = Vector3.MoveTowards(transform.position, hit.collider.transform.position, step);
            }
        }
    }*/
}
