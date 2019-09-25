using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TheBubble : MonoBehaviour
{
    [SerializeField] Animator deathReason;

    void TriggerDeathAnimation()
    {
        deathReason.SetTrigger("Death1");
    }
}