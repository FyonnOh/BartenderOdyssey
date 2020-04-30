using System;
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
        
        public Text sizerText;
        public Image background;
        private List<AudioClip> clips;

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

            clips = new List<AudioClip>();
        }

        void Start()
        {
            // SetSpeechStyle(SoundEffectType.Talking);
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

            AudioManager.instance.SpeakWordsOnLoop(clips);
        }

        // Use this for things like displaying response options
        // for the player to choose, or enabling input to continue
        // to the next line.
        public virtual void OnLineFinishDisplaying()
        {
            AudioManager.instance.StillSpeaking = false;
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

        [YarnCommand("setSpeechStyle")]
        public void SetSpeechStyle(string style)
        {
            if (Enum.TryParse(style, out SoundEffectType set))
            {
                SetSpeechStyle(set);
            }
            else
            {
                Debug.LogError($"No matching speech style for {style}");
            }
        }

        public void SetSpeechStyle(SoundEffectType style)
        {
            string[] clipsToLoad = null;
            switch (style)
            {
                case SoundEffectType.Typing:
                    clipsToLoad = SoundEffects.TypingClips;
                    break;
                case SoundEffectType.Talking:
                    clipsToLoad = SoundEffects.TalkingClips;
                    break;
                default:
                    Debug.Log($"Invalid speech style {style}");
                    break;
            }
            
            if (clipsToLoad != null)
            {
                clips.Clear();
                foreach (string clipFile in clipsToLoad)
                {
                    clips.Add(Resources.Load<AudioClip>(clipFile));
                }
            }
        }
    }
}