using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TopCoverCollider : MonoBehaviour
{
    public ControllerGrabObject left;
    public ControllerGrabObject right;
    public MixerScript Mixer;
    private bool isInRange = false;
    
    public void OnTriggerEnter(Collider other)
    {

        // Vibrate controller if the player is holding both mixer components, and
        // they are in range for combination, give the player feedback a simple vibrations
        if (left.getGrabStatus() && right.getGrabStatus())
        {
            if ((left.getObjName().Equals("Mixer") && right.getObjName().Equals("Top Cover")) ||
            (right.getObjName().Equals("Mixer") && left.getObjName().Equals("Top Cover")))
            {
                left.vibrateController();
                right.vibrateController();
                isInRange = true;
            }
        }
        
    }

    public void OnTriggerExit(Collider other)
    {
        isInRange = false;
    }

    public bool getStatus()
    {
        return isInRange;
    }


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
