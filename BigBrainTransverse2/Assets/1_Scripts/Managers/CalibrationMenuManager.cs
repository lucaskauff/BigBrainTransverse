using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CalibrationMenuManager : MonoBehaviour
{
    //GameManager
    SceneLoader sceneLoader;

    [Header("Objects to serialize")]
    [SerializeField] GameObject[] playerKnobs;

    [SerializeField] RawImage realtimeCam;
    [SerializeField] RectTransform tvButton;
    [SerializeField] Text tvButtonText;
    [SerializeField] SuperTextMesh[] playerStatus;

    [SerializeField] GameObject connectingBox;

    [Header("Serializable variables")]
    [SerializeField] KinectWrapper.NuiSkeletonPositionIndex[] TrackedJoints;
    [SerializeField] float smoothFactor = 5f;
    [SerializeField] float[] rangeMultipliers;

    [SerializeField] float pointT = 0.5f;
    [SerializeField] float[] buttonYPositions;

    [SerializeField] float connectingT = 3;
    [SerializeField] string nextSceneName;

    //Private
    int[] iJointIndexes;
    private float distanceToCamera = 10f;

    string[] playerStatusTexts;
    bool arePointsRunning = false;

    bool playersAreConnecting = false;

    private void Start()
    {
        sceneLoader = NewGameManager.Instance.sceneLoader;

        distanceToCamera = (playerKnobs[0].transform.position - Camera.main.transform.position).magnitude;

        iJointIndexes = new int[2];
        playerStatusTexts = new string[playerStatus.Length];
    }

    private void Update()
    {
        KinectManager kinectManager = KinectManager.Instance;

        if (kinectManager && kinectManager.IsInitialized())
        {
            RenderRealtimeCam(kinectManager);

            for (int i = 0; i < iJointIndexes.Length; i++)
            {
                iJointIndexes[i] = (int)TrackedJoints[i];
            }

            //PlayerSide1
            if (kinectManager.IsPlayerCalibrated(kinectManager.Player1ID))
            {
                uint userId;
                userId = kinectManager.GetPlayer1ID();

                for (int i = 0; i < iJointIndexes.Length; i++)
                {
                    if (kinectManager.IsJointTracked(userId, iJointIndexes[i]))
                    {
                        Vector3 posJoint = kinectManager.GetRawSkeletonJointPos(userId, iJointIndexes[i]);

                        if (posJoint != Vector3.zero)
                        {
                            Vector2 posDepth = kinectManager.GetDepthMapPosForJointPos(posJoint);
                            //Debug.Log(posDepth);
                            
                            Vector2 posColor = kinectManager.GetColorMapPosForDepthPos(posDepth);
                            //Debug.Log(posDepth);

                            float scaleX = posColor.x / KinectWrapper.Constants.ColorImageWidth;
                            float scaleY = 1.0f - posColor.y / KinectWrapper.Constants.ColorImageHeight;

                            if (playerKnobs[i])
                            {
                                /*
                                Vector2 vPosOverlay = new Vector2(scaleX, scaleY);
                                Vector2 vPosOverlay = new Vector2((scaleX - 0.5f) * rangeMultipliers[0], (scaleY - 0.5f) * rangeMultipliers[1]);
                                Debug.Log(vPosOverlay);
                                playerKnobs[i].anchoredPosition = Vector2.Lerp(playerKnobs[i].anchoredPosition, vPosOverlay, smoothFactor * Time.deltaTime);
                                */

                                Vector3 vPosOverlay = Camera.main.ViewportToWorldPoint(new Vector3(scaleX, scaleY, distanceToCamera));
                                playerKnobs[i].transform.position = Vector3.Lerp(playerKnobs[i].transform.position, vPosOverlay, smoothFactor * Time.deltaTime);
                            }                            
                        }
                    }
                }

                StopCoroutine(ThreePoints(0));
                playerStatusTexts[0] = "Calibrated !";
            }
            else if (!arePointsRunning)
            {
                StartCoroutine(ThreePoints(0));
            }

            //PlayerSide2
            /*
            if (kinectManager.IsPlayerCalibrated(kinectManager.Player2ID))
            {
                uint userId;
                userId = kinectManager.GetPlayer2ID();

                StopCoroutine(ThreePoints(1));
                playerStatusTexts[1] = "Calibrated !";
            }
            else if (!arePointsRunning)
            {
                StartCoroutine(ThreePoints(1));
            }
            */

            if (kinectManager.IsPlayerCalibrated(kinectManager.Player1ID)
                /*&& kinectManager.IsPlayerCalibrated(kinectManager.Player2ID)*/)
            {

            }
        }

        for (int i = 0; i < playerStatus.Length; i++)
        {
            playerStatus[i].text = "Player " + (i+1) + " Status : " + playerStatusTexts[i];
        }
    }

    void RenderRealtimeCam(KinectManager man)
    {
        if (realtimeCam && realtimeCam.texture == null)
        {
            realtimeCam.texture = man.GetUsersClrTex();
        }

        if (realtimeCam.enabled)
        {
            tvButton.localPosition = new Vector2(tvButton.localPosition.x, buttonYPositions[0]);
            tvButtonText.text = "Turn off camera";
        }
        else
        {
            tvButton.localPosition = new Vector2(tvButton.localPosition.x, buttonYPositions[1]);
            tvButtonText.text = "Turn on camera";
        }
    }

    public void SwitchRCam()
    {
        realtimeCam.enabled = !realtimeCam.enabled;
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

    IEnumerator Connecting()
    {
        playersAreConnecting = true;
        yield return new WaitForSeconds(connectingT);
        //sceneLoader.ChangeScene(nextSceneName);
    }
}