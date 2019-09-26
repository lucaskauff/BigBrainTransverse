using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SceneLoader))]
[RequireComponent(typeof(NewInputManager))]
public class NewGameManager : Singleton<NewGameManager>
{
    protected NewGameManager() { }

    public SceneLoader sceneLoader;
    public NewInputManager inputManager;

    public int[] selectedLobbyPlayers;
    public int[] peopleKilled;

    public bool isDeathAnimOnGoing = false;

    private void Awake()
    {
        sceneLoader = GetComponent<SceneLoader>();
        inputManager = GetComponent<NewInputManager>();
    }
}