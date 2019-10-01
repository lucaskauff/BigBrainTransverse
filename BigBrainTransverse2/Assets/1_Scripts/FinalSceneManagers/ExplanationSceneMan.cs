using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplanationSceneMan : MonoBehaviour
{
    //GameManager
    SceneLoader sceneLoader;

    [Header("Serializable variables")]
    [SerializeField] float secondsGugusAreOnScreen;
    [SerializeField] string nextSceneName;

    [Header("Objects to serialize")]
    [SerializeField] GameObject gugus;

	void Start ()
    {
        sceneLoader = FindObjectOfType<SceneLoader>();
	}

    public void ShowTheGuys()
    {
        gugus.SetActive(true);

        StartCoroutine(ShowGugus());
    }

    IEnumerator ShowGugus()
    {
        yield return new WaitForSeconds(secondsGugusAreOnScreen);
        sceneLoader.ChangeScene(nextSceneName);
    }
}
