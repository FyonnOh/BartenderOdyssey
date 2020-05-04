using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Yarn.Unity.BartenderOdyssey;

public class RedButtonActivator : MonoBehaviour
{
    public PillDispenser pd;
    public GameObject barowner;

    public void OnTriggerEnter(Collider other)
    {
        pd.DispensePill();
        barowner.GetComponent<Scene1_BarOwner_WayPointControl>().pushRedButton();
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
