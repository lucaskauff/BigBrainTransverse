using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathRecap : MonoBehaviour
{
    [SerializeField] Animator bubble;

    void BubbleShouldFadeOut()
    {
        bubble.SetTrigger("FadeOut");
    }
}