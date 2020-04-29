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
        // public virtual Component<Text> SpeechText 
        // { 
        //     get { return this._speechText.GetComponent<Text>(); } 
        //     set { this._speechText = value.gameObject; }
        // }
        public Text sizerText;
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

        public virtual void OnDialogueStart()
        {
            // Default implementation is empty.
            // You can use this to disable user controls during
            // a dialogue.
        }

        public virtual void OnDialogueEnd()
        {
            // Default implementation is empty.
            // You can use this to re-enable user controls after
            // a dialogue.
        }

        public virtual void OnLineStart(string fullText = "")
        {
            Debug.Log($"(SpeechBubble.cs) line: {fullText}");

            // Set the invisible text to dynamically resize the speech bubble
            if (sizerText != null) 
            {
                sizerText.text = fullText;
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
        public virtual void OnLineFinishDisplaying()
        {
            // Show the next button to advance the dialogue
            // if (nextButton != null) 
            // {
            //     ShowOptions(true);
            // }
        }

        public virtual void OnLineUpdate(string line)
        {
            if (speechText != null) 
            {
                speechText.text = line;
            }
        }

        public virtual void OnLineEnd()
        {
            if (speechText != null && background != null) 
            {
                ShowSpeechBubble(false);
                ShowOptions(false);
            }
        }

        public virtual void OnOptionsStart()
        {

        }

        public virtual void OnOptionsEnd()
        {

        }

        public virtual void OnCommand(string command)
        {

        }

        public void SetActive(bool setActive)
        {
            ShowSpeechBubble(setActive);
            ShowOptions(setActive);
        }

        protected virtual void ShowSpeechBubble(bool show) 
        {
            if (speechText != null)
                speechText.gameObject.SetActive(show);

            if (background != null)
                background.gameObject.SetActive(show);
        }

        protected virtual void ShowOptions(bool show) 
        {
            // nextButton.gameObject.SetActive(show);
        }
    }
}