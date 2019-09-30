using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SelectionSceneMan : MonoBehaviour
{
    //GameManager
    NewGameManager gameManager;
    SceneLoader sceneLoader;

    [SerializeField] ConnectionBoxes[] connectionBoxes;
    [SerializeField] string nextSceneName;

    private void Start()
    {
        /*
        gameManager = NewGameManager.Instance;
        sceneLoader = gameManager.sceneLoader;
        */

        gameManager = FindObjectOfType<NewGameManager>();
        sceneLoader = gameManager.GetComponent<SceneLoader>();
    }

    void Update ()
    {
        if (connectionBoxes[0].isSelected && connectionBoxes[1].isSelected)
        {
            for (int i = 0; i < connectionBoxes.Length; i++)
            {
                if (connectionBoxes[i].selectedBy == 1)
                {
                    gameManager.selectedLobbyPlayers[0] = connectionBoxes[i].whichLobby;
                }
                else if (connectionBoxes[i].selectedBy == 2)
                {
                    gameManager.selectedLobbyPlayers[1] = connectionBoxes[i].whichLobby;
                }
            }

            sceneLoader.ChangeScene(nextSceneName);
        }
	}
}