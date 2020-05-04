using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Yarn.Unity.BartenderOdyssey 
{
    public class Scene8_Customer1 : MonoBehaviour
    {
        public GameObject waypoint_Entrance;
        public UnityEngine.AI.NavMeshAgent agent;
        
        public Animator anim;

    void Awake()
    {
        DialogueRunner dialogueRunner = FindObjectOfType<DialogueRunner>();
        dialogueRunner.AddCommandHandler("turnTowardsEntrance", Rotate_Customer1);
    }
    void Start()
    {
        anim = GetComponent<Animator>();
    }

    void Update()
    {
        // Debug.Log($"acceleration: {agent.acceleration}; speed: {agent.speed}; velocity: {agent.velocity}");
    }

    [YarnCommand("startled")]
    public void Startled()
    {
        anim.SetTrigger("Startled");
    }

    [YarnCommand("runToEntrance")]
    public void runToEntrance() {
        anim.SetTrigger("Run");
        agent.SetDestination(waypoint_Entrance.transform.position);
    }


    public void Rotate_Customer1(string[] parameters, System.Action onComplete) {
        anim.SetTrigger("TurnRight");
        StartCoroutine(RotateMe(Vector3.up * 150, 0.8f, onComplete));
    }
    IEnumerator RotateMe(Vector3 byAngles, float inTime, System.Action onComplete) 
    {    
        var fromAngle = transform.rotation;
        var toAngle = Quaternion.Euler(transform.eulerAngles + byAngles);
        for(var t = 0f; t < 1; t += Time.deltaTime/inTime) {
            transform.rotation = Quaternion.Slerp(fromAngle, toAngle, t);
            // Debug.Log($"Rotation: {transform.eulerAngles}");
            yield return null;
        }
        onComplete();
    }
    }
}