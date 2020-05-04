using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Yarn.Unity;

public class SceneLoader : MonoBehaviour
{
    public Animator sceneAnimator;

    public void QuitGame()
    {
        Application.Quit();
    }

    [YarnCommand("loadNextScene")]
    public void LoadNextScene() {
        StartCoroutine(DoLoadScene(SceneManager.GetActiveScene().buildIndex + 1));
    }

    [YarnCommand("loadScene")]
    public void LoadScene(string parameter)
    {
        if (Int32.TryParse(parameter, out int sceneIndex))
        {
            StartCoroutine(DoLoadScene(sceneIndex));
        }
        else
        {
            Debug.LogErrorFormat($"Cannot parse {parameter} as an integer scene index to load.");
            // Scene sceneToLoad = SceneManager.GetSceneByPath(parameter);
            // if (sceneToLoad.IsValid())
            // {
            //     StartCoroutine(DoLoadScene(sceneToLoad.buildIndex));
            // }
            // else
            // {
            //     Debug.LogErrorFormat($"Failed to parse {parameter} as a valid scene name.");
            // }
        }
    }

    private IEnumerator DoLoadScene(int sceneIndex) {
        //play animation
        sceneAnimator.SetTrigger("Start");
        //wait
        yield return new WaitForSeconds(3);
        SceneManager.LoadScene(sceneIndex);
    }
}
