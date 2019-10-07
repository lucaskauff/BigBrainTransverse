using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SceneLoader))]
[RequireComponent(typeof(NewInputManager))]
public class NewGameManager : /*Singleton<NewGameManager>*/ MonoBehaviour
{
    //protected NewGameManager() { }

    public SceneLoader sceneLoader;
    public NewInputManager inputManager;

    public int[] selectedLobbyPlayers;
    public int[] peopleKilled;

    public int maxDeathAnims = 5;
    public int deathAnimsOnGoing;

    private void Awake()
    {
        sceneLoader = GetComponent<SceneLoader>();
        inputManager = GetComponent<NewInputManager>();

        //Quite some things to fix there !
        selectedLobbyPlayers = new int[2];
        peopleKilled = new int[2];

        //Debug
        selectedLobbyPlayers[0] = 0;
        selectedLobbyPlayers[1] = 0;
    }

    private void Start()
    {
        //
    }
}