using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMenuController : MonoBehaviour
{
    //GameManager
    NewInputManager inputManager;

    [SerializeField] bool isPlayerOne;

    void Start()
    {
        inputManager = NewGameManager.Instance.inputManager;
    }

    void Update ()
    {
        if (isPlayerOne)
            transform.position = Vector3.Lerp(transform.position, inputManager.cursor1Pos, inputManager.smoothFactor * Time.deltaTime);
        else
            transform.position = Vector3.Lerp(transform.position, inputManager.cursor2Pos, inputManager.smoothFactor * Time.deltaTime);
    }
}