using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMenuController : MonoBehaviour
{
    //GameManager
    NewInputManager inputManager;

    [SerializeField] bool isPlayerOne;

    public bool isConnecting;
    public bool hasSelected = false;

    float distanceToCam;

    void Start()
    {
        //inputManager = NewGameManager.Instance.inputManager;
        inputManager = FindObjectOfType<NewInputManager>();

        distanceToCam = (transform.position - Camera.main.transform.position).magnitude;
    }

    void Update()
    {
        if (isPlayerOne)
            transform.position = Vector3.Lerp(transform.position, 
                new Vector3(inputManager.cursor1Pos.x, inputManager.cursor1Pos.y, distanceToCam),
                inputManager.smoothFactor * Time.deltaTime);
        else
            transform.position = Vector3.Lerp(transform.position,
                new Vector3(inputManager.cursor2Pos.x, inputManager.cursor2Pos.y, distanceToCam), 
                inputManager.smoothFactor * Time.deltaTime);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "ConnectionBox" && !hasSelected)
        {
            other.gameObject.GetComponent<ConnectionBoxes>().connecting = true;
            if (isPlayerOne)
                other.gameObject.GetComponent<ConnectionBoxes>().selectedBy = 1;
            else
                other.gameObject.GetComponent<ConnectionBoxes>().selectedBy = 2;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "ConnectionBox" && !hasSelected)
        {
            other.gameObject.GetComponent<ConnectionBoxes>().connecting = false;
            other.gameObject.GetComponent<ConnectionBoxes>().selectedBy = 0;
        }
    }
}