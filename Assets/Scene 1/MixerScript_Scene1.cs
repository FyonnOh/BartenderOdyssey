using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Yarn.Unity.BartenderOdyssey;

public class MixerScript_Scene1 : MonoBehaviour
{
    // Recipes
    //List<string> redJuice = new List<string>();
    List<string> redJuice = new List<string>(new string[] { "Red", "Red", "Red" });

    private string currentDrink = null;

    //public GameObject mixerCollider;
    private int counterRed = 0;
    private int counterBlue = 0;
    private bool isCovered = false;
    private bool isDoneMixing = false;
    private float mixerTimer = 0.0f;

    public GameObject coverChild;
    public GameObject topCover;
    public GameObject nozzle;

    public ControllerGrabObject left;
    public ControllerGrabObject right;

    public AudioSource mixSound;
    private List<string> pillList = new List<string>();

    public ParticleSystem mixParticle;
    public ParticleSystem destroyParticle;

    public GameObject barOwner;

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
        //topCover.transform.position = coverChild.transform.position;
        topCover.transform.position = coverChild.transform.position;
        //topCover.SetActive(false);

        topCover.transform.Find("Normal Cover").GetComponent<BoxCollider>().enabled = false;
        topCover.transform.Find("Normal Cover").GetComponent<MeshRenderer>().enabled = false;

        coverChild.SetActive(true);
        isCovered = true;

        
    }

    public void removeCover()
    {
        if (isDoneMixing) destroyParticle.Play();
        if (mixSound.isPlaying) mixSound.Stop();
        if (mixParticle.isPlaying) mixParticle.Stop();
        //topCover.SetActive(true);

        topCover.transform.Find("Normal Cover").GetComponent<BoxCollider>().enabled = true;
        topCover.transform.Find("Normal Cover").GetComponent<MeshRenderer>().enabled = true;

        coverChild.SetActive(false);
        isCovered = false;
        isDoneMixing = false;
        mixerTimer = 0.0f;
        // TODO: Spawn a 'used up thing'
        GetComponent<PourDetector>().enabled = false;
        currentDrink = null;
        nozzle.SetActive(false);
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
        }
        else
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
                setDrink("Red Juice");
                return;
            }
        }
        setDrink("fail");
    }

    private void setDrink(string name)
    {
        switch (name)
        {
            case "Red Juice":
                print("red juice made");
                currentDrink = "Red Juice";
                GetComponent<PourDetector>().enabled = true;
                // TODO: Play success sound
                break;
            case "fail":
                print("no good drink made");
                // TODO: Play fail sound
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
        //GetComponent<PourDetector>().enabled = true;
        nozzle.SetActive(true);
    }

    private void checkComponentStatus()
    {
        // If player is holding both components in hand, then they are no longer combined
        if ((left.getObjName().Equals("Mixer") && right.getObjName().Equals("Top Cover")) ||
            (right.getObjName().Equals("Mixer") && left.getObjName().Equals("Top Cover")))
        {
            removeCover();

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
            barOwner.GetComponent<Scene1_BarOwner_WayPointControl>().closeMixer();
            topCover.transform.position = coverChild.transform.position;
            topCover.transform.rotation = coverChild.transform.rotation;

            if (!isDoneMixing)
            {
                if (!mixSound.isPlaying)
                {
                    mixSound.Play();
                }

                if (!mixParticle.isPlaying)
                {
                    mixParticle.Play();
                }

                if (mixerTimer >= 5.0f)
                {
                    finishMixing();
                }
                mixerTimer += Time.deltaTime;
                //print(mixerTimer);
            }
        }
        else
        {
            if (mixSound.isPlaying)
            {
                mixSound.Stop();
            }

            if (mixParticle.isPlaying)
            {
                mixParticle.Stop();
                print("STOP");
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

        if (Input.GetKeyDown(KeyCode.C))
        {
            finishMixing();
        }
    }
}