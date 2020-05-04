using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Yarn.Unity;

public class RedScreen : MonoBehaviour
{
    public float timeBetweenFlash = 1.0f;
    public float warningAppearTime = 0.25f;
    public Image warningImage;
    public Sprite warningSprite;
    public Text messageText;
    private bool isFlashOn;
    private bool keepFlashing;

    // Start is called before the first frame update
    void Start()
    {
        this.GetComponent<Image>().canvasRenderer.SetAlpha(0.0f);
        messageText.canvasRenderer.SetAlpha(0.0f);
        warningImage.canvasRenderer.SetAlpha(0.0f);
        warningImage.gameObject.SetActive(false);

        // DialogueRunner dialogueRunner = FindObjectOfType<DialogueRunner>();
        // if (dialogueRunner != null)
        // {
        //     dialogueRunner.AddCommandHandler("fadeEnding", FadeInTitle);
        // }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    [YarnCommand("flashRed")]
    public void FlashRed()
    {
        isFlashOn = false;
        keepFlashing = true;
        this.GetComponent<Image>().canvasRenderer.SetAlpha(0.0f);
        StartCoroutine(DoFlashRed());
    }

    private IEnumerator DoFlashRed()
    {
        while (keepFlashing)
        {
            if (isFlashOn)
            {
                // Fade the flash
                this.GetComponent<Image>().CrossFadeAlpha(0.25f, timeBetweenFlash, true);
                isFlashOn = false;
            }
            else
            {
                // Turn on the flash
                this.GetComponent<Image>().CrossFadeAlpha(1.0f, timeBetweenFlash, true);
                isFlashOn = true;
            }
            yield return new WaitForSeconds(timeBetweenFlash);
        }
    }

    [YarnCommand("shutdown")]
    public void Shutdown()
    {
        StartCoroutine(DoShutdown());
    }

    private IEnumerator DoShutdown()
    {
        // Stop the screen flashing red
        keepFlashing = false;

        // Ensure the screen stays red
        if (!isFlashOn)
        {
            this.GetComponent<Image>().CrossFadeAlpha(1.0f, timeBetweenFlash, true);
            yield return new WaitForSeconds(timeBetweenFlash);
        }

        // Display warning signs fter screen is fully red
        warningImage.gameObject.SetActive(true);
        warningImage.sprite = warningSprite;
        warningImage.CrossFadeAlpha(1.0f, warningAppearTime, true);
        messageText.text = "Systems shutting down";
        messageText.CrossFadeAlpha(1.0f, warningAppearTime, true);
    }

    [YarnCommand("showEndScreen")]
    public void ShowEndScreen()
    {
        StartCoroutine(DoShowEndScreen());
    }

    private IEnumerator DoShowEndScreen()
    {
        float duration = 2f;
        float delay = 2f;
        Image panelImage = this.GetComponent<Image>();
        float initialAlpha = panelImage.color.a;
        Color startColor = panelImage.color;
        Color intermediateColor = new Color(0, 0, 0, initialAlpha);
        Color endColor = new Color(0, 0, 0, 100);

        // Fade the warning signs
        warningImage.CrossFadeAlpha(0.0f, duration, true);
        messageText.CrossFadeAlpha(0.0f, duration, true);

        // Ensure the second loop starts from where the first loop ended
        float t = 0f;

        // Gradually change screen color from red to black at the same time
        for (; t < initialAlpha*duration; t += Time.deltaTime)
        {
            panelImage.color = Color.Lerp(startColor, intermediateColor, t / (initialAlpha*duration));
            Debug.Log($"t: {t}; limit: {initialAlpha*duration}; progress: {t / (initialAlpha*duration)}; alpha: {panelImage.color.a}");
            yield return null;
        }
        for (; panelImage.color.a < 1.0f; t += Time.deltaTime)
        {
            // panelImage.color = Color.Lerp(intermediateColor, endColor, t / (duration - initialAlpha*duration));
            Color tempColor = panelImage.color;
            tempColor.a = t / duration;
            panelImage.color = tempColor;
            // Debug.Log($"(Fade to black) t: {t}; limit: {(initialAlpha*duration)}; progress: {t / (initialAlpha*duration)}; alpha: {panelImage.color.a}");
            yield return null;
        }
        Debug.Log($"Final alpha: {panelImage.color.a}");
        // Don't mess up the layout with an invisible Image
        warningImage.gameObject.SetActive(false);
        
        // Display "The End" message after a delay
        yield return new WaitForSeconds(delay);
        messageText.text = "You have reached the bad ending.\nWould you like to replay?";
        messageText.CrossFadeAlpha(1.0f, delay, true);
    }
}

/*
<<runIn BarOwner>>
<<waitForRun BarOwner>>
<<wait 2>>
<<breakUpFight BarOwner>>
BarOwner: Hey! Stop this instance before I call the police!
<<wait 1>>
<<runToEntrance Customer1>>
<<rotate_Customer1 Customer1>>
<<wait 2>>
<<standUp Jameson>>
BarOwner: Deckard! What were you doing?!
BarOwner: This poor guy was beaten up right in front of you, and yet you didn't do anything?!
BarOwner: You could have shouted for help or something!
BarOwner: To think that you could stand there and watch as this poor man gets beaten up. 
BarOwner: Don't you feel anything?!
<<shakeHead BarOwner>>
BarOwner: I think I might have put too much faith in you.
BarOwner: How naive was I to believe that what they say about you is wrong.
BarOwner: I was hoping you could prove them wrong.
BarOwner: But look how the tables have changed. You proved me wrong instead.
<<leave BarOwner>>
<<wait 2>>
*/
