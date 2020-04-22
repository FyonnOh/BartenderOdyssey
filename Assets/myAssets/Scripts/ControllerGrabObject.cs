using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;
using Valve.VR.InteractionSystem;

public class ControllerGrabObject : MonoBehaviour
{

    public SteamVR_Input_Sources handType;
    public SteamVR_Behaviour_Pose controllerPose;
    public SteamVR_Action_Boolean grabAction;
    public GameObject ItemPosition;
    public bool isGrab = false;

    public SteamVR_Action_Vibration hapticAction;

    /*
    public float chargeDuration;
    private float chargeTimerStart;
    private float chargeTimerEnd;
    private float chargeTimer = 0.0f;
    public AudioSource chargeSound;
    public ParticleSystem chargeParticle;

    public SteamVR_Action_Boolean shootAction;
    public float maxShootSpeed;
    private float shootSpeed;
    public AudioSource gunFire;
    public ParticleSystem flareParticles;
    public ParticleSystem muzzleFlash;
    */

    public Shader highlightShader;
    public Shader standardShader;

    private Color startColor;
    private bool isHighlighted = false;
    private Color highlightColor = new Color(1f, 1f, 1f, 0.2f);

    private GameObject collidingObject;
    private GameObject objectInHand = null;
    private string colliderName = "";

    public RedButton redButton;
    public MixerScript mixerScript;
    public TopCoverCollider coverCollider;

    public string CollidingWith()
    {
        return colliderName;
    }

    private void SetCollidingObject(Collider col)
    {
        if (collidingObject || !col.GetComponent<Rigidbody>())
        {
            return;
        }

        collidingObject = col.gameObject;
        colliderName = col.transform.name;
    }

    public void OnTriggerEnter(Collider other)
    {
        SetCollidingObject(other);

        if (!isGrab && (other.gameObject.CompareTag("Grabbable") || other.gameObject.CompareTag("Button")))
        {
            //highlightObject();
            if (gameObject.name.Contains("right"))
                hapticAction.Execute(0.0f, 0.05f, 50.0f, 0.1f, SteamVR_Input_Sources.RightHand);
            if (gameObject.name.Contains("left"))
                hapticAction.Execute(0.0f, 0.05f, 50.0f, 0.1f, SteamVR_Input_Sources.LeftHand);
        }

        vibrateController();
    }

    public void OnTriggerStay(Collider other)
    {
        SetCollidingObject(other);

        if (other.gameObject.CompareTag("Grabbable") || other.gameObject.CompareTag("Button"))
        {
            //highlightObject();
        }
    }

    public void OnTriggerExit(Collider other)
    {
        colliderName = "";
        if (!collidingObject)
        {
            return;
        }

        if (other.gameObject.CompareTag("Grabbable") || other.gameObject.CompareTag("Button"))
        {
            //unhighlightObject();
        }

        collidingObject = null;
    }

    public void vibrateController()
    {
        if (gameObject.name.Contains("right"))
            hapticAction.Execute(0.0f, 0.05f, 50.0f, 0.1f, SteamVR_Input_Sources.RightHand);
        if (gameObject.name.Contains("left"))
            hapticAction.Execute(0.0f, 0.05f, 50.0f, 0.1f, SteamVR_Input_Sources.LeftHand);
    }


    private void highlightObject()
    {
        
        if (!isHighlighted)
        {
            //bulletShader = collidingObject.GetComponent<Renderer>().material.shader;
            //collidingObject.GetComponent<Renderer>().material.shader = highlightShader;
            startColor = collidingObject.GetComponent<Renderer>().material.color;
            //collidingObject.GetComponent<Renderer>().material.shader = highlightShader;
            isHighlighted = true;
            collidingObject.GetComponent<Renderer>().material.color = highlightColor;
        }
    }

    private void unhighlightObject()
    {
        //collidingObject.GetComponent<Renderer>().material.shader = standardShader;
        isHighlighted = false;
        collidingObject.GetComponent<Renderer>().material.color = startColor;
    }
    private void GrabObject()
    {
        //print("grabbing object...");
        isGrab = true;
        objectInHand = collidingObject;
        collidingObject = null;
        objectInHand.transform.position = ItemPosition.transform.position;

        var joint = AddFixedJoint();
        joint.connectedBody = objectInHand.GetComponent<Rigidbody>();
    }

    public string getObjName()
    {
        return objectInHand.name;
    }

    public bool getGrabStatus()
    {
        return isGrab;
    }

    private FixedJoint AddFixedJoint()
    {
        FixedJoint fx = gameObject.AddComponent<FixedJoint>();
        fx.breakForce = 20000;
        fx.breakTorque = 20000;
        return fx;
    }

    private void ReleaseObject()
    {
        // Make sure there is fixed joint attached to controller
        if (GetComponent<FixedJoint>())
        {
            GetComponent<FixedJoint>().connectedBody = null;
            Destroy(GetComponent<FixedJoint>());
            isGrab = false;

            if ((objectInHand.name.Equals("Mixer") || objectInHand.name.Equals("Top Cover")) && coverCollider.getStatus())
            {
                mixerScript.addCover();
            }

            objectInHand.GetComponent<Rigidbody>().velocity = controllerPose.GetVelocity();
            objectInHand.GetComponent<Rigidbody>().angularVelocity = controllerPose.GetAngularVelocity();
        }
    }

    private void ActivateButton()
    {
        if (colliderName.Contains("Red"))
        {
            print("red button activated!");
            redButton.Activate();
        }
    }

    /*
    private void getShootSpeed()
    {
        float chargeDiff = chargeTimerEnd - chargeTimerStart;
        float maxChargeDiff = chargeDuration;
        print("chargeDiff = " + chargeDiff);

        if (chargeDiff >= maxChargeDiff)
        {
            chargeDiff = maxChargeDiff;
        }

        shootSpeed = chargeDiff / maxChargeDiff * maxShootSpeed;
        print("shootSpeed = " + shootSpeed);
        chargeTimer = 0; 
    }

    private void ShootObject()
    {
        // Make sure there is fixed joint attached to controller
        if (GetComponent<FixedJoint>())
        {
            if (chargeSound.isPlaying)
            {
                chargeSound.Stop();
                print("Charge sound stop");
            }

            chargeTimerEnd = chargeTimer;
            chargeParticle.Stop();
            gunFire.Play();
            muzzleFlash.Play();
            GetComponent<FixedJoint>().connectedBody = null;
            Destroy(GetComponent<FixedJoint>());
            isGrab = false;

            Vector3 gunDir = new Vector3(controllerPose.transform.forward.x, controllerPose.transform.forward.y, controllerPose.transform.forward.z);

            getShootSpeed();

            objectInHand.GetComponent<Rigidbody>().AddForce(gunDir * shootSpeed);

            flareParticles.Play();
        }
    }

    private void Charge()
    {
        chargeTimerStart = chargeTimer;
        chargeSound.time = 1.5f;
        chargeSound.Play();
        chargeParticle.Play();
        hapticAction.Execute(0, chargeDuration, 150, 0.25f, handType);
        print("vibrating!");
    }
    */

    void Start()
    {
        //bulletShader = GetComponent<Renderer>().material.shader;
        print("highlighting..." + startColor);
    }

    // Update is called once per frame
    void Update()
    {

        //chargeTimer += Time.deltaTime;

        if (Input.GetKeyDown(KeyCode.Space))
        {
            print(collidingObject);
        }

        if (Input.GetKeyDown(KeyCode.X))
        {
            mixerScript.toggleCover();
        }

        if (collidingObject)
        {
            if (grabAction.GetLastStateDown(handType))
            {
                if (collidingObject.gameObject.CompareTag("Grabbable"))
                {
                    GrabObject();
                } else if (collidingObject.gameObject.CompareTag("Button"))
                {
                    print("Button Tag Activated");
                    ActivateButton();
                }
                
            }
        }

        if (grabAction.GetLastStateUp(handType))
        {
            if (objectInHand)
            {
                ReleaseObject();
            }
        }

        if (isGrab)
        {
            /*
            if (shootAction.GetLastStateDown(handType))
            {
                Charge();
            } 
            else if (shootAction.GetLastStateUp(handType))
            {
                ShootObject();
            }
            */
        }

    }
}
