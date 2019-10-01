using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RetourCamera : MonoBehaviour
{
    //Managers
    NewInputManager inputManager;
    KinectManager kinectManager;

    [Header("Components to serialize")]
    [SerializeField] RawImage realtimeCam;

    private void Start()
    {
        /*
        inputManager = NewGameManager.Instance.inputManager;
        */

        inputManager = FindObjectOfType<NewInputManager>();

    }

    private void Update()
    {
        kinectManager = KinectManager.Instance;

        if (kinectManager && kinectManager.IsInitialized())
        {
            RenderRealtimeCamera(kinectManager);
        }

        if (inputManager.switchCameraOnOff)
        {
            SwitchCamera();
        }
    }

    void RenderRealtimeCamera(KinectManager man)
    {
        if (realtimeCam && realtimeCam.texture == null)
        {
            realtimeCam.texture = man.GetUsersClrTex();
        }
    }

    void SwitchCamera()
    {
        realtimeCam.enabled = !realtimeCam.enabled;
    }
}