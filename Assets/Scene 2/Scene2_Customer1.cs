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

        private bool isDrink1Served = false;
        private bool isDrink2Served = false;
        private bool isMixTriggered = false;

        private bool hasStarted = false;
        void Awake()
        {
            DialogueRunner dialogueRunner = FindObjectOfType<DialogueRunner>();

            dialogueRunner.AddCommandHandler("waitForMixingTrigger", WaitForMixingTrigger);
            dialogueRunner.AddCommandHandler("waitForDrinksServed", WaitForDrinksServed);
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

        public void drink1IsServed()
        {
            isDrink1Served = true;
        }

        public void drink2IsServed()
        {
            isDrink2Served = true;
        }

        public void WaitForDrinksServed(string[] parameters, System.Action onComplete)
        {
            StartCoroutine(DoWaitForDrinks(onComplete));
        }

        private IEnumerator DoWaitForDrinks(System.Action onComplete)
        {
            isDrink1Served = false;
            isDrink2Served = false;
            while (!(isDrink1Served && isDrink2Served))
            {
                yield return null;
            }

            onComplete();
        }
        public void mixTriggered()
        {
            isMixTriggered = true;
        }

        public void WaitForMixingTrigger(string[] parameters, System.Action onComplete)
        {
            StartCoroutine(DoWaitForTrigger(onComplete));
        }

        private IEnumerator DoWaitForTrigger(System.Action onComplete)
        {
            isMixTriggered = false;
            while (!isMixTriggered)
            {
                yield return null;
            }

            onComplete();
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