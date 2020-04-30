using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace Yarn.Unity.BartenderOdyssey
{
    public class Scene8_BarOwner : MonoBehaviour
    {
        public GameObject breakupWaypoint;
        public GameObject entrance;
        public NavMeshAgent agent;
        public Animator anim;
        public GameObject player;
        public float rotationSpeed = 0.5f;
        public float meleeRange = 0.5f;
        private bool hasStarted = false;
        public string continueButton = "ContinueDialogue";
        float bounce = 0.0f;
        float threshold = 0.1f;

        void Awake()
        {
            DialogueRunner dialogueRunner = FindObjectOfType<DialogueRunner>();
            dialogueRunner.AddCommandHandler("waitForRun", WaitForMove);
            dialogueRunner.AddCommandHandler("rotateBarOwner", RotateBarOwner);
        }

        void Start()
        {
            anim = this.GetComponent<Animator>();
        }

        void Update()
        {
            if (!hasStarted)
            {
                StartDialogue();
                hasStarted = true;
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

        private void StartDialogue()
        {
            if (!FindObjectOfType<DialogueRunner>().isDialogueRunning)
            {
                FindObjectOfType<DialogueRunner>().StartDialogue(this.gameObject.GetComponent<NPC>().talkToNode);
            }
        }

        [YarnCommand("runIn")]
        public void RunIn()
        {
            anim.SetTrigger("RunIn");
            agent.SetDestination(breakupWaypoint.transform.position);
        }

        public void WaitForMove(string[] parameters, System.Action onComplete)
        {
            StartCoroutine(DoWaitForMove(onComplete));
        }

        private IEnumerator DoWaitForMove(System.Action onComplete)
        {
            Debug.Log($"Remaining distance: {agent.remainingDistance}; Path pending: {agent.pathPending}");
            while (agent.pathPending || agent.remainingDistance > 0.5f)
            {
                //Debug.Log($"Remaining distance: {agent.remainingDistance}; Path pending: {agent.pathPending}");
                //rotate towards player
                // if (IsInMeleeRangeOf(breakupWaypoint.transform)) {
                //     Debug.Log($"Rotate towards player");
                //     RotateTowards(player.transform);
                // }

                yield return null;
            }
            onComplete();
        }

        [YarnCommand("breakUpFight")]
        public void BreakUpFight()
        {
            anim.SetTrigger("BreakUpFight");
        }

        [YarnCommand("shakeHead")]
        public void ShakeHead()
        {
            anim.SetTrigger("Shake");
        }

        [YarnCommand("leave")]
        public void Leave()
        {
            anim.SetTrigger("Walk");
            agent.SetDestination(entrance.transform.position);

        }

        public void RotateBarOwner(string[] parameters, System.Action onComplete) {
            StartCoroutine(RotateMe(Vector3.up * 90, 0.8f, onComplete));
        }
        IEnumerator RotateMe(Vector3 byAngles, float inTime, System.Action onComplete) 
        {    
            var fromAngle = transform.rotation;
            var toAngle = Quaternion.Euler(transform.eulerAngles + byAngles);
            for(var t = 0f; t < inTime; t += Time.deltaTime/inTime) {
                transform.rotation = Quaternion.Slerp(fromAngle, toAngle, t);
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
    }
}