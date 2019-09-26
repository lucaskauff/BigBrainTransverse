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
    [SerializeField] Image[] playerGauges; 

    [Header("Serializable variables")]
    [SerializeField, Range(60, 300)] float maxTimer;
    [SerializeField, Range(1, 25)] int maxPeopleDead;
    [SerializeField] string nextSceneName;

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
    }

    void Update()
    {
        TimerTranslation();

        for (int g = 0; g < playerGauges.Length; g++)
        {
            UpdateGauges(g);
        }
    }

    //To update with minutes and seconds
    void TimerTranslation()
    {
        currentTimer = maxTimer - Time.time;
        
        seconds = (int)currentTimer - minutes*60;
        deciSeconds = (int)((currentTimer - minutes*60 - seconds) * 10);
        centiSeconds = (int)((currentTimer - minutes*60 - seconds - (deciSeconds / 10)) * 100);

        if (seconds >= 60)
            minutes += 1;

        timerText.text = minutes.ToString()
            + "'" + (seconds).ToString()
            + "''" + deciSeconds.ToString() + centiSeconds.ToString();

        //Timer over
        if (currentTimer <= 0)
        {
            Debug.Log("Game Over");
            //sceneLoader.ChangeScene(nextSceneName);
        }
    }

    void UpdateGauges(int playerIndex)
    {
        playerGauges[playerIndex].fillAmount = gameManager.peopleKilled[playerIndex] / maxPeopleDead;
        
        if (gameManager.peopleKilled[playerIndex] >= maxPeopleDead)
        {
            Debug.Log("Game Over");
            //sceneLoader.ChangeScene(nextSceneName);
        }        
    }
}