using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Yarn.Unity.BartenderOdyssey;

public class ServingScript_Scene1 : MonoBehaviour
{
    private bool isServed = false;
    private Color ogColor;
    public GameObject coaster;
    public GameObject barOwner;

    public void OnTriggerStay(Collider other)
    {
        if (other.name.Contains("Glass"))
        {
            checkGlass(other.gameObject);
        }
    }

    public void OnTriggerExit(Collider other)
    {
        restore();
    }

    public void checkGlass(GameObject glass)
    {
        //float fill = glass.GetComponent<CupLiquid>().getFill();
        float fill = glass.transform.Find("Drink").GetComponent<CupLiquid>().getFill();


        // Note: Fill is opposite, so if fill < 0.5, means it is more 
        // than half full
        if (fill < 0.5)
        {
            success();
        }
        else
        {
            failure();
        }
    }

    private void success()
    {
        coaster.GetComponent<Renderer>().material.color = Color.green;
        barOwner.GetComponent<Scene1_BarOwner_WayPointControl>().drinksIsServed();
    }

    private void failure()
    {
        coaster.GetComponent<Renderer>().material.color = Color.red;
    }

    private void restore()
    {
        coaster.GetComponent<Renderer>().material.color = ogColor;
    }

    // Start is called before the first frame update
    void Start()
    {
        ogColor = GetComponent<Renderer>().material.color;
    }

    // Update is called once per frame
    void Update()
    {

    }
}

