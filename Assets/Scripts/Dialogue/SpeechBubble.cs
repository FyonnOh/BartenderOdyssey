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
        public GameObject nextButton;

        void Awake()
        {
            // Get the name of the NPC
            characterName = this.gameObject.GetComponentInParent<NPC>().characterName;
            Debug.Log($"Character name is {characterName}");

            // Hide the speech bubble and buttons when the game starts
            speechText?.enabled = false;
            background?.enabled = false;
            nextButton?.enabled = false;
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
            speechText?.enabled = true;
            background?.enabled = true;
            nextButton?.enabled = false;
        }

        // Use this for things like displaying response options
        // for the player to choose, or enabling input to continue
        // to the next line.
        public void OnLineFinishDisplaying()
        {
            // Show the next button to advance the dialogue
            nextButton?.enabled = true;
        }

        public void OnLineUpdate(string line)
        {
            speechText?.text = line;
        }

        public void OnLineEnd()
        {
            speechText?.enabled = false;
            background?.enabled = false;
            nextButton?.enabled = false;
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