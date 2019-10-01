using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplanationSceneMan : MonoBehaviour
{
    [Header("Serializable variables")]
    [SerializeField] float secondsGugusAreOnScreen;

    [Header("Objects to serialize")]
    [SerializeField] Animator[] controls;
    [SerializeField] GameObject[] gugus;

	void Start ()
    {
		
	}

	void Update ()
    {
		
	}

    IEnumerator ShowGugus()
    {
        yield return new WaitForSeconds(secondsGugusAreOnScreen);
        foreach (var guy in gugus)
        {
            guy.SetActive(false);
        }
    }
}
