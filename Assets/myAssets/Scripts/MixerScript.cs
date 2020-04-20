using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MixerScript : MonoBehaviour
{
    //public GameObject mixerCollider;
    private int counterRed = 0;
    private int counterBlue = 0;
    private bool isCovered = false;
    private bool isDoneMixing = false;
    private float mixerTimer = 0.0f;
    public AudioSource mixSound;
    private List<string> pillList;  

    public void increaseRed(string name)
    {
        counterRed += 1;
        pillList.Add(name);
        print(counterRed);
    }

    public void increaseBlue(string name)
    {
        counterBlue += 1;
        pillList.Add(name);
    }

    public void decreaseRed(string name)
    {
        counterRed -= 1;
        pillList.Remove(name);
    }

    public void decreaseBlue(string name)
    {
        counterBlue -= 1;
        pillList.Remove(name);
    }

    public void removePills()
    {
        pillList.ForEach((removePill));
        pillList.Clear();
    }

    public void removePill(string name)
    {
        Destroy(GameObject.Find(name));
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (isCovered)
        {
            if (!mixSound.isPlaying)
            {
                mixSound.Play();
            }
            
            if (mixerTimer == 5.0f)
            {
                isDoneMixing = true;
                mixerTimer = 0.0f;
            }
            mixerTimer += Time.deltaTime;
        }
    }
}
