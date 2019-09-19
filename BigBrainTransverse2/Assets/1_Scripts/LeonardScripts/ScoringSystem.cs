using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoringSystem : MonoBehaviour
{
    static int player1Score = 0;
    static int player2Score = 0;



    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        //ScoreCalculator();
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

    void ScoreCalculator()
    {
        Debug.Log("Player1 has " + player1Score + " points");
        Debug.Log("Player2 has " + player2Score + " points");
    }
}
