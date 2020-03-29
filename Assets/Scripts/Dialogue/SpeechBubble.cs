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
            // Hide the speech text when the game starts
            if (speechText != null)
            {
                speechText.enabled = false;
                background.enabled = false;
            }
            else
            {
                Debug.LogError("There is no text UI element registered to the speech bubble.");
            }
        }

        public void OnDialogueStart()
        {
            // Default implementation is empty.
            // You can use this to disable controls during
            // a dialogue.
        }

        public void OnDialogueEnd()
        {
            // Default implementation is empty.
            // You can use this to re-enable controls after
            // a dialogue.
        }

        public void OnLineStart()
        {
            if (speechText != null)
            {
                speechText.enabled = true;
                background.enabled = true;
            }
        }

        public void OnLineFinishDisplaying()
        {
            // Default implementation is empty.
            // Use this for things like displaying response options
            // for the player to choose, or enabling input to continue
            // to the next line.
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
            if (speechText != null)
            {
                speechText.enabled = false;
                background.enabled = false;
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
            this.gameObject.SetActive(setActive);
        }
    }
}