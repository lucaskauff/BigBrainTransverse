using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SceneLoader))]
//[RequireComponent(typeof(InputManager))]
public class NewGameManager : Singleton<NewGameManager>
{
    protected NewGameManager() { }

    public SceneLoader sceneLoader;
    //public InputManager inputManager;

    private void Awake()
    {
        sceneLoader = GetComponent<SceneLoader>();
        //inputManager = GetComponent<InputManager>();
    }
}