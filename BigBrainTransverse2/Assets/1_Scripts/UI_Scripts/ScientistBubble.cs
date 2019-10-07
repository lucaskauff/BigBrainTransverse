using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScientistBubble : MonoBehaviour
{
    public int whichScientistDeath;
    [SerializeField] Animator scientistDeathReason;

    void TriggerScientistAnimation()
    {
        if (whichScientistDeath == 0) scientistDeathReason.SetTrigger("Death1");
        else if (whichScientistDeath == 1) scientistDeathReason.SetTrigger("Death2");
        else if (whichScientistDeath == 2) scientistDeathReason.SetTrigger("Death3");
    }
}
