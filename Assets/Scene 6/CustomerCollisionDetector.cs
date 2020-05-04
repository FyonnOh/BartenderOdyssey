using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Yarn.Unity.BartenderOdyssey;

public class CustomerCollisionDetector : MonoBehaviour
{
    public GameObject customer1;
    private string throwCupDebug = "ThrowCupDebug";

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetAxis(throwCupDebug) == 1 && customer1.GetComponent<Scene6_Customer1>().IsWaitingForCupThrow)
        {
            Debug.Log("throw cup debug");
            customer1.GetComponent<Scene6_Customer1>().GetHit();
        }
    }

    public void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Grabbable") && customer1.GetComponent<Scene6_Customer1>().IsWaitingForCupThrow)
        {
            customer1.GetComponent<Scene6_Customer1>().GetHit();
        }
    }
}
