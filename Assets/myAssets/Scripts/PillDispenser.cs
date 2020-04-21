using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PillDispenser : MonoBehaviour
{
    public Transform spawnPosition;
    public GameObject pill;
    public AudioSource buttonSound;

    private GameObject pillClone;
    private int pillCounter = 0;

    public void DispensePill()
    {
        //print("Dispensing Pill");
        buttonSound.Play();
        pillClone = Instantiate(pill, spawnPosition.position, spawnPosition.rotation);
        pillCounter += 1;
        pillClone.name = pill.name + pillCounter;
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
