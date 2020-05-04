using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SceneManagement;

namespace Yarn.Unity.BartenderOdyssey {
    public class Scene3_Jameson : MonoBehaviour
    {
        public GameObject waypoint_Entrance;
        public GameObject waypoint_InFrontOfPlayer;
        public NavMeshAgent agent;
        public float rotationSpeed = 3.0f;
        public float meleeRange = 3.0f;
        public Animator anim;
        public Animator sceneAnimator;
        public GameObject player;
        public string continueButton = "ContinueDialogue";
        float bounce = 0.0f;
        float threshold = 0.1f;
        private string currWaypoint = "entrance";
        private bool isWalking = false;
        private bool hasStarted = false;

        private bool isDrinksServed = false;

        void Awake()
        {
            DialogueRunner dialogueRunner = FindObjectOfType<DialogueRunner>();
            dialogueRunner.AddCommandHandler("waitForMove", WaitForMove);

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

        [YarnCommand("pointingAction")]
        public void pointingAction() {
            anim.SetTrigger("Point");
        }

        [YarnCommand("annoyedAction")]
        public void annoyedAction() {
            anim.SetTrigger("Annoyed");
        }

        [YarnCommand("dismissiveAction")]
        public void dismissiveAction() {
            anim.SetTrigger("Dismissive");
        }

        [YarnCommand("walkToPlayer")]
        public void walkToPlayer() {
            anim.SetTrigger("Walk");
            agent.SetDestination(waypoint_InFrontOfPlayer.transform.position);
        }

        [YarnCommand("walkToEntrance")]
        public void walkToEntrance(){
            anim.SetTrigger("Walk");
            agent.SetDestination(waypoint_Entrance.transform.position);
        }

        public void WaitForMove(string[] parameters, System.Action onComplete)
        {
            StartCoroutine(DoWaitForMove(onComplete));
        }

        private IEnumerator DoWaitForMove(System.Action onComplete)
        {
            while (agent.pathPending || agent.remainingDistance > agent.stoppingDistance)
            {
                //Debug.Log($"Remaining distance: {agent.remainingDistance}; stoppingDistance: {agent.stoppingDistance} Path pending: {agent.pathPending}");
                //rotate towards player
                if (IsInMeleeRangeOf(player.transform)) {
                    RotateTowards(player.transform);
                }

                yield return null;
            }
            onComplete();
        }

        public void drinksIsServed()
        {
            isDrinksServed = true;
        }

        public void WaitForDrinksServed(string[] parameters, System.Action onComplete)
        {
            StartCoroutine(DoWaitForDrinks(onComplete));
        }

        private IEnumerator DoWaitForDrinks(System.Action onComplete)
        {
            isDrinksServed = false;
            print(isDrinksServed);
            while (!isDrinksServed)
            {
                print(isDrinksServed);
                yield return null;
            }

            onComplete();
        }

        private bool IsInMeleeRangeOf (Transform target) {
            float distance = Vector3.Distance(transform.position, target.position);
            return distance < meleeRange;
        }

        private void RotateTowards (Transform target) {
            Vector3 direction = (target.position - transform.position).normalized;
            Quaternion lookRotation = Quaternion.LookRotation(direction);
            transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * rotationSpeed);
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
