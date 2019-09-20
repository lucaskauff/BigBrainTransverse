using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CalibrationMenuManager : MonoBehaviour
{
    [SerializeField] SuperTextMesh[] playerStatus;

    private void Update()
    {
        KinectManager kinectManager = KinectManager.Instance;

        if (kinectManager && kinectManager.IsInitialized())
        {
            if (kinectManager.IsUserDetected())
            {
                if (kinectManager.IsPlayerCalibrated(kinectManager.Player1ID))
                {
                    playerStatus[0].text = "Player 1 Status : Connected !";
                }
                else
                {
                    playerStatus[0].text = "Player 1 Status : Searching for player...";
                }
            }
        }
    }
}