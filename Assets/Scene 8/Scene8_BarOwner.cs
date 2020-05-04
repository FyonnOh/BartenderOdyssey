using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace Yarn.Unity.BartenderOdyssey
{
    public class Scene8_BarOwner : MonoBehaviour
    {
        public GameObject breakupWaypoint;
        public GameObject besideJamesonWaypoint;
        public GameObject entrance;
        public NavMeshAgent agent;
        public Animator anim;
        // public GameObject player;
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
            dialogueRunner.AddCommandHandler("waitForWalk", WaitForMove);
            dialogueRunner.AddCommandHandler("turnRightTowardsPlayer", TurnRightTowardsPlayer);
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

        [YarnCommand("walkToJameson")]
        public void WalkToJameson()
        {
            anim.SetTrigger("Walk");
            agent.SetDestination(besideJamesonWaypoint.transform.position);
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
            // agent.isStopped = true;
            agent.velocity = Vector3.zero;
            onComplete();
        }

        [YarnCommand("point")]
        public void Point()
        {
            anim.SetTrigger("Point");
        }

        [YarnCommand("idle")]
        public void Idle()
        {
            anim.SetTrigger("Idle");
        }

        [YarnCommand("crouch")]
        public void Crouch()
        {
            anim.SetTrigger("Crouch");
        }

        [YarnCommand("riseFromCrouch")]
        public void RiseFromCrouch()
        {
            anim.SetTrigger("RiseFromCrouch");
        }

        [YarnCommand("angryPoint")]
        public void AngryPoint()
        {
            anim.SetTrigger("AngryPoint");
        }

        [YarnCommand("angryGesture")]
        public void AngryGesture()
        {
            anim.SetTrigger("AngryGesture");
        }

        [YarnCommand("shakeHead")]
        public void ShakeHead()
        {
            anim.SetTrigger("ShakeHead");
        }

        [YarnCommand("sigh")]
        public void Sigh()
        {
            anim.SetTrigger("Sigh");
        }

        [YarnCommand("leave")]
        public void Leave()
        {
            anim.SetTrigger("Walk");
            agent.SetDestination(entrance.transform.position);

        }

        public void TurnRightTowardsPlayer(string[] parameters, System.Action onComplete) {
            if (parameters.Length != 2)
            {
                Debug.LogErrorFormat("<<turnRightTowardsPlayer>> expects 2 parameters");
                onComplete();
                return;
            }

            if (float.TryParse(parameters[1], out float duration))
            {
                anim.SetTrigger("TurnRight");
                StartCoroutine(DoTurnRightTowardsPlayer(duration, onComplete));
            }
            else
            {
                Debug.LogErrorFormat($"Invalid number parameter {parameters[1]} for <<turnRightTowardsPlayer>>");
                onComplete();
            }
        }
        
        private IEnumerator DoTurnRightTowardsPlayer(float duration, System.Action onComplete)
        {
            Quaternion oldRotation = transform.rotation;
            // Vector3 direction =  player.transform.position - transform.position;
            // Debug.Log($"Player position: {player.transform.position}; BarOwner position: {transform.position}");
            // Debug.Log($"Direction vector: {direction}");
            // direction.x = direction.z = 0f;
            // direction.Normalize();

            Quaternion towards = Quaternion.Euler(transform.eulerAngles + (Vector3.up * 60));
            // Vector3 originalForward = transform.forward;

            for (float t = 0f; t < duration; t += Time.deltaTime)
            {
                transform.rotation = Quaternion.Slerp(oldRotation, towards, t / duration);
                transform.rotation = Quaternion.Euler(new Vector3(0f, transform.rotation.eulerAngles.y, 0f));
                yield return null;
            }

            onComplete();
        }
    }
}