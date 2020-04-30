using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace Yarn.Unity.BartenderOdyssey {
    public class Scene3_BarOwner : MonoBehaviour
    {
        public GameObject waypoint_InFrontOfPlayer_2;
        public GameObject waypoint_Entrance;
        public NavMeshAgent agent_BarOwner;
        public float rotationSpeed = 3.0f;
        public float meleeRange = 3.0f;
        public Animator anim_BarOwner;
        public string continueButton = "ContinueDialogue";
        public GameObject player;
        float bounce = 0.0f;
        float threshold = 0.1f;
        private bool hasStarted = false;
        
        void Awake()
        {
            DialogueRunner dialogueRunner = FindObjectOfType<DialogueRunner>();
            dialogueRunner.AddCommandHandler("waitForMove_BarOwner", WaitForMove_BarOwner);
        }

        void Start()
        {
            anim_BarOwner = GetComponent<Animator>();
        }

        [YarnCommand("walkToPlayer")]
        public void walkToPlayer() {
            anim_BarOwner.SetTrigger("Walk");
            agent_BarOwner.SetDestination(waypoint_InFrontOfPlayer_2.transform.position);
        }

        [YarnCommand("walkToEntrance")]
        public void walkToEntrance(){
            anim_BarOwner.SetTrigger("Walk");
            agent_BarOwner.SetDestination(waypoint_Entrance.transform.position);
        }

        [YarnCommand("talkingAction")]
        public void talkingAction() {
            Debug.Log($"TALKINGACTION TRIFGGERED");
            anim_BarOwner.SetTrigger("Talk");
        }


        public void WaitForMove_BarOwner(string[] parameters, System.Action onComplete)
            {
                StartCoroutine(DoWaitForMove(onComplete));
            }

        private IEnumerator DoWaitForMove(System.Action onComplete)
        {
            while (agent_BarOwner.pathPending || agent_BarOwner.remainingDistance > agent_BarOwner.stoppingDistance)
            {
                //Debug.Log($"Remaining distance: {agent_BarOwner.remainingDistance}; stoppingDistance: {agent_BarOwner.stoppingDistance} Path pending: {agent_BarOwner.pathPending}");
                if (IsInMeleeRangeOf(player.transform)) {
                    RotateTowards(player.transform);
                }

                yield return null;
            }
            Debug.Log($"COMPLETE!");
            onComplete();
        }

        private bool IsInMeleeRangeOf (Transform target) {
            float distance = Vector3.Distance(transform.position, target.position);
            Debug.Log($"distance: {distance}, meleeRange: {meleeRange}, {distance < meleeRange}");
            return distance < meleeRange;
        }

        private void RotateTowards (Transform target) {
                Vector3 direction = (target.position - transform.position).normalized;
                Quaternion lookRotation = Quaternion.LookRotation(direction);
                transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * rotationSpeed);
            }
    }
}
