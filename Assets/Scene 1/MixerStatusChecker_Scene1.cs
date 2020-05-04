using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Yarn.Unity.BartenderOdyssey;

public class MixerStatusChecker_Scene1 : MonoBehaviour
{
    public GameObject barOwner;
    public GameObject Mixer;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Mixer.GetComponent<MixerScript>().getIsCovered())
        {
            barOwner.GetComponent<Scene1_BarOwner_WayPointControl>().closeMixer();
        }
    }
}
