using UnityEngine;
using System.Collections;
using System;

public class GestureListener : MonoBehaviour, KinectGestures.GestureListenerInterface
{
	// GUI Text to display the gesture messages.
	public GUIText GestureInfo;

    NewInputManager inputManager;
    KinectManager manager;

    private bool swipeLeftP1;
	private bool swipeRightP1;
    private bool swipeDownP1;
    private bool pushP1;
    private bool throwP1;

    private bool swipeLeftP2;
    private bool swipeRightP2;
    private bool swipeDownP2;
    private bool pushP2;
    private bool throwP2;

    public bool IsSwipeLeftP1()
	{
		if(swipeLeftP1)
		{
			swipeLeftP1 = false;
			return true;
		}
		
		return false;
	}
	
	public bool IsSwipeRightP1()
	{
		if(swipeRightP1)
		{
			swipeRightP1 = false;
			return true;
		}
		
		return false;
	}

    public bool IsSwipeDownP1()
    {
        if (swipeDownP1)
        {
            swipeDownP1 = false;
            return true;
        }

        return false;
    }

    public bool IsPushP1()
    {
        if (pushP1)
        {
            pushP1 = false;
            return true;
        }

        return false;
    }

    public bool IsThrowP1()
    {
        if (throwP1)
        {
            throwP1 = false;
            return true;
        }

        return false;
    }

    public bool IsSwipeLeftP2()
    {
        if (swipeLeftP2)
        {
            swipeLeftP2 = false;
            return true;
        }

        return false;
    }

    public bool IsSwipeRightP2()
    {
        if (swipeRightP2)
        {
            swipeRightP2 = false;
            return true;
        }

        return false;
    }

    public bool IsSwipeDownP2()
    {
        if (swipeDownP2)
        {
            swipeDownP2 = false;
            return true;
        }

        return false;
    }

    public bool IsPushP2()
    {
        if (pushP2)
        {
            pushP2 = false;
            return true;
        }

        return false;
    }

    public bool IsThrowP2()
    {
        if (throwP2)
        {
            throwP2 = false;
            return true;
        }

        return false;
    }

    private void Start()
    {
        //inputManager = NewGameManager.Instance.inputManager;
        inputManager = FindObjectOfType<NewInputManager>();
        manager = KinectManager.Instance;
    }

    public void UserDetected(uint userId, int userIndex)
	{
		// detect these user specific gestures
		//KinectManager manager = KinectManager.Instance;
		
		manager.DetectGesture(userId, KinectGestures.Gestures.SwipeLeft);
		manager.DetectGesture(userId, KinectGestures.Gestures.SwipeRight);
        manager.DetectGesture(userId, KinectGestures.Gestures.SwipeDown);
        manager.DetectGesture(userId, KinectGestures.Gestures.Push);
        manager.DetectGesture(userId, KinectGestures.Gestures.Throw);

        if (GestureInfo != null)
		{
			GestureInfo.GetComponent<GUIText>().text = "Swipe left or right to change the slides.";
		}
	}
	
	public void UserLost(uint userId, int userIndex)
	{
		if(GestureInfo != null)
		{
			GestureInfo.GetComponent<GUIText>().text = string.Empty;
		}
	}

	public void GestureInProgress(uint userId, int userIndex, KinectGestures.Gestures gesture, 
	                              float progress, KinectWrapper.NuiSkeletonPositionIndex joint, Vector3 screenPos)
	{
		// don't do anything here
	}

	public bool GestureCompleted (uint userId, int userIndex, KinectGestures.Gestures gesture, 
	                              KinectWrapper.NuiSkeletonPositionIndex joint, Vector3 screenPos)
	{
		string sGestureText = gesture + " detected";
		if(GestureInfo != null)
		{
			GestureInfo.GetComponent<GUIText>().text = sGestureText;
		}
		
        if (userId == manager.GetPlayer1ID())
        {            
            if (joint == inputManager.gestureJoint)
            {
                if (gesture == KinectGestures.Gestures.SwipeLeft)
                    swipeLeftP1 = true;
                else if (gesture == KinectGestures.Gestures.SwipeRight)
                    swipeRightP1 = true;
                else if (gesture == KinectGestures.Gestures.SwipeDown)
                    swipeDownP1 = true;
                else if (gesture == KinectGestures.Gestures.Push)
                    pushP1 = true;
                else if (gesture == KinectGestures.Gestures.Throw)
                    throwP1 = true;
            }
        }

        if (userId == manager.GetPlayer2ID())
        {
            if (joint == inputManager.gestureJoint)
            {
                if (gesture == KinectGestures.Gestures.SwipeLeft)
                    swipeLeftP2 = true;
                else if (gesture == KinectGestures.Gestures.SwipeRight)
                    swipeRightP2 = true;
                else if (gesture == KinectGestures.Gestures.SwipeDown)
                    swipeDownP2 = true;
                else if (gesture == KinectGestures.Gestures.Push)
                    pushP2 = true;
                else if (gesture == KinectGestures.Gestures.Throw)
                    throwP2 = true;
            }
        }

        return true;
	}

	public bool GestureCancelled (uint userId, int userIndex, KinectGestures.Gestures gesture, 
	                              KinectWrapper.NuiSkeletonPositionIndex joint)
	{
		// don't do anything here, just reset the gesture state
		return true;
	}
	
}
