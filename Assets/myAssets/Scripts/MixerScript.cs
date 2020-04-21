using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MixerScript : MonoBehaviour
{
    // Recipes
    //List<string> redJuice = new List<string>();
    List<string> redJuice = new List<string>(new string[] { "Red", "Red", "Red" });

    //public GameObject mixerCollider;
    private int counterRed = 0;
    private int counterBlue = 0;
    private bool isCovered = false;
    private bool isDoneMixing = false;
    private float mixerTimer = 0.0f;

    public GameObject coverChild;
    public GameObject topCover;

    public ControllerGrabObject left;
    public ControllerGrabObject right;

    public AudioSource mixSound;
    private List<string> pillList = new List<string>();  

    private void initRedDrink()
    {
        redJuice.Add("Red");
        redJuice.Add("Red");
        redJuice.Add("Red");
    }

    private void initRecipes()
    {
        initRedDrink();
    }

    public void resetCounters()
    {
        counterBlue = 0;
        counterRed = 0;
    }
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


    public void addCover()
    {
        //coverChild.SetActive(true);
        topCover.transform.position = coverChild.transform.position;
        isCovered = true;
    }

    public void closeCover()
    {
        //coverChild.SetActive(false);
    }

    public void toggleCover()
    {
        if (isCovered)
        {
            closeCover();
            isCovered = false;
        } else
        {
            addCover();
            isCovered = true;
            isDoneMixing = false;
            
        }
    }

    public void removeMixerPills()
    {
        pillList.ForEach((removePill));
        pillList.Clear();
    }

    public void removePill(string name)
    {
        Destroy(GameObject.Find(name));
        print("removing pill");
    }

    private bool compareRecipe(List<string> A, List<string> B)
    {
        for (int i = 0; i < A.Count; i++)
        {
            if (A[i].Equals(B[i]))
            {
                return false;
            }
        }
        return true;
    }

    private void determineDrink()
    {
        if (pillList.Count == 3)
        {
            if (compareRecipe(pillList, redJuice))
            {
                setDrink("redJuice");
                return;
            }
        }
        setDrink("fail");
    }

    private void setDrink(string name)
    {
        switch (name)
        {
            case "redJuice":
                print("red juice made");
                break;
            case "fail":
                print("no good drink made");
                break;
        }
    }

    private void finishMixing()
    {
        isDoneMixing = true;
        mixerTimer = 0.0f;
        determineDrink();
        removeMixerPills();
        resetCounters();
    }

    private void checkComponentStatus()
    {
        // If player is holding both components in hand, then they are no longer combined
        if ((left.getObjName().Equals("Mixer") && right.getObjName().Equals("Top Cover")) ||
            (right.getObjName().Equals("Mixer") && left.getObjName().Equals("Top Cover")))
        {
            isCovered = false;
            isDoneMixing = false;
            mixerTimer = 0.0f;
            if (mixSound.isPlaying)
            {
                mixSound.Stop();
            }
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        //initRecipes();
    }

    // Update is called once per frame
    void Update()
    {
        if (isCovered)
        {
            topCover.transform.position = coverChild.transform.position;
            topCover.transform.rotation = coverChild.transform.rotation;

            if (!isDoneMixing)
            {
                if (!mixSound.isPlaying)
                {
                    mixSound.Play();
                }

                if (mixerTimer >= 5.0f)
                {
                    finishMixing();
                }
                mixerTimer += Time.deltaTime;
                print(mixerTimer);
            } 
            else
            {
                if (mixSound.isPlaying)
                {
                    mixSound.Stop();
                }
            }
        }

        if (left.getGrabStatus() && right.getGrabStatus())
        {
            checkComponentStatus();
        }

        if (Input.GetKeyDown(KeyCode.X))
        {
            toggleCover();
        }
    }
}
