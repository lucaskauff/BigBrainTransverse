using Sirenix.OdinInspector;
using Sirenix.Serialization;
using System.Diagnostics;
using UnityEngine;

public class ScoringSystem : MonoBehaviour
{
    [ShowInInspector] [FoldoutGroup("Player Scores")] static int player1Score = 0;
    [ShowInInspector] [FoldoutGroup("Player Scores")] static int player2Score = 0;

    [SerializeField] [FoldoutGroup("Game Variables")] float gameLengthInSeconds;


    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        //DebuggingScoreCalculator();
        GameTimer();
    }

    public static void GreaseKills(string whichPlayer = "")
    {
        if(whichPlayer == "player1")
        {
            player1Score++;
        }

        else if (whichPlayer == "player2")
        {
            player2Score++;
        }
    }

    public static void SweetKills(string whichPlayer = "", float yeeter = 0)
    {
        if (whichPlayer == "player1")
        {
            player1Score++;
        }

        else if (whichPlayer == "player2")
        {
            player2Score++;
        }
    }

    public static void EnergyKills(string whichPlayer = "", float yeeter = 0)
    {
        if (whichPlayer == "player1")
        {
            player1Score++;
        }

        else if (whichPlayer == "player2")
        {
            player2Score++;
        }
    }

    void DebuggingScoreCalculator()
    {
        UnityEngine.Debug.Log("Player1 has " + player1Score + " points");
        UnityEngine.Debug.Log("Player2 has " + player2Score + " points");
    }
    
    void GameTimer() 
    {
        gameLengthInSeconds -= Time.deltaTime;
        if (gameLengthInSeconds < 0) {
            GameOver();
        }
    }

    void GameOver() 
    {
        UnityEngine.Debug.Log("The game is over");

        if(player1Score > player2Score) 
        {
            UnityEngine.Debug.Log("Player1 has defeated player2");
        }

        else if (player2Score > player1Score) 
        {
            UnityEngine.Debug.Log("Player2 has defeated player1");
        }
    }
}
