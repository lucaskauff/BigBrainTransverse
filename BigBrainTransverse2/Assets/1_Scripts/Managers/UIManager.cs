using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    //GameManager
    NewGameManager gameManager;
    SceneLoader sceneLoader;

    [Header("Objects to serialize")]
    [SerializeField] SuperTextMesh timerText;
    [SerializeField] Image[] playerBlasons;
    [SerializeField] Image[] playerGauges;
    [SerializeField] RectTransform[] selectors;
    [SerializeField] RectTransform[] weaponsPlayer1;
    [SerializeField] RectTransform[] weaponsPlayer2;
    [SerializeField] Sprite[] allWeapons;
    [SerializeField] Sprite[] allBlasons;

    [Header("Serializable variables")]
    [SerializeField, Range(10, 300)] float maxTimer;
    [SerializeField, Range(1, 25)] int maxPeopleDead;
    [SerializeField] Color cacheColor;
    [SerializeField] string nextSceneName;

    //Hidden Public
    [HideInInspector] public bool[] doesPlayerHaveUlt;
    [HideInInspector] public int[] playerSelection;

    //Private
    float currentTimer;
    int minutes;
    float seconds;
    float deciSeconds;
    float centiSeconds;

    private void Start()
    {
        gameManager = NewGameManager.Instance;
        sceneLoader = gameManager.sceneLoader;

        doesPlayerHaveUlt = new bool[2];
        playerSelection = new int[2];

        minutes = (int)maxTimer / 60;

        for (int i = 0; i < doesPlayerHaveUlt.Length; i++)
        {
            TurnBlasonsForRightLobby(i);
            TurnWeaponsForRightLobby(i);
        }
    }

    void TurnBlasonsForRightLobby(int playerIndex)
    {
        playerBlasons[playerIndex].sprite = allBlasons[gameManager.selectedLobbyPlayers[playerIndex]];
    }

    void TurnWeaponsForRightLobby(int playerIndex)
    {
        if (playerIndex == 0)
        {
            if (gameManager.selectedLobbyPlayers[0] == 0)
            {
                weaponsPlayer1[0].GetComponent<Image>().sprite = allWeapons[0];
                weaponsPlayer1[1].GetComponent<Image>().sprite = allWeapons[2];
            }
            else
            {
                weaponsPlayer1[0].GetComponent<Image>().sprite = allWeapons[1];
                weaponsPlayer1[1].GetComponent<Image>().sprite = allWeapons[3];
            }
        }
        else
        {
            if (gameManager.selectedLobbyPlayers[1] == 0)
            {
                weaponsPlayer2[0].GetComponent<Image>().sprite = allWeapons[0];
                weaponsPlayer2[1].GetComponent<Image>().sprite = allWeapons[2];
            }
            else
            {
                weaponsPlayer2[0].GetComponent<Image>().sprite = allWeapons[1];
                weaponsPlayer2[1].GetComponent<Image>().sprite = allWeapons[3];
            }
        }
    }

    void Update()
    {
        TimerTranslation();

        for (int g = 0; g < playerGauges.Length; g++)
        {
            UpdateGauges(g);
        }

        for (int i = 0; i < doesPlayerHaveUlt.Length; i++)
        {
            WeaponsSelection(i);
        }
    }

    void TimerTranslation()
    {
        currentTimer = maxTimer - Time.time;

        seconds = (int)currentTimer - minutes*60;
        if (seconds == 60) seconds = 59; 
        deciSeconds = (int)((currentTimer - minutes*60 - seconds) * 10);
        if (deciSeconds >= 10) deciSeconds -= 10;
        centiSeconds = (int)((currentTimer - minutes*60 - seconds - (deciSeconds / 10)) * 100);
        if (centiSeconds >= 100) centiSeconds -= 100;

        if (seconds < 0)
            minutes -= 1;

        if (currentTimer > 0)
        {
            if (seconds >= 10)
            {
                timerText.text = minutes.ToString()
                + "'" + seconds.ToString()
                + "''" + deciSeconds.ToString() + centiSeconds.ToString();
            }
            else
            {
                timerText.text = minutes.ToString()
                + "'0" + seconds.ToString()
                + "''" + deciSeconds.ToString() + centiSeconds.ToString();
            }
        }
        else
        {
            Debug.Log("Game Over");
            //sceneLoader.ChangeScene(nextSceneName);
        }
    }

    void UpdateGauges(int playerIndex)
    {
        playerGauges[playerIndex].fillAmount = (float)gameManager.peopleKilled[playerIndex] / maxPeopleDead;
        
        if (gameManager.peopleKilled[playerIndex] >= maxPeopleDead)
        {
            Debug.Log("Game Over");
            //sceneLoader.ChangeScene(nextSceneName);
        }        
    }

    void WeaponsSelection(int playerIndex)
    {
        if (playerIndex == 0)
        {
            selectors[playerIndex].position = weaponsPlayer1[playerSelection[playerIndex]].position;

            if (!doesPlayerHaveUlt[playerIndex])
                weaponsPlayer1[2].GetComponent<Image>().color = cacheColor;
            else
                weaponsPlayer1[2].GetComponent<Image>().color = Color.white;
        }
        else
        {
            selectors[playerIndex].position = weaponsPlayer2[playerSelection[playerIndex]].position;

            if (!doesPlayerHaveUlt[playerIndex])
                weaponsPlayer2[2].GetComponent<Image>().color = cacheColor;
            else
                weaponsPlayer2[2].GetComponent<Image>().color = Color.white;
        }
    }
}