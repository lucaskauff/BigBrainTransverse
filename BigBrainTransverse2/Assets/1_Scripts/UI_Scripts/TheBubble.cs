using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TheBubble : MonoBehaviour
{
    NewGameManager gameManager;

    public int whichDeath;
    [SerializeField] Animator deathReason;

    private void Start()
    {
        //gameManager = NewGameManager.Instance;
        gameManager = FindObjectOfType<NewGameManager>();
    }

    void TriggerDeathAnimation()
    {
        if (whichDeath == 0) deathReason.SetTrigger("Death1");
        else if (whichDeath == 1) deathReason.SetTrigger("Death2");
        else if (whichDeath == 2) deathReason.SetTrigger("Death3");
        //Bonus
        else if (whichDeath == 3) deathReason.SetTrigger("Death4");
        else if (whichDeath == 4) deathReason.SetTrigger("Death5");
    }

    void CompletelyGone()
    {
        gameManager.deathAnimsOnGoing -= 1;
    }
}