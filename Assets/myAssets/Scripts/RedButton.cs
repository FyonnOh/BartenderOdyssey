using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RedButton : MonoBehaviour
{
    public GameObject downPos;
    public float returnSpeed;

    private Vector3 startPosition;
    private Color startColor;
    private Color downColor = Color.yellow;
    private bool isDown = false;

    public void Activate()
    {
        isDown = true;
        this.GetComponent<Renderer>().material.color = downColor;
        //transform.position = downPos.transform.position;
        //transform.position = Vector3.Lerp(startPosition, downPos.transform.position, Time.deltaTime * returnSpeed);
        print("Red Pressed");
    }

    // Start is called before the first frame update
    void Start()
    {
        startPosition = transform.position;
        startColor = this.GetComponent<Renderer>().material.color;
    }

    // Update is called once per frame
    void Update()
    {
        if (!isDown)
        {
            //transform.localPosition = Vector3.Lerp(transform.localPosition, startPosition, Time.deltaTime * returnSpeed);
            //transform.position = Vector3.Lerp(downPos.transform.position, startPosition, Time.deltaTime * returnSpeed);
            //this.GetComponent<Renderer>().material.color = startColor;
        }

        if (isDown)
        {
            this.GetComponent<Renderer>().material.color = downColor;
        }
    }

    void LateUpdate()
    {
        //isDown = false;
    }
}
