using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PillDispenser : MonoBehaviour
{
    public Transform spawnPosition;
    public GameObject pill;
    public AudioSource buttonSound;

    public void DispensePill()
    {
        //print("Dispensing Pill");
        buttonSound.Play();
        Instantiate(pill, spawnPosition.position, spawnPosition.rotation);
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown("z"))
        {
            //print("z detected");
            DispensePill();
        }
    }
}
