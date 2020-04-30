using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Yarn.Unity.BartenderOdyssey {
    public class Scene2_Customer1 : MonoBehaviour
    {
        public Animator anim;
        public Animator sceneAnimator;
        public string continueButton = "ContinueDialogue";
        float bounce = 0.0f;
        float threshold = 0.1f;

        private bool hasStarted = false;
        void Awake()
        {
            DialogueRunner dialogueRunner = FindObjectOfType<DialogueRunner>();
        }
        void Start()
        {
            anim = GetComponent<Animator>();
        }

        void Update()
        {
            if(!(hasStarted)) {
                hasStarted = true;
                startDialogue();
            }

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

        public void startDialogue() {
            if (!FindObjectOfType<DialogueRunner>().isDialogueRunning)
            {
                FindObjectOfType<DialogueRunner>().StartDialogue(this.gameObject.GetComponent<NPC>().talkToNode);
            }
        }

        [YarnCommand("doPointingGesture")]
        public void doPointingGesture() {
            anim.SetTrigger("Point");
        }

        [YarnCommand("doIdle")]
        public void doIdle() {
            anim.SetTrigger("Idle");
        }

        [YarnCommand("loadNextLevel")]
        public void loadNextLevel() {
            StartCoroutine(LoadLevel(SceneManager.GetActiveScene().buildIndex));
        }

        IEnumerator LoadLevel(int levelIndex) {
            //play animation
            sceneAnimator.SetTrigger("Start");
            //wait
            yield return new WaitForSeconds(3);
            SceneManager.LoadScene(levelIndex + 1);
        }
    }
}