using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerKnob : MonoBehaviour
{
    //[HideInInspector]
    public bool isConnecting = false;

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