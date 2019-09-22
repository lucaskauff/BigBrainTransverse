using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SelectionSceneManager : MonoBehaviour
{
    //GameManager
    SceneLoader sceneLoader;

    [Header("Objects to serialize")]
    [SerializeField] GestureListener gestureListener;

    [SerializeField] RectTransform[] selector;
    [SerializeField] RectTransform[] lobbies;

    [SerializeField] RawImage realtimeCam;
    [SerializeField] RectTransform tvButton;
    [SerializeField] Text tvButtonText;

    [Header("Serializable variables")]
    [SerializeField] float[] buttonYPositions;

    //Private
    int[] currentlySelectedLobby;

    void Start()
    {
        sceneLoader = GameManager.Instance.sceneLoader;

        currentlySelectedLobby = new int[selector.Length];
    }

    void Update ()
    {
        KinectManager kinectManager = KinectManager.Instance;

        if (!kinectManager || !kinectManager.IsInitialized() || !kinectManager.IsUserDetected())
            return;

        RenderRealtimeCam(kinectManager);

        if (kinectManager.IsPlayerCalibrated(kinectManager.Player1ID))
        {
            uint userId;
            userId = kinectManager.GetPlayer1ID();
            if (gestureListener)
            {
                /* Simple test*/
                if (gestureListener.IsSwipeLeft())
                {
                    currentlySelectedLobby[0] = ChangeSelection(0, -1);
                }
                else if (gestureListener.IsSwipeRight())
                {
                    currentlySelectedLobby[0] = ChangeSelection(0, 1);
                }
                else if (gestureListener.IsSwipeDown())
                {
                    Debug.Log("swipedown");
                }
                /**/
            }

            UpdateSelector(0);
        }
	}

    void UpdateSelector(int playerIndex)
    {
        selector[playerIndex].anchoredPosition = lobbies[currentlySelectedLobby[playerIndex]].anchoredPosition;
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

    int ChangeSelection(int playerIndex, int factor)
    {
        currentlySelectedLobby[playerIndex] += factor;

        if (currentlySelectedLobby[playerIndex] > 2)
            return 0;
        else if (currentlySelectedLobby[playerIndex] < 0)
            return 2;
        else
            return currentlySelectedLobby[playerIndex];
    }
}