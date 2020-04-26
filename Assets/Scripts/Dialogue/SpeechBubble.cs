using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Yarn.Unity.BartenderOdyssey
{
    public class SpeechBubble : MonoBehaviour
    {
        public string characterName { get; private set; }

        // the text UI to display the character's lines
        public Text speechText;
        public Image background;

        void Awake()
        {
            // Get the name of the NPC
            characterName = this.gameObject.GetComponentInParent<NPC>().characterName;
            Debug.Log($"Character name is {characterName}");

            // Hide the speech bubble and buttons when the game starts
            if (speechText != null && background != null) 
            {
                SetActive(false);
            }
        }

        public void OnDialogueStart()
        {
            // Default implementation is empty.
            // You can use this to disable user controls during
            // a dialogue.
        }

        public void OnDialogueEnd()
        {
            // Default implementation is empty.
            // You can use this to re-enable user controls after
            // a dialogue.
        }

        public void OnLineStart(string fullText = "")
        {
            Debug.Log($"(SpeechBubble.cs) line: {fullText}");

            // Dynamically resize the speech bubble to fit the line
            if (!string.IsNullOrEmpty(fullText)) 
            {
                TextGenerationSettings settings = speechText.GetGenerationSettings(speechText.rectTransform.rect.size);

                float width = speechText.cachedTextGeneratorForLayout.GetPreferredWidth(fullText, settings);
                float height = speechText.cachedTextGeneratorForLayout.GetPreferredHeight(fullText, settings);

                Debug.Log($"Width: {speechText.preferredWidth}; Height: {speechText.preferredHeight}");
                Debug.Log($"Width: {width}; Height: {height}");
            }

            if (speechText != null && background != null) 
            {
                ShowSpeechBubble(true);
                ShowOptions(false);
            }
        }

        // Use this for things like displaying response options
        // for the player to choose, or enabling input to continue
        // to the next line.
        public void OnLineFinishDisplaying()
        {
            // Show the next button to advance the dialogue
            // if (nextButton != null) 
            // {
            //     ShowOptions(true);
            // }
        }

        public void OnLineUpdate(string line)
        {
            if (speechText != null) 
            {
                speechText.text = line;
            }
        }

        public void OnLineEnd()
        {
            if (speechText != null && background != null) 
            {
                ShowSpeechBubble(false);
                ShowOptions(false);
            }
        }

        public void OnOptionsStart()
        {

        }

        public void OnOptionsEnd()
        {

        }

        public void OnCommand(string command)
        {

        }

        public void SetActive(bool setActive)
        {
            ShowSpeechBubble(setActive);
            ShowOptions(setActive);
        }

        private void ShowSpeechBubble(bool show) 
        {
            speechText.gameObject.SetActive(show);
            background.gameObject.SetActive(show);
        }

        private void ShowOptions(bool show) 
        {
            // nextButton.gameObject.SetActive(show);
        }
    }
}