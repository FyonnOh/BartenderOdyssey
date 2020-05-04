using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Yarn.Unity.BartenderOdyssey 
{
    public class Scene8_Jameson : MonoBehaviour
    {
        public GameObject entrance;
        public GameObject waypoint_InFrontOfPlayer;
        public UnityEngine.AI.NavMeshAgent agent;
        public float rotationSpeed = 3.0f;
        public float meleeRange = 3.0f;
        public Animator anim;
        public Animator sceneAnimator;
        public GameObject player;
        public string continueButton = "ContinueDialogue";
        float bounce = 0.0f;
        float threshold = 0.1f;
        private bool hasStarted = false;

        void Awake()
        {
            DialogueRunner dialogueRunner = FindObjectOfType<DialogueRunner>();
            dialogueRunner.AddCommandHandler("turnLeftTowardsPlayer", TurnLeftTowardsPlayer);
        }

        void Start()
        {
            anim = GetComponent<Animator>();
        }

        [YarnCommand("standUp")]
        public void StandUp() {
            anim.SetTrigger("StandUp");
        }

        [YarnCommand("injuredWalkLeave")]
        public void InjuredWalkLeave()
        {
            anim.SetTrigger("InjuredWalk");
            agent.SetDestination(entrance.transform.position);

        }

        public void TurnLeftTowardsPlayer(string[] parameters, System.Action onComplete) {
            if (parameters.Length != 2)
            {
                Debug.LogErrorFormat("<<turnLeftTowardsPlayer>> expects 2 parameters");
                onComplete();
                return;
            }

            if (float.TryParse(parameters[1], out float duration))
            {
                anim.SetTrigger("TurnLeft");
                StartCoroutine(DoTurnLeftTowardsPlayer(duration, onComplete));
            }
            else
            {
                Debug.LogErrorFormat($"Invalid number parameter {parameters[1]} for <<turnLeftTowardsPlayer>>");
                onComplete();
            }
        }
        
        private IEnumerator DoTurnLeftTowardsPlayer(float duration, System.Action onComplete)
        {
            Quaternion oldRotation = transform.rotation;
            Vector3 direction =  player.transform.position - transform.position;
            // Debug.Log($"Player position: {player.transform.position}; BarOwner position: {transform.position}");
            // Debug.Log($"Direction vector: {direction}");
            direction.x = direction.z = 0f;
            // direction.Normalize();

            Quaternion towards = Quaternion.Euler(transform.eulerAngles + (Vector3.up * -60));

            Vector3 originalForward = transform.forward;

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