using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Yarn.Unity.BartenderOdyssey
{
    public class PlayerCharacter : MonoBehaviour
    {
        public string talkToKevinButton = "TalkToKevin";
        public string talkToArthurButton = "";
        public string continueButton = "ContinueDialogue";
        public float interactionRadius = 5.0f;

        float bounce = 0.0f;
        float threshold = 0.1f;

        // Update is called once per frame
        void Update()
        {
            // TODO: remove all player control when a dialogue is running?

            // Detect if we want to start a conversation
            if (Input.GetAxis(talkToKevinButton) == 1)
            {
                Debug.Log("Talking to Kevin");
                if (!FindObjectOfType<DialogueRunner>().isDialogueRunning)
                {
                    CheckForNearbyNPC("Kevin");
                }
            }

            // Debounce the continue button
            float now = Time.realtimeSinceStartup;
            if (now - bounce > threshold)
            {
                bounce = Time.realtimeSinceStartup;
                if (Input.GetAxis(continueButton) == 1)
                {
                    if (FindObjectOfType<DialogueRunner>().isDialogueRunning)
                    {
                        FindObjectOfType<ExtendedDialogueUI>().MarkLineComplete();
                    }
                }
            }
            
        }
        
        public void CheckForNearbyNPC(string targetNpc)
        {
            List<NPC> allParticipants = new List<NPC>(FindObjectsOfType<NPC>());
            NPC target = allParticipants.Find(p => 
                string.IsNullOrEmpty(p.talkToNode) == false  // has a conversation node?
                && p.characterName == targetNpc  // is it the NPC we're looking for?
                && (p.transform.position - this.transform.position) // is in range?
                    .magnitude <= interactionRadius
            );
            
            if (target != null)
            {
                FindObjectOfType<DialogueRunner>().StartDialogue(target.talkToNode);
            }
        } 
    }
}
