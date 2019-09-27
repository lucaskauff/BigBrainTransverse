using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewInputManager : MonoBehaviour
{
    [Header("Public variables")]
    public float smoothFactor = 60f;

    public KinectWrapper.NuiSkeletonPositionIndex gestureJoint;

    [Header("Serializable variables")]
    [SerializeField] KinectWrapper.NuiSkeletonPositionIndex[] TrackedJoints;

    //Private
    KinectManager kinectManager;
    GestureListener gestureListener;

    int[] iJointIndexes;
    PlayerController playerKnobExample;
    public float distanceToCamera;

    //Kinect inputs
    //Player 1
    public Vector3 cursor1Pos;
    public bool swipeLeftP1;
    public bool swipeRightP1;
    public bool swipeDownP1;

    //Player 2
    public Vector3 cursor2Pos;
    public bool swipeLeftP2;
    public bool swipeRightP2;
    public bool swipeDownP2;

    //Mouse clicks
    public bool mouseLeftClick;
    public bool mouseWheelClick;
    public bool mouseRightClick;

    //Keyboard keys
    public bool weaponMinusKeyP1;
    public bool weaponPlusKeyP1;
    public bool weaponMinusKeyP2;
    public bool weaponPlusKeyP2;
    public bool switchCameraOnOff;

    void Start()
    {
        kinectManager = KinectManager.Instance;
        gestureListener = FindObjectOfType<GestureListener>();

        iJointIndexes = new int[TrackedJoints.Length];
        playerKnobExample = FindObjectOfType<PlayerController>();

        distanceToCamera = (playerKnobExample.transform.position - Camera.main.transform.position).magnitude;
    }

    void Update ()
    {
        //KinectInputs
        if (kinectManager && kinectManager.IsInitialized() && kinectManager.IsUserDetected())
        {            
            for (int i = 0; i < iJointIndexes.Length; i++)
            {
                iJointIndexes[i] = (int)TrackedJoints[i];
            }

            Player1JointsPositions(true);
            Player1JointsPositions(false);

            if (gestureListener)
            {
                swipeLeftP1 = gestureListener.IsSwipeLeftP1();
                swipeRightP1 = gestureListener.IsSwipeRightP1();
                swipeDownP1 = gestureListener.IsSwipeDownP1();

                swipeLeftP2 = gestureListener.IsSwipeLeftP2();
                swipeRightP2 = gestureListener.IsSwipeRightP2();
                swipeDownP2 = gestureListener.IsSwipeDownP2();
            }
        }

        //Mouse clicks
        mouseLeftClick = Input.GetMouseButton(0);
        mouseRightClick = Input.GetMouseButton(1); ;
        mouseWheelClick = Input.GetMouseButton(2);

        //Keyboard keys
        switchCameraOnOff = Input.GetKeyDown(KeyCode.C);
        weaponMinusKeyP1 = Input.GetKeyDown(KeyCode.Alpha1);
        weaponPlusKeyP1 = Input.GetKeyDown(KeyCode.Alpha2);
        weaponMinusKeyP2 = Input.GetKeyDown(KeyCode.Alpha3);
        weaponPlusKeyP2 = Input.GetKeyDown(KeyCode.Alpha4);
    }

    void Player1JointsPositions(bool isPlayerOne)
    {
        uint userId;
        if (isPlayerOne)
            userId = kinectManager.GetPlayer1ID();
        else
            userId = kinectManager.GetPlayer2ID();

        if (kinectManager.IsPlayerCalibrated(userId))
        {
            for (int i = 0; i < iJointIndexes.Length; i++)
            {
                if (kinectManager.IsJointTracked(userId, iJointIndexes[i]))
                {
                    Vector3 posJoint = kinectManager.GetRawSkeletonJointPos(userId, iJointIndexes[i]);

                    if (posJoint != Vector3.zero)
                    {
                        Vector2 posDepth = kinectManager.GetDepthMapPosForJointPos(posJoint);
                        Vector2 posColor = kinectManager.GetColorMapPosForDepthPos(posDepth);

                        float scaleX = posColor.x / KinectWrapper.Constants.ColorImageWidth;
                        float scaleY = 1.0f - posColor.y / KinectWrapper.Constants.ColorImageHeight;

                        if (isPlayerOne)
                            cursor1Pos = Camera.main.ViewportToWorldPoint(new Vector3(scaleX, scaleY, distanceToCamera));
                        else
                            cursor2Pos = Camera.main.ViewportToWorldPoint(new Vector3(scaleX, scaleY, distanceToCamera));
                    }
                }
            }
        }
    }
}