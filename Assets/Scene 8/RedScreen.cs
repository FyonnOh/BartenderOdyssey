using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Yarn.Unity;

public class RedScreen : MonoBehaviour
{
    public Text introText;

    // Start is called before the first frame update
    void Start()
    {
        this.GetComponent<Image>().canvasRenderer.SetAlpha(0.0f);
        introText.canvasRenderer.SetAlpha(0.0f);

        DialogueRunner dialogueRunner = FindObjectOfType<DialogueRunner>();
        if (dialogueRunner != null)
        {
            dialogueRunner.AddCommandHandler("fadeEnding", FadeInTitle);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
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
        this.GetComponent<Image>().CrossFadeAlpha(1.0f, duration, true);
        introText.CrossFadeAlpha(1.0f, duration, true);
        yield return new WaitForSeconds(duration);
        onComplete();
    }
}
