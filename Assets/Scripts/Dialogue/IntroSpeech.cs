using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace Yarn.Unity.BartenderOdyssey 
{
    public class IntroSpeech : SpeechBubble
    {
        public TextMeshProUGUI introText;

        [HideInInspector]
        public bool IsLineComplete { get; set; }

        // public virtual Component SpeechText 
        // { 
        //     get { return this._speechText.GetComponent<TextMeshPro>(); } 
        //     set { this._speechText = value; }
        // }

        public override void OnLineStart(string fullText = "")
        {
            if (introText != null) 
            {
                IsLineComplete = false;
                ShowSpeechBubble(true);
                ShowOptions(false);
            }
        }

        public override void OnLineUpdate(string line)
        {
            if (this.introText != null)
            {
                // Ensure there is whitespace at the end of current sentence 
                // before appending the next sentence.
                if (!string.IsNullOrEmpty(introText.text))
                {
                    if (this.introText.text.Substring(this.introText.text.Length - 1, 1) == ".")
                    {
                        this.introText.text += " ";
                    }
                }

                // Append the latest character to the text
                string lastChar = line.Length <= 1 ? line : line.Substring(line.Length - 1, 1);
                this.introText.text += lastChar;
            }
        }

        public override void OnLineFinishDisplaying()
        {
            IsLineComplete = true;
        }

        public override void OnLineEnd()
        {
            
        }


        protected override void ShowSpeechBubble(bool show) 
        {
            if (introText != null)
                introText.gameObject.SetActive(show);
        }
    }
}

