using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CupLiquid : MonoBehaviour
{
    public GameObject liquid;
    private float fillSpeed = 0.006f;
    private float fill = 1.0f; // 1 when empty, 0 when full
    private float minFill = 0.53f;
    private float maxFill = 0.46f;

    public void fillCup()
    {
        fill -= fillSpeed;
        if (fill < 0.0f) fill = 0.0f;
        float newFillAmount = maxFill - (fill * (maxFill - minFill));
        liquid.GetComponent<Renderer>().sharedMaterial.SetFloat("_FillAmount", newFillAmount);
    }

    public float getFill()
    {
        return fill;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey("o"))
        {
            fillCup();
        }
        if (Input.GetKey("p"))
        {
            fill = 0.2f;
        }
    }
}
