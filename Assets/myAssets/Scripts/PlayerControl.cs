using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControl : MonoBehaviour
{
    public float speed;
    public float sprintSpeed;
    //public float jumpSpeed;
    //public GameObject camera;

    private Rigidbody rb;
    private Vector3 movement;
    private float finalSpeed;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    void handleMovement()
    {
        if (Input.GetKey(KeyCode.A))
        {
            //movement += rb.transform.right;
            transform.position += speed * transform.right;
        }

        if (Input.GetKey(KeyCode.D))
        {
            //movement += -rb.transform.right;
            transform.position += speed * -transform.right;
        }

        if (Input.GetKey(KeyCode.W))
        {
            //movement += rb.transform.forward;
            transform.position += speed * transform.forward;
        }

        if (Input.GetKey(KeyCode.S))
        {
            //movement += -rb.transform.forward;
            transform.position += speed * -transform.forward;
        }
    }

    void handleSprint()
    {
        if (Input.GetKey(KeyCode.LeftShift))
        {
            finalSpeed = sprintSpeed;
        } else
        {
            finalSpeed = speed;
        }
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        movement = new Vector3(0, 0, 0);

        /*
        if (Input.GetKey(KeyCode.A))
        {
            //transform.Translate(Vector3.left * speed * Time.deltaTime);
            transform.position = transform.position + Camera.main.transform.right * -1 * speed * Time.deltaTime;
        }

        if (Input.GetKey(KeyCode.D))
        {
            //transform.Translate(Vector3.right * speed * Time.deltaTime); ;
            transform.position = transform.position + Camera.main.transform.right * speed * Time.deltaTime;
        }

        if (Input.GetKey(KeyCode.W))
        {
            //transform.Translate(Vector3.forward * speed * Time.deltaTime);
            transform.position = transform.position + Camera.main.transform.forward * speed * Time.deltaTime;
        }

        if (Input.GetKey(KeyCode.S))
        {
            //transform.Translate(Vector3.back * speed * Time.deltaTime);
            transform.position = transform.position + Camera.main.transform.forward * -1 * speed * Time.deltaTime;
        }
        */

        // rb.AddForce(movement * speed);

        /*
        float moveHorizontal = Input.GetAxis("Horizontal");
        float moveVertical = Input.GetAxis("Vertical");

        Vector3 movement = new Vector3(moveHorizontal, 0.0f, moveVertical);

        rb.AddForce(movement * speed);
        */

        handleMovement();
        handleSprint();
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Pick Up"))
        {
            other.gameObject.SetActive(false);
        }
    }


    void LateUpdate()
    {
        //rb.AddForce(movement * finalSpeed);
    }

}
