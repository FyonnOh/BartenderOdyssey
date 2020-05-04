using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Yarn.Unity.BartenderOdyssey;

public class ButtonActivator_Scene2 : MonoBehaviour
{
    public PillDispenser pd;
    public GameObject customer1;

    public void OnTriggerEnter(Collider other)
    {
        pd.DispensePill();
        customer1.GetComponent<Scene2_Customer1>().mixTriggered();
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
