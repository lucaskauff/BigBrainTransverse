using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CalibrationMenuManager : MonoBehaviour
{
    [Header("Objects to serialize")]
    [SerializeField] RawImage realtimeCam;
    [SerializeField] RectTransform tvButton;
    [SerializeField] Text tvButtonText;
    [SerializeField] SuperTextMesh[] playerStatus;

    [Header("Serializable variables")]
    [SerializeField] float pointT = 0.5f;
    [SerializeField] Vector2 buttonYPositions;

    //Private
    string[] playerStatusTexts;
    bool arePointsRunning = false;

    private void Start()
    {
        playerStatusTexts = new string[playerStatus.Length];
    }

    private void Update()
    {
        KinectManager kinectManager = KinectManager.Instance;

        if (kinectManager && kinectManager.IsInitialized())
        {
            RenderRealtimeCam(kinectManager);

            if (kinectManager.IsPlayerCalibrated(kinectManager.Player1ID))
            {
                StopAllCoroutines();
                playerStatusTexts[0] = "Calibrated !";
            }
            else if (!arePointsRunning)
            {
                StartCoroutine(ThreePoints(0));
            }
        }

        for (int i = 0; i < playerStatus.Length; i++)
        {
            playerStatus[i].text = "<j>Player " + (i+1) + " Status : " + playerStatusTexts[i];
        }


    }

    void RenderRealtimeCam(KinectManager man)
    {
        if (realtimeCam && realtimeCam.texture == null)
        {
            realtimeCam.texture = man.GetUsersClrTex();
        }
    }

    public void SwitchRCam()
    {
        realtimeCam.enabled = !realtimeCam.enabled;

        if (realtimeCam.enabled)
        {
            tvButton.localPosition = new Vector2(tvButton.localPosition.x, buttonYPositions[0]);
        }
        else
        {
            tvButton.localPosition = new Vector2(tvButton.localPosition.x, buttonYPositions[0]);
        }
    }

    IEnumerator ThreePoints(int playerInt)
    {
        arePointsRunning = true;
        playerStatusTexts[playerInt] = "Searching for player";
        yield return new WaitForSeconds(pointT);
        playerStatusTexts[playerInt] = "Searching for player.";
        yield return new WaitForSeconds(pointT);
        playerStatusTexts[playerInt] = "Searching for player..";
        yield return new WaitForSeconds(pointT);
        playerStatusTexts[playerInt] = "Searching for player...";
        yield return new WaitForSeconds(pointT);
        arePointsRunning = false;
    }
}