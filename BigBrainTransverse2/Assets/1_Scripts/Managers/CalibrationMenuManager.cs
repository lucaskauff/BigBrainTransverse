using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CalibrationMenuManager : MonoBehaviour
{
    //GameManager
    SceneLoader sceneLoader;

    [Header("Objects to serialize")]
    [SerializeField] PlayerKnob[] playerKnobs;

    [SerializeField] RawImage realtimeCam;
    [SerializeField] RectTransform tvButton;
    [SerializeField] Text tvButtonText;
    [SerializeField] SuperTextMesh[] playerStatus;

    [SerializeField] Image connectingGauge;

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
    bool[] arePointsRunning;

    bool playersAreConnecting = false;
    float startFillT = 0;
    float connectionFill = 0;

    private void Start()
    {
        sceneLoader = NewGameManager.Instance.sceneLoader;

        distanceToCamera = (playerKnobs[0].transform.position - Camera.main.transform.position).magnitude;

        iJointIndexes = new int[playerStatus.Length]; //=2
        playerStatusTexts = new string[playerStatus.Length]; //=2
        arePointsRunning = new bool[playerStatus.Length]; //=2
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
                                Vector3 vPosOverlay = Camera.main.ViewportToWorldPoint(new Vector3(scaleX, scaleY, distanceToCamera));
                                playerKnobs[i].transform.position = Vector3.Lerp(playerKnobs[i].transform.position, vPosOverlay, smoothFactor * Time.deltaTime);
                            }                            
                        }
                    }
                }

                StopCoroutine(ThreePoints(0));
                playerStatusTexts[0] = "Calibrated !";
            }
            else if (!arePointsRunning[0])
            {
                StartCoroutine(ThreePoints(0));
            }

            //PlayerSide2
            if (kinectManager.IsPlayerCalibrated(kinectManager.Player2ID))
            {
                uint userId;
                userId = kinectManager.GetPlayer2ID();

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

                            if (playerKnobs[i+2])
                            {
                                Vector3 vPosOverlay = Camera.main.ViewportToWorldPoint(new Vector3(scaleX, scaleY, distanceToCamera));
                                playerKnobs[i+2].transform.position = Vector3.Lerp(playerKnobs[i+2].transform.position, vPosOverlay, smoothFactor * Time.deltaTime);
                            }
                        }
                    }
                }

                StopCoroutine(ThreePoints(1));
                playerStatusTexts[1] = "Calibrated !";
            }
            else if (!arePointsRunning[1])
            {
                StartCoroutine(ThreePoints(1));
            }

            for (int i = 0; i < playerStatus.Length; i++)
            {
                playerStatus[i].text = "Player " + (i + 1) + " Status : " + playerStatusTexts[i];
            }

            if (kinectManager.IsPlayerCalibrated(kinectManager.Player1ID)
                /*&& kinectManager.IsPlayerCalibrated(kinectManager.Player2ID)*/
                && (playerKnobs[0].isConnecting || playerKnobs[1].isConnecting)
                /*&& (playerKnobs[2].isConnecting || playerKnobs[3].isConnecting)*/)
            {
                if (!playersAreConnecting)
                {
                    startFillT = Time.time;
                    StartCoroutine(Connecting());
                    playersAreConnecting = true;
                }
                else
                {
                    connectionFill = Time.time - startFillT;
                }
            }
            else
            {
                //StopCoroutine(Connecting());
                StopAllCoroutines();
                connectionFill = 0;
                playersAreConnecting = false;
            }

            UpdateConnectionUI();
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

    void UpdateConnectionUI()
    {
        connectingGauge.fillAmount = connectionFill / connectingT;
    }

    public void SwitchRCam()
    {
        realtimeCam.enabled = !realtimeCam.enabled;
    }

    IEnumerator ThreePoints(int playerInt)
    {
        arePointsRunning[playerInt] = true;
        playerStatusTexts[playerInt] = "Searching for player";
        yield return new WaitForSeconds(pointT);
        playerStatusTexts[playerInt] = "Searching for player.";
        yield return new WaitForSeconds(pointT);
        playerStatusTexts[playerInt] = "Searching for player..";
        yield return new WaitForSeconds(pointT);
        playerStatusTexts[playerInt] = "Searching for player...";
        yield return new WaitForSeconds(pointT);
        arePointsRunning[playerInt] = false;
    }

    IEnumerator Connecting()
    {
        yield return new WaitForSeconds(connectingT);
        sceneLoader.ChangeScene(nextSceneName);
        //Debug.Log("NEXT SCENE");
    }
}