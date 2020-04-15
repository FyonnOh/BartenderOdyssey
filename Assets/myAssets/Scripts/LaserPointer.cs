using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;

public class LaserPointer : MonoBehaviour
{

    public SteamVR_Input_Sources handType; // 1
    public SteamVR_Behaviour_Pose controllerPose; // 2
    public SteamVR_Action_Boolean teleportAction; // 3
    
    // Laser Pointer Variables
    public GameObject laserePrefab;
    private GameObject laser;
    private Transform laserTransform;
    private Vector3 hitPoint;
    public AudioSource teleportLoop;

    // Reticle Variables
    public Transform cameraRigTransform;
    public GameObject teleportReticlePrefab;
    private GameObject reticle;
    private Transform teleportReticleTransform;
    public Transform headTransform; // Stores a reference to the player's head
    public Vector3 teleportReticleOffset; // Reticle offset from floor, so no 'z-fighting' with other surfaces
    public LayerMask teleportMask; // Layer mask to filter areas on which teleports are allowed
    private bool shouldTeleport;
    public AudioSource teleportGo;


    // Start is called before the first frame update
    void Start()
    {
        // Spawn new laser and save a reference 'laser'
        laser = Instantiate(laserePrefab);
        laserTransform = laser.transform;


        reticle = Instantiate(teleportReticlePrefab);
        teleportReticleTransform = reticle.transform;
    }

    // Update is called once per frame
    void Update()
    {
        
        if (teleportAction.GetState(handType))
        {
            RaycastHit hit;

            // Shoot a ray from controller
            // If it hits something, store the point where it hits and show the laser
            if (Physics.Raycast(controllerPose.transform.position, transform.forward, out hit, 100, teleportMask))
            {
                hitPoint = hit.point;
                ShowLaser(hit);

                reticle.SetActive(true);
                teleportReticleTransform.position = hitPoint + teleportReticleOffset;
                shouldTeleport = true;
                playTeleportLoop();
            } else
            {
                // Hide laser if pointed location is non-teleportable
                laser.SetActive(false);
                reticle.SetActive(false);
                teleportLoop.Stop();
            }
        } else
        {
            // Hide laser when teleport action deactivates
            laser.SetActive(false);
            reticle.SetActive(false);
            teleportLoop.Stop();
        }

        // Teleport player if touchpad is released, and there's valid teleport pos
        if (teleportAction.GetLastStateUp(handType) && shouldTeleport)
        {
            Teleport();
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

    private void Teleport()
    {
        teleportGo.Play();
        shouldTeleport = false;
        reticle.SetActive(false);

        // Calculate difference b/w the position of camera rig's center and player's head
        Vector3 difference = cameraRigTransform.position - headTransform.position;

        // Reset y-pos for above diff to 0, because the calculation doesn't consider vertical pos of player's head
        difference.y = 0;

        // Move camera rig to the position of hit point and add calculated difference
        cameraRigTransform.position = hitPoint + difference;
        
    }

    private void playTeleportLoop()
    {
        if (teleportLoop.isPlaying)
        {
            return;
        } else
        {
            teleportLoop.Play();
        }
    }
}
