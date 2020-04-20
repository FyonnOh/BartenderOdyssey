using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MixerCollider : MonoBehaviour
{

    public MixerScript Mixer;
    public void OnTriggerEnter(Collider other)
    {
        print("Object detected");
        if (other.name.Contains("Red"))
        {
            print("red pill");
            Mixer.increaseRed(other.name);
        }
        else if (other.name.Contains("Blue"))
        {
            Mixer.increaseBlue(other.name);
        }
    }

    public void OnTriggerExit(Collider other)
    {
        if (other.name.Contains("Red"))
        {
            Mixer.decreaseRed(other.name);
        }
        else if (other.name.Contains("Blue"))
        {
            Mixer.decreaseBlue(other.name);
        }
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
