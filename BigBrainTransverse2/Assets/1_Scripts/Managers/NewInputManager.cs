using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewInputManager : MonoBehaviour
{
    [Header("Objects to serialize")]
    [SerializeField] PlayerKnob[] playerKnobs;

    [Header("Serializable variables")]
    [SerializeField] KinectWrapper.NuiSkeletonPositionIndex[] TrackedJoints;
    [SerializeField] float smoothFactor = 5f;

    //Private
    GestureListener gestureListener;

    float distanceToCamera;

    //Kinect inputs
    //Player 1
    public bool swipeDownP1;



    //Mouse clicks
    public bool leftClick;
    public bool mouseWheelClick;
    public bool rightClick;

    //Keyboard keys
    public bool switchCameraOnOff;

    void Start()
    {
        gestureListener = FindObjectOfType<GestureListener>();


    }

    void Update ()
    {
        //KinectInputs
        KinectManager kinectManager = KinectManager.Instance;
        if (kinectManager && kinectManager.IsInitialized() && kinectManager.IsUserDetected())
        {
            /*
            for (int i = 0; i < iJointIndexes.Length; i++)
            {
                iJointIndexes[i] = (int)TrackedJoints[i];
            }
            */

            if (gestureListener)
            {
                if (gestureListener.IsSwipeDownP1())
                {
                    Debug.Log("Swiped down player1");
                }

                if (gestureListener.IsSwipeDownP2())
                {
                    Debug.Log("Swipe down player2");
                }
            }
        }

        //Mouse clicks
        leftClick = Input.GetMouseButton(0);
        rightClick = Input.GetMouseButton(1); ;
        mouseWheelClick = Input.GetMouseButton(2);

        //Keyboard keys
        switchCameraOnOff = Input.GetKeyDown(KeyCode.C);
    }
}