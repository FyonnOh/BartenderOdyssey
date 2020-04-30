using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace Yarn.Unity.BartenderOdyssey {
    public class Scene7_Customer1 : MonoBehaviour
    {
        public GameObject waypoint_Entrance;
        public NavMeshAgent agent;
        
        public Animator anim;

        void Awake()
        {
            DialogueRunner dialogueRunner = FindObjectOfType<DialogueRunner>();
            dialogueRunner.AddCommandHandler("rotate_Customer1", Rotate_Customer1);
        }
        void Start()
        {
            anim = GetComponent<Animator>();
        }

        [YarnCommand("runToEntrance")]
        public void runToEntrance() {
            anim.SetTrigger("Run");
            agent.SetDestination(waypoint_Entrance.transform.position);
        }


        public void Rotate_Customer1(string[] parameters, System.Action onComplete) {
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
    }
}