using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Lock the button's axis
public class FreezeLocal : MonoBehaviour
{
    public bool lockX;
    public bool lockY;
    public bool lockZ;

    public float returnSpeed;

    public Color initialColor;
    public Color collidedColor;
    public ControllerGrabObject rightController;
    public ControllerGrabObject leftController;

    protected Vector3 startPosition;

    void Start()
    {
        // Remember start position of button
        startPosition = transform.localPosition;
        this.GetComponent<Renderer>().material.color = initialColor;
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 localPos = transform.localPosition;
        if (lockX && localPos.x != startPosition.x) localPos.x = startPosition.x;
        if (lockY && localPos.y != startPosition.y) localPos.y = startPosition.y;
        if (lockZ && localPos.z != startPosition.z) localPos.z = startPosition.z;
        transform.localPosition = localPos;
        // Return button to startPosition

        if (leftController.CollidingWith() != this.name && rightController.CollidingWith() != this.name) // only start the move back when the controllers are not touching
            transform.localPosition = Vector3.Lerp(transform.localPosition, startPosition, Time.deltaTime * returnSpeed);

        //if(((leftController.IsColliding() && leftController.CollidingWith().Contains(this.name)) || (rightController.IsColliding() && rightController.CollidingWith().Contains(this.name)))
        //    && this.GetComponent<Renderer>().material.color != collidedColor)
        //{
        //    this.GetComponent<Renderer>().material.color = collidedColor;
        //}
        //else
        //    this.GetComponent<Renderer>().material.color = initialColor;


        //this works better than collision based color change
        if (Vector3.Distance(transform.localPosition, startPosition) > 0.03 && this.GetComponent<Renderer>().material.color == initialColor)
        {
            this.GetComponent<Renderer>().material.color = collidedColor;
        }
        else if (Vector3.Distance(transform.localPosition, startPosition) <= 0.03 && this.GetComponent<Renderer>().material.color != initialColor)
            this.GetComponent<Renderer>().material.color = initialColor;

    }
}