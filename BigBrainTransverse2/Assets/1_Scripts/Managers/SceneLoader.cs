using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    public string actualSceneName;

    private void Start()
    {
        actualSceneName = SceneManager.GetActiveScene().name;
    }

    public void ChangeScene(string sceneToLoad)
    {
        actualSceneName = sceneToLoad;
        SceneManager.LoadScene(sceneToLoad);
    }

    public void ReloadScene()
    {
        SceneManager.LoadScene(actualSceneName);
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}