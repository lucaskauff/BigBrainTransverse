using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerKnob : MonoBehaviour
{
    //GameManager
    NewInputManager inputManager;

    [Header("Serializable variables")]
    [SerializeField] bool isCursor1;

    //[HideInInspector]
    public bool isConnecting = false;

    private void Start()
    {
        inputManager = NewGameManager.Instance.inputManager;
    }

    private void Update()
    {
        UpdatePosition();
    }

    void UpdatePosition()
    {
        if (isCursor1)
            transform.position = Vector3.Lerp(transform.position, inputManager.cursor1Pos, inputManager.smoothFactor * Time.deltaTime);
        else
            transform.position = Vector3.Lerp(transform.position, inputManager.cursor2Pos, inputManager.smoothFactor * Time.deltaTime);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.name == "ConnectionBox")
        {
            isConnecting = ChangeConnectionStatus();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.name == "ConnectionBox")
        {
            isConnecting = ChangeConnectionStatus();
        }
    }

    bool ChangeConnectionStatus()
    {
        return !isConnecting;
    }
}