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
    [SerializeField, Range(60, 300)] int maxTimer;
    [SerializeField, Range(1, 25)] int maxPeopleDead;
    [SerializeField] string nextSceneName;

    //Private
    float timer;
    string finaltext;

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
        timer = Time.time;
        timer = (int)timer;

        finaltext = timer.ToString();

        timerText.text = finaltext;

        //Timer over
        if (timer >= maxTimer)
        {
            Debug.Log("Game Over");
            //sceneLoader.ChangeScene(nextSceneName);
        }
    }

    void UpdateGauges(int playerIndex)
    {
        playerGauges[playerIndex].fillAmount = gameManager.peopleKilled[playerIndex] / maxPeopleDead;

        Debug.Log("Game Over");
        //sceneLoader.ChangeScene(nextSceneName);
    }
}