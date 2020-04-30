using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityStandardAssets.Characters.ThirdPerson;
using Yarn.Unity;

public class WaypointControl : MonoBehaviour
{
    public GameObject waypoint1;
    public GameObject waypoint2;
    public NavMeshAgent agent;
    public ThirdPersonCharacter character;

    public float rotationSpeed = 10f;
    public float meleeRange = 4f;

    public GameObject player;
    private string currWaypoint = "entrance";

    static Animator anim;

    private bool isMoving = false;

    void Awake()
    {
        DialogueRunner dialogueRunner = FindObjectOfType<DialogueRunner>();

        if (dialogueRunner != null) {
            dialogueRunner.AddCommandHandler("waitForMove", WaitForMove);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
    }

    private bool IsInMeleeRangeOf(Transform target)
    {
        float distance = Vector3.Distance(transform.position, target.position);
        return distance < meleeRange;
    }

    private void RotateTowards(Transform target)
    {
        Vector3 direction = (target.position - transform.position).normalized;
        Quaternion lookRotation = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z));    // flattens the vector3
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * rotationSpeed);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButton("Jump"))
        {
                agent.SetDestination(waypoint1.transform.position);
                isMoving = true;
                currWaypoint = "waypoint1";
            /*else if (currWaypoint == "waypoint1")
            {
                agent.SetDestination(waypoint2.transform.position);
                isMoving = true;
                currWaypoint = "waypoint2";
            }*/
            
        }

        if (Input.GetKeyDown("a"))
        {
            agent.SetDestination(waypoint2.transform.position);
            isMoving = true;
            currWaypoint = "waypoint2";
        }

        if (Input.GetKey("s"))
        {
            anim.SetBool("isPointing", true);
            anim.SetBool("isIdle", false);
        } 
        else if (Input.GetKey("d"))
        {
            anim.SetBool("isHoldingDrink", true);
            anim.SetBool("isIdle", false);
            if (Input.GetKey("f"))
            {
                anim.SetBool("isDrinking", true);
            } else
            {
                anim.SetBool("isDrinking", false);
            }
        }
        else {
            anim.SetBool("isPointing", false);
            anim.SetBool("isDrinking", false);
            anim.SetBool("isHoldingDrink", false);
            anim.SetBool("isIdle", true);
        }

        /*
        if (isMoving) 
        {
            if (Vector3.Distance(waypoint.transform.position, transform.position) < agent.stoppingDistance)
            {
                agent.SetDestination(transform.position);
            }
            else
            {
                agent.SetDestination(waypoint.transform.position);
            }
        }*/

        if (currWaypoint == "waypoint1" && IsInMeleeRangeOf(player.transform))
        {
            RotateTowards(player.transform);
        }

        // if (agent.remainingDistance > agent.stoppingDistance)
        // {
        //     character.Move(agent.desiredVelocity, false, false);
        // } else
        // {
        //     character.Move(Vector3.zero, false, false);
        // }

    }

    [YarnCommand("moveToNextWaypoint")]
    public void MoveToNextWaypoint()
    {
        Debug.Log("Moving to next waypoint");
        if (currWaypoint == "waypoint1" || currWaypoint == "entrance")
        {
            agent.SetDestination(waypoint2.transform.position);
            isMoving = true;
            currWaypoint = "waypoint2";
        }
        else if (currWaypoint == "waypoint2")
        {
            agent.SetDestination(waypoint1.transform.position);
            isMoving = true;
            currWaypoint = "waypoint1";
        }
    }

    private void WaitForMove(string[] parameters, System.Action onComplete)
    {
        StartCoroutine(DoWaitForMove(onComplete));
    }

    private IEnumerator DoWaitForMove(System.Action onComplete)
    {
        while (agent.pathPending || agent.remainingDistance > 0.5f)
        {
            Debug.Log($"Remaining distance: {agent.remainingDistance}; Path pending: {agent.pathPending}");
            yield return null;
        }
        onComplete();
    }
}
