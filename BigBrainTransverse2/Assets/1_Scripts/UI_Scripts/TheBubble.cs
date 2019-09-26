using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TheBubble : MonoBehaviour
{
    public int whichDeath;
    [SerializeField] Animator deathReason;

    void TriggerDeathAnimation()
    {
        if (whichDeath == 0) deathReason.SetTrigger("Death1");
        else if (whichDeath == 1) deathReason.SetTrigger("Death2");
        else if (whichDeath == 2) deathReason.SetTrigger("Death3");
    }
}