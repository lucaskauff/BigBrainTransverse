using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewInputManager : MonoBehaviour
{
    [Header("Public variables")]
    public bool isUsingKinectInputs = true;
    public float smoothFactor = 60f;
    public KinectWrapper.NuiSkeletonPositionIndex gestureJoint;

    [Header("Serializable variables")]
    [SerializeField] KinectWrapper.NuiSkeletonPositionIndex[] TrackedJoints;

    //Private
    SceneLoader sceneLoader;

    KinectManager kinectManager;
    GestureListener gestureListener;

    int[] iJointIndexes;
    PlayerController playerKnobExample;
    public float distanceToCamera;
    public float taposiSion;

    //Kinect inputs
    //Player 1
    public Vector3 cursor1Pos;
    public bool swipeLeftP1;
    public bool swipeRightP1;
    public bool swipeDownP1;
    public bool pushP1;
    public bool throwP1;

    //Player 2
    public Vector3 cursor2Pos;
    public bool swipeLeftP2;
    public bool swipeRightP2;
    public bool swipeDownP2;
    public bool pushP2;
    public bool throwP2;

    //AnyKey
    public bool anyKeyDown;

    //Mouse inputs
    public Vector3 mouseCursorPos;
    public bool mouseLeftClick;
    public bool mouseWheelClick;
    public bool mouseRightClick;

    //Keyboard inputs
    public bool weaponMinusKeyP1;
    public bool weaponPlusKeyP1;
    public bool weaponMinusKeyP2;
    public bool weaponPlusKeyP2;
    public bool switchCameraOnOff;

    private void Awake()
    {
        sceneLoader = FindObjectOfType<SceneLoader>();

        //kinectManager = KinectManager.Instance;

        iJointIndexes = new int[TrackedJoints.Length];

        if (sceneLoader.actualSceneName == "04_GameScene")
        {
            gestureListener = FindObjectOfType<GestureListener>();

            playerKnobExample = FindObjectOfType<PlayerController>();
        }
    }

    void Start()
    {
        //
    }

    void Update ()
    {
        kinectManager = KinectManager.Instance;

        //KinectInputs
        if (kinectManager && kinectManager.IsInitialized() && kinectManager.IsUserDetected())
        {            
            for (int i = 0; i < iJointIndexes.Length; i++)
            {
                iJointIndexes[i] = (int)TrackedJoints[i];
            }

            PlayerJointsPositions(true);
            PlayerJointsPositions(false);

            if (gestureListener)
            {
                swipeLeftP1 = gestureListener.IsSwipeLeftP1();
                swipeRightP1 = gestureListener.IsSwipeRightP1();
                swipeDownP1 = gestureListener.IsSwipeDownP1();
                pushP1 = gestureListener.IsPushP1();
                throwP1 = gestureListener.IsThrowP1();

                swipeLeftP2 = gestureListener.IsSwipeLeftP2();
                swipeRightP2 = gestureListener.IsSwipeRightP2();
                swipeDownP2 = gestureListener.IsSwipeDownP2();
                pushP2 = gestureListener.IsPushP2();
                throwP2 = gestureListener.IsThrowP2();
            }
        }

        //Any key pressed
        anyKeyDown = Input.anyKeyDown;

        //Mouse clicks
        mouseCursorPos = Input.mousePosition;
        mouseLeftClick = Input.GetMouseButtonDown(0);
        mouseRightClick = Input.GetMouseButtonDown(1);
        mouseWheelClick = Input.GetMouseButtonDown(2);

        //Keyboard keys
        switchCameraOnOff = Input.GetKeyDown(KeyCode.C);
        weaponMinusKeyP1 = Input.GetKeyDown(KeyCode.Alpha1);
        weaponPlusKeyP1 = Input.GetKeyDown(KeyCode.Alpha2);
        weaponMinusKeyP2 = Input.GetKeyDown(KeyCode.Alpha3);
        weaponPlusKeyP2 = Input.GetKeyDown(KeyCode.Alpha4);
    }

    void PlayerJointsPositions(bool isPlayerOne)
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
                            cursor1Pos = Camera.main.ViewportToWorldPoint(new Vector3(scaleX, scaleY, taposiSion));
                        else
                            cursor2Pos = Camera.main.ViewportToWorldPoint(new Vector3(scaleX, scaleY, taposiSion));
                    }
                }
            }
        }
    }
}