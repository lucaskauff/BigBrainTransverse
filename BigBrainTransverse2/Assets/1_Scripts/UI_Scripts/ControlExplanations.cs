using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControlExplanations : MonoBehaviour
{
    [SerializeField] ExplanationSceneMan sceneManager;

    void ControlExplanationDone()
    {
        sceneManager.ShowTheGuys();
    }
}