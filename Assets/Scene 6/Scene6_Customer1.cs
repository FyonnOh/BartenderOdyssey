using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SceneManagement;

namespace Yarn.Unity.BartenderOdyssey {
    public class Scene6_Customer1 : MonoBehaviour
    {
        public GameObject waypoint_Entrance;
        public GameObject waypoint_InFrontOfPlayer_2;
        public GameObject waypoint_InFrontOfPlayer_3;
        public NavMeshAgent agent;
        public Animator anim;
        public SceneLoader sceneLoader;
        private bool isHit = false;
        private bool _waitingForCupThrow = false;
        public bool IsWaitingForCupThrow 
        { 
            get { return _waitingForCupThrow; } 
        }

        void Awake()
        {
            DialogueRunner dialogueRunner = FindObjectOfType<DialogueRunner>();
            dialogueRunner.AddCommandHandler("waitForMove_Customer1", WaitForMove_Customer1);
            dialogueRunner.AddCommandHandler("rotate_Customer1", Rotate_Customer1);
            dialogueRunner.AddCommandHandler("rotateLeft", RotateLeft);
            dialogueRunner.AddCommandHandler("waitForHit_Customer1", WaitForHit_Customer1);
        }
        void Start()
        {
            anim = GetComponent<Animator>();
        }

        // Update is called once per frame
        void Update()
        {
            
        }

        [YarnCommand("walkToPlayer")]
        public void walkToPlayer() {
            agent.SetDestination(waypoint_InFrontOfPlayer_2.transform.position);
        }

        [YarnCommand("argueAction")]
        public void argueAction() {
            anim.SetTrigger("Argue");
        }

        [YarnCommand("punch1")]
        public void punch1() {
            agent.SetDestination(waypoint_InFrontOfPlayer_3.transform.position);
            anim.SetTrigger("Punch1");
        }

        [YarnCommand("punch2")]
        public void punch2() {
            anim.SetTrigger("Punch2");
        }

        [YarnCommand("punch3")]
        public void punch3() {
            anim.SetTrigger("Punch3");
        }

        public void WaitForMove_Customer1(string[] parameters, System.Action onComplete)
        {
            StartCoroutine(DoWaitForMove(onComplete));
        }

        private IEnumerator DoWaitForMove(System.Action onComplete)
        {
            while (agent.pathPending || agent.remainingDistance > agent.stoppingDistance)
            {
                //Debug.Log($"Remaining distance: {agent.remainingDistance}; stoppingDistance: {agent.stoppingDistance} Path pending: {agent.pathPending}");
                //rotate towards player
                // if (IsInMeleeRangeOf(player.transform)) {
                //     RotateTowards(player.transform);
                // }

                yield return null;
            }
            anim.SetTrigger("Idle");
            onComplete();
        }

        public void RotateLeft(string[] parameters, System.Action onComplete) {
            StartCoroutine(RotateMe(Vector3.up * -90, 0.8f, onComplete));
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

        [YarnCommand("startWaitingForCupThrow")]
        public void StartWaitingForCupThrow()
        {
            _waitingForCupThrow = true;
        }

        public void GetHit()
        {
            StartCoroutine(DoGetHit());
        }

        private IEnumerator DoGetHit()
        {
            isHit = true;
            anim.SetTrigger("Hit");
            yield return new WaitForSeconds(2);
            sceneLoader.LoadScene("8"); // scene index for Scene 7
        }

        public void WaitForHit_Customer1(string[] parameters, System.Action onComplete)
        {
            StartCoroutine(DoWaitForHit(onComplete));
        }

        private IEnumerator DoWaitForHit(System.Action onComplete)
        {

            while (!isHit)
            {
                yield return null;
            }

            onComplete();
        }
    }
}