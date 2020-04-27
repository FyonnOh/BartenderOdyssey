using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PourEndpointColliderScript : MonoBehaviour
{

    public void OnTriggerStay(Collider other)
    {
        /*
        if (other.name.Contains("DrinkCollider"))
        {
            other.transform.parent.gameObject.transform.Find("Drink").GetComponent<CupLiquid>().fillCup();
        }*/

        if (other.name.Contains("Drink") || other.name.Contains("DrinkCollider"))
        {
            other.transform.parent.gameObject.transform.Find("Drink").GetComponent<CupLiquid>().fillCup();
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
