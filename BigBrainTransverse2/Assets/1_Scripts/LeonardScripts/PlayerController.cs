using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using Sirenix.Serialization;
using System.Diagnostics;

public class PlayerController : MonoBehaviour
{
    [SerializeField] Transform spawnPoint;
    [SerializeField] GameObject food;
    GameObject cloneProj;
    [ShowInInspector] public static bool isGreasy = true;
    [ShowInInspector] public static bool isSweet = false;
    [ShowInInspector] public static bool isEnergy = false;

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (Input.GetMouseButtonDown(0))
        {
            cloneProj = Instantiate(food, spawnPoint.position, Quaternion.identity);
            cloneProj.gameObject.SendMessage("MoveToPosition");
        }
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            isGreasy = true;
            isSweet = false;
            isEnergy = false;
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            isGreasy = false;
            isSweet = true;
            isEnergy = false;
        }
    }
}
