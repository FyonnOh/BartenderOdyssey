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
        [Tooltip("Start time for typing audio clip")]
        public float startTime = 0f;
        [HideInInspector]
        public bool IsLineComplete { get; set; }
        private AudioClip typingClip;
        private AudioSource audioSource;

        void Start()
        {
            audioSource = gameObject.GetComponent<AudioSource>();
            Debug.Log(SoundEffects.TypingClips[1]);

            // Load "Keyboard-Typing-05"
            typingClip = Resources.Load<AudioClip>(SoundEffects.TypingClips[1]);
            audioSource.clip = typingClip;
            Debug.Log(typingClip);
        }

        public override void OnLineStart(string fullText = "")
        {
            if (introText != null) 
            {
                IsLineComplete = false;
                ShowSpeechBubble(true);
                ShowOptions(false);
            }

            audioSource.Stop();
            audioSource.Play();
            audioSource.time = startTime;
            audioSource.loop = true;
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
            audioSource.Stop();
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

