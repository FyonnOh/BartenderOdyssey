using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Yarn.Unity.BartenderOdyssey;

public class GlassButtonActivator_Scene1 : MonoBehaviour
{
    public PillDispenser pd;
    public GameObject barowner;

    public void OnTriggerEnter(Collider other)
    {
        pd.DispensePill();
        barowner.GetComponent<Scene1_BarOwner_WayPointControl>().pushGlassButton();
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
