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
        inputManager = NewGameManager.Instance.inputManager;
        kinectManager = KinectManager.Instance;
    }

    private void Update()
    {
        RenderRealtimeCamera(kinectManager);

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