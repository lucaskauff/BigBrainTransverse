using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SelectionSceneManager : MonoBehaviour
{
    //GameManager
    NewGameManager gameManager;
    SceneLoader sceneLoader;
    NewInputManager inputManager;

    [Header("Objects to serialize")]
    [SerializeField] GestureListener gestureListener;

    [SerializeField] RectTransform[] selector;
    [SerializeField] RectTransform[] lobbies;
    [SerializeField] GameObject[] rideaux;

    [SerializeField] RawImage realtimeCam;
    [SerializeField] RectTransform tvButton;
    [SerializeField] Text tvButtonText;

    [Header("Serializable variables")]
    [SerializeField] float[] buttonYPositions;

    [SerializeField] Color cachedColor;

    [SerializeField] string previousSceneName;
    [SerializeField] string nextSceneName;

    //Hidden public
    [HideInInspector] int[] currentlySelectedLobby;

    //Private
    bool hasP1Selected;

    void Start()
    {
        gameManager = NewGameManager.Instance;
        sceneLoader = gameManager.sceneLoader;
        inputManager = gameManager.inputManager;

        gameManager.selectedLobbyPlayers = new int[2];

        currentlySelectedLobby = new int[selector.Length];

        hasP1Selected = false;
    }

    void Update ()
    {
        MouseBackupForSceneMan();

        TheCacheManagement();

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
                    Selection(0);
                }
            }

            UpdateSelector(0);
        }
	}

    void MouseBackupForSceneMan()
    {
        if (inputManager.leftClick)
            sceneLoader.ChangeScene(previousSceneName);
        else if (inputManager.mouseWheelClick)
            sceneLoader.ReloadScene();
        else if (inputManager.rightClick)
            sceneLoader.ChangeScene(nextSceneName);
    }

    void TheCacheManagement()
    {
        if (!hasP1Selected)
        {
            rideaux[0].SetActive(false);
            rideaux[1].SetActive(true);
        }
        else
        {
            rideaux[1].SetActive(false);
            rideaux[0].SetActive(true);
        }

        lobbies[currentlySelectedLobby[0] + 3].GetComponent<LobbyInfos>().isOnSelect = true;
    }

    void Selection(int playerIndex)
    {
        gameManager.selectedLobbyPlayers[playerIndex] = currentlySelectedLobby[playerIndex];
        hasP1Selected = true;
    }

    void UpdateSelector(int playerIndex)
    {
        if (playerIndex == 0)
            selector[playerIndex].anchoredPosition = lobbies[currentlySelectedLobby[playerIndex]].anchoredPosition;
        else
            selector[playerIndex].anchoredPosition = lobbies[currentlySelectedLobby[playerIndex] + 3].anchoredPosition;
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