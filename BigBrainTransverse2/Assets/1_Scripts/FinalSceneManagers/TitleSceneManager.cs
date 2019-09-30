using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TitleSceneManager : MonoBehaviour
{
    //GameManager
    SceneLoader sceneLoader;
    NewInputManager inputManager;

    [Header("Serializable")]
    [SerializeField] string nextSceneName;

    void Start()
    {
        //sceneLoader = NewGameManager.Instance.sceneLoader;
        //inputManager = NewGameManager.Instance.inputManager;

        sceneLoader = FindObjectOfType<SceneLoader>();
        inputManager = FindObjectOfType<NewInputManager>();
    }

    void Update ()
    {
		if (inputManager.anyKeyDown)
        {
            sceneLoader.ChangeScene(nextSceneName);
        }
	}
}