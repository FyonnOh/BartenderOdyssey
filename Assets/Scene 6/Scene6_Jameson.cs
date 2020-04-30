using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SceneManagement;

namespace Yarn.Unity.BartenderOdyssey {

    public class Scene6_Jameson : MonoBehaviour
    {
        public GameObject waypoint_Entrance;
        public GameObject waypoint_InFrontOfPlayer;
        public GameObject waypoint_InFrontOfPlayer_4;
        public NavMeshAgent agent;
        public float rotationSpeed = 3.0f;
        public float meleeRange = 3.0f;
        public Animator anim;
        public Animator sceneAnimator;
        public GameObject player;

        public GameObject customer1;
        public string continueButton = "ContinueDialogue";
        float bounce = 0.0f;
        float threshold = 0.1f;
        private bool isWalking = false;
        private bool hasStarted = false;

        void Awake()
        {
            DialogueRunner dialogueRunner = FindObjectOfType<DialogueRunner>();
            dialogueRunner.AddCommandHandler("waitForMove", WaitForMove);
            dialogueRunner.AddCommandHandler("rotate", Rotate);
        }
        void Start()
        {
            anim = GetComponent<Animator>();
        }

        // Update is called once per frame
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

        [YarnCommand("walkToPlayer")]
        public void walkToPlayer() {
            agent.SetDestination(waypoint_InFrontOfPlayer.transform.position);
        }

        [YarnCommand("headShakeAction")]
        public void headShakeAction() {
            anim.SetTrigger("HeadShake");
        }

        [YarnCommand("turnAround")]
        public void turnAround() {
            anim.SetTrigger("TurnAround");
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

        [YarnCommand("receivePunch1")]
        public void receivePunch1() {
            anim.SetTrigger("Punch1");
        }

        [YarnCommand("receivePunch2")]
        public void receivePunch2() {
            anim.SetTrigger("Punch2");
        }

        [YarnCommand("receivePunch3")]
        public void receivePunch3() {
            agent.SetDestination(waypoint_InFrontOfPlayer_4.transform.position);
            anim.SetTrigger("Punch3");
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

        private bool IsInMeleeRangeOf (Transform target) {
            float distance = Vector3.Distance(transform.position, target.position);
            return distance < meleeRange;
        }

        private void RotateTowards (Transform target) {
            Vector3 direction = (target.position - transform.position).normalized;
            Quaternion lookRotation = Quaternion.LookRotation(direction);
            transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * rotationSpeed);
        }

        public void Rotate(string[] parameters, System.Action onComplete) {
            StartCoroutine(RotateMe(Vector3.up * 90, 0.8f, onComplete));
        }
        IEnumerator RotateMe(Vector3 byAngles, float inTime, System.Action onComplete) 
        {    
            var fromAngle = transform.rotation;
            var toAngle = Quaternion.Euler(transform.eulerAngles + byAngles);
            for(var t = 0f; t < 1; t += Time.deltaTime/inTime) {
                transform.rotation = Quaternion.Slerp(fromAngle, toAngle, t);
                yield return null;
            }
            onComplete();
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
