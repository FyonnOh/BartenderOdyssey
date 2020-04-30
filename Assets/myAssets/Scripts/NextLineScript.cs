using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;
using Yarn.Unity;

public class NextLineScript : MonoBehaviour
{
    public SteamVR_Input_Sources handType; // 1
    public SteamVR_Behaviour_Pose controllerPose; // 2
    public SteamVR_Action_Boolean nextLineAction; // 3

    public LayerMask dialogueMask;

    private bool isHover = true;
    private bool isDown = false;

    // Laser Pointer Variables
    public GameObject laserePrefab;
    private GameObject laser;
    private Transform laserTransform;
    private Vector3 hitPoint;

    private void ContDialogue()
    {
        print("conting dialogue");
        if (FindObjectOfType<DialogueRunner>().isDialogueRunning)
        {
            FindObjectOfType<ExtendedDialogueUI>().MarkLineComplete();
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        // Spawn new laser and save a reference 'laser'
        laser = Instantiate(laserePrefab);
        laserTransform = laser.transform;
    }

    // Update is called once per frame
    void Update()
    {
        /*
        RaycastHit hit;
        if (Physics.Raycast(controllerPose.transform.position, transform.forward, out hit, 100, dialogueMask))
        {
            isHover = true;
            print("hovering");
        } else
        {
            isHover = false;
        }

        if (isHover)
        {
            print("buttonDown");
            if (nextLineAction.GetLastStateDown(handType))
            {
                ContDialogue();
                
                /*
                if (!isDown)
                {
                    ContDialogue();
                }*/ /*

            } else
            {
                //isDown = false;
            }

        }*/

        /*
        RaycastHit hit;
        Physics.Raycast(controllerPose.transform.position, transform.forward, out hit, 100);
        hitPoint = hit.point;
        ShowLaser(hit);

        if (Physics.Raycast(controllerPose.transform.position, transform.forward, 100, dialogueMask))
        {
            isHover = true;
            print("hovering");
        }
        else
        {
            isHover = false;
        }

        if (isHover)
        {
            print("buttonDown");
            if (nextLineAction.GetLastStateDown(handType))
            {
                ContDialogue();

            }
        }*/

        /*
        if (nextLineAction.GetLastStateDown(handType))
        {

            print("buttondown");

            RaycastHit hit;
            
            if (Physics.Raycast(controllerPose.transform.position, transform.forward, out hit, 100, dialogueMask))
            {
                hitPoint = hit.point;
                ShowLaser(hit);
                ContDialogue();
            } else
            {
                laser.SetActive(false);
            }
        
        }*/

        if (nextLineAction.GetLastStateDown(handType))
        {
            ContDialogue();
        }

    }

    private void ShowLaser(RaycastHit hit)
    {
        // Show the laser
        laser.SetActive(true);

        // Position the laser b/w controller and pt where raycast hits
        // Use 'lerp' cos you can give it two positions and the percent it should travel
        // If we pass in 0.5f, it returns the precise middle point
        laserTransform.position = Vector3.Lerp(controllerPose.transform.position, hitPoint, .5f);

        // Point laser at position where raycast hit
        laserTransform.LookAt(hitPoint);

        // Scale laser so it fits perfectly b/w the two positions
        laserTransform.localScale = new Vector3(laserTransform.localScale.x, laserTransform.localScale.y, hit.distance);
    }
}
