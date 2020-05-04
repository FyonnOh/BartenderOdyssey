using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonActionWrapper : MonoBehaviour
{
    public SceneLoader sceneLoader;

    public void Replay()
    {
        sceneLoader.LoadScene("0");
    }

    public void Quit()
    {
        Application.Quit();
    }
}
