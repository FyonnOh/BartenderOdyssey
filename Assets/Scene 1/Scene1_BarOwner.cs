using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scene1_BarOwner : MonoBehaviour
{
    private Rigidbody rb;
    private Vector3 movement;
    private float finalSpeed;
    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
