using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.UI;
using Yarn.Unity.BartenderOdyssey;

namespace Yarn.Unity
{
    // Reworked version of YarnSpinner's DialogueUI.
    // This implementation supports multiple speech bubbles
    // by directing lines to the target speech bubble UI.
    public class ExtendedDialogueUI : Yarn.Unity.DialogueUIBehaviour
    {
        /// The object that contains the dialogue and the options.
        /** This object will be enabled when conversation starts, and
         * disabled when it ends.
         */
        public SpeechBubble[] dialogueContainers;  // public array for easy assignment from the inspector

        // String-indexed to make it easy to get the container for a character by name
        private IDictionary<string, SpeechBubble> _dialogueContainers;

        /// How quickly to show the text, in seconds per character
        [Tooltip("How quickly to show the text, in seconds per character")]
        public float textSpeed = 0.001f;

        /// The buttons that let the user choose an option
        public List<Button> optionButtons;

        // When true, the user has indicated that they want to proceed to
        // the next line.
        private bool userRequestedNextLine = false;

        // The method that we should call when the user has chosen an
        // option. Externally provided by the DialogueRunner.
        private System.Action<int> currentOptionSelectionHandler;

        // When true, the DialogueRunner is waiting for the user to press
        // one of the option buttons.
        private bool waitingForOptionSelection = false;     

        public UnityEngine.Events.UnityEvent onDialogueStart;

        public UnityEngine.Events.UnityEvent onDialogueEnd;  

        public UnityEngine.Events.UnityEvent onLineStart;
        public UnityEngine.Events.UnityEvent onLineFinishDisplaying;
        public DialogueRunner.StringUnityEvent onLineUpdate;
        public UnityEngine.Events.UnityEvent onLineEnd;

        public UnityEngine.Events.UnityEvent onOptionsStart;
        public UnityEngine.Events.UnityEvent onOptionsEnd;

        public DialogueRunner.StringUnityEvent onCommand;
        
        void Start ()
        {
            _dialogueContainers = new Dictionary<string, SpeechBubble>();

            // Start by hiding the container
            foreach (SpeechBubble container in dialogueContainers)
            {
                Debug.Log(container);
                _dialogueContainers.Add(container.characterName, container);
                // _dialogueContainers[container.characterName].SetActive(false);
            }

            foreach (var button in optionButtons) {
                button.gameObject.SetActive (false);
            }
        }

        public override Dialogue.HandlerExecutionType RunLine (Yarn.Line line, IDictionary<string,string> strings, System.Action onComplete)
        {
            // Start displaying the line; it will call onComplete later
            // which will tell the dialogue to continue
            StartCoroutine(DoRunLine(line, strings, onComplete));
            return Dialogue.HandlerExecutionType.PauseExecution;
        }

        /// Show a line of dialogue, gradually        
        private IEnumerator DoRunLine(Yarn.Line line, IDictionary<string,string> strings, System.Action onComplete) {
            // onLineStart?.Invoke();

            userRequestedNextLine = false;
            
            if (strings.TryGetValue(line.ID, out var text) == false) {
                Debug.LogWarning($"Line {line.ID} doesn't have any localised text.");
                text = line.ID;
            }

            string characterName = GetCharacterSpeaking(text);
            SpeechBubble speechBubble = _dialogueContainers[characterName];
            speechBubble.OnLineStart(text);
            text = RemoveCharacterName(text);

            if (textSpeed > 0.0f) {
                // Display the line one character at a time
                var stringBuilder = new StringBuilder ();

                foreach (char c in text) {
                    stringBuilder.Append (c);
                    // onLineUpdate?.Invoke(stringBuilder.ToString());
                    speechBubble.OnLineUpdate(stringBuilder.ToString());
                    if (userRequestedNextLine) {
                        // We've requested a skip of the entire line.
                        // Display all of the text immediately.
                        //onLineUpdate?.Invoke(text);
                        speechBubble.OnLineUpdate(text);
                        break;
                    }
                    yield return new WaitForSeconds (textSpeed);
                }
            } else {
                // Display the entire line immediately if textSpeed <= 0
                speechBubble.OnLineUpdate(text);
            }

            // We're now waiting for the player to move on to the next line
            userRequestedNextLine = false;

            // Indicate to the rest of the game that the line has finished being delivered
            speechBubble.OnLineFinishDisplaying();

            while (userRequestedNextLine == false) {
                yield return null;
            }

            // Avoid skipping lines if textSpeed == 0
            yield return new WaitForEndOfFrame();

            // Hide the text and prompt
            speechBubble.OnLineEnd();

            onComplete();

        }

        public override void RunOptions (Yarn.OptionSet optionsCollection, IDictionary<string,string> strings, System.Action<int> selectOption) {
            StartCoroutine(DoRunOptions(optionsCollection, strings, selectOption));
        }

        /// Show a list of options, and wait for the player to make a
        /// selection.
        public  IEnumerator DoRunOptions (Yarn.OptionSet optionsCollection, IDictionary<string,string> strings, System.Action<int> selectOption)
        {
            // Do a little bit of safety checking
            if (optionsCollection.Options.Length > optionButtons.Count) {
                Debug.LogWarning("There are more options to present than there are" +
                                 "buttons to present them in. This will cause problems.");
            }

            // Display each option in a button, and make it visible
            int i = 0;

            waitingForOptionSelection = true;

            currentOptionSelectionHandler = selectOption;
            
            foreach (var optionString in optionsCollection.Options) {
                optionButtons [i].gameObject.SetActive (true);

                // When the button is selected, tell the dialogue about it
                optionButtons [i].onClick.RemoveAllListeners();
                optionButtons [i].onClick.AddListener(() => SelectOption(optionString.ID));

                if (strings.TryGetValue(optionString.Line.ID, out var optionText) == false) {
                    Debug.LogWarning($"Option {optionString.Line.ID} doesn't have any localised text");
                    optionText = optionString.Line.ID;
                }

                var unityText = optionButtons [i].GetComponentInChildren<Text> ();
                if (unityText != null) {
                    unityText.text = optionText;
                }

                var textMeshProText = optionButtons [i].GetComponentInChildren<TMPro.TMP_Text> ();
                if (textMeshProText != null) {
                    textMeshProText.text = optionText;
                }

                i++;
            }

            onOptionsStart?.Invoke();

            // Wait until the chooser has been used and then removed 
            while (waitingForOptionSelection) {
                yield return null;
            }

            
            // Hide all the buttons
            foreach (var button in optionButtons) {
                button.gameObject.SetActive (false);
            }

            onOptionsEnd?.Invoke();

        }

        /// Run a command.
        public override Dialogue.HandlerExecutionType RunCommand (Yarn.Command command, System.Action onComplete) {
            // Dispatch this command via the 'On Command' handler.
            // onCommand?.Invoke(command.Text);
            
            // TODO: consider invoking the OnCommand handler for the currently active container

            // Signal to the DialogueRunner that it should continue executing.
            return Dialogue.HandlerExecutionType.ContinueExecution;
        }

        /// Called when the dialogue system has started running.
        public override void DialogueStarted ()
        {
            // Enable the dialogue controls.
            foreach (SpeechBubble container in _dialogueContainers.Values)
            {
                // container.SetActive(true);
            }

            // TODO: invoke OnDialogueStart in the appropriate container
            // onDialogueStart?.Invoke();
        }

        /// Called when the dialogue system has finished running.
        public override void DialogueComplete ()
        {
            // Hide the dialogue interface.
            foreach (SpeechBubble container in _dialogueContainers.Values)
            {
                container.OnDialogueEnd();
                container.SetActive(false);
            }
            
        }

        public void MarkLineComplete() {
            userRequestedNextLine = true;
        }

        public void SelectOption(int index) {
            if (waitingForOptionSelection == false) {
                Debug.LogWarning("An option was selected, but the dialogue UI was not expecting it.");
                return;
            }
            waitingForOptionSelection = false;
            currentOptionSelectionHandler?.Invoke(index);
        }

        // Returns the name of the character that's speaking
        private string GetCharacterSpeaking(string line) 
        {
            // Anything before a colon is the name of the character that's speaking
            const char delimiter = ':';

            if (!String.IsNullOrWhiteSpace(line))
            {
                int charLocation = line.IndexOf(delimiter);
                
                if (charLocation > 0)
                {
                    // Return the character's name excluding the colon.
                    return line.Substring(0, charLocation);
                }
            }

            // No colon was found, so we assume there's no character name. 
            // This shouldn't happen, so we raise the problem to the developer.
            Debug.LogError($"No character name in line \"{line}\".");
            return String.Empty;
        }

        // Removes the character's name in a dialogue script line
        // E.g. "Kevin: Hi there!" -> "Hi there!"
        private string RemoveCharacterName(string line)
        {
            const char delimiter = ':';

            if (!String.IsNullOrWhiteSpace(line))
            {
                int charLocation = line.IndexOf(delimiter);
                if (charLocation > 0)
                {
                    return line.Remove(0, charLocation + 2); // Remove the name + ": "
                }
            }

            // No colon, so there's no name to remove. Just return the whole line.
            return line;
        }
    }
}

