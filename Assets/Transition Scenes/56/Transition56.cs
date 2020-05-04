using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Yarn.Unity;
using Yarn.Unity.BartenderOdyssey;

public class Transition56 : MonoBehaviour
{
    public Text titleText;
    public TextMeshProUGUI introText;
    public Animator sceneAnimator;
    private bool hasIntroStarted = false;
    private bool hasIntroEnded = false;
    private DialogueRunner dialogueRunner;

    void Start()
    {
        dialogueRunner = FindObjectOfType<DialogueRunner>();
        if (dialogueRunner != null)
        {
            dialogueRunner.AddCommandHandler("fadeInTitle", FadeInTitle);
        }

        // Hide the title
        titleText.canvasRenderer.SetAlpha(0.0f);

        // Ensure the introduction text is cleared
        introText.text = "";
    }

    void Update()
    {
        if (!hasIntroStarted)
        {
            if (dialogueRunner != null && !dialogueRunner.isDialogueRunning)
            {
                dialogueRunner.StartDialogue("transitionScene56");
                hasIntroStarted = true;
            }
        }

        if (!hasIntroEnded && this.gameObject.GetComponent<IntroSpeech>().IsLineComplete)
        {
            FindObjectOfType<ExtendedDialogueUI>().MarkLineComplete();
            this.gameObject.GetComponent<IntroSpeech>().IsLineComplete = false;
        }
    }
    
    [YarnCommand("newline")]
    public void InsertNewLine(string num) 
    {
        if (Int32.TryParse(num, out int count)) 
        {
            if (count < 0) count *= -1; // convert negative integers to positive
            while (count > 0)
            {
                introText.text += "\n";
                count--;
            }
        }
        else
        {
            Debug.LogError($"Parameter {num} is an invalid integer.");
        }
    }

    [YarnCommand("markIntroEnd")]
    public void MarkIntroEnd()
    {
        hasIntroEnded = true;
    }

    [YarnCommand("loadNextLevel")]
    public void loadNextLevel() {
        StartCoroutine(LoadLevel(SceneManager.GetActiveScene().buildIndex));
    }

    IEnumerator LoadLevel(int levelIndex) {
        //play animation
        sceneAnimator.SetTrigger("Start");
        //wait
        yield return new WaitForSeconds(3);
        SceneManager.LoadScene(levelIndex + 1);
    }

    private void FadeInTitle(string[] parameters, System.Action onComplete)
    {
        float duration = 1.0f; // default fade duration
        if (parameters.Length >= 2)
        {
            // First parameter is the GameObject name
            if (!float.TryParse(parameters[1], out duration))
                Debug.LogError($"Parameter {parameters[1]} must be a valid fade-in duration (float).");
        }

        StartCoroutine(DoFadeInTitle(duration, onComplete));
    }

    private IEnumerator DoFadeInTitle(float duration, System.Action onComplete)
    {
        Debug.Log($"Duration: {duration}");
        titleText.CrossFadeAlpha(1.0f, duration, true);
        yield return new WaitForSeconds(duration);
        onComplete();
    }
}
