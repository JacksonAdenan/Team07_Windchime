using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;


public enum CatcherState
{
    EMPTY,
    EMPTY_CAPSULE,
    FILLED_1,
    FILLED_2,
    FILLED_3,
    FILLED_4,
    FULL_CAPSULE
}

[Serializable]
public class SoupCatcher
{

    // Soup catcher stats and current things. //
    public List<Soup> currentPortions;
    public CatcherState currentCatcherState = CatcherState.EMPTY;

    public Transform emptyAttachedCapsule;
    //public Transform filledAttachedCapsule;

    public bool hasCapsule = true;

    public SkinnedMeshRenderer skinnedMesh;

    // Start is called before the first frame update
    public void Start()
    {
        // Initialising catcher things. //
        currentPortions = new List<Soup>();
    }

    // Update is called once per frame
    public void Update()
    {
        // Catcher updates //
        UpdateCatcherState();
        UpdateCatcherCapsule();
    }

    void UpdateCatcherState()
    {
        if (!hasCapsule)
        {
            currentPortions.Clear();
            currentCatcherState = CatcherState.EMPTY;
            //skinnedMesh.material.color = Color.clear;
        }
        else if (currentPortions.Count == 0)
        {
            currentCatcherState = CatcherState.EMPTY_CAPSULE;
        }
        else if (currentPortions.Count == 1)
        {
            currentCatcherState = CatcherState.FILLED_1;
        }
        else if (currentPortions.Count == 2)
        {
            currentCatcherState = CatcherState.FILLED_2;
        }
        else if (currentPortions.Count == 3)
        {
            currentCatcherState = CatcherState.FILLED_3;
        }
        else if (currentPortions.Count == 4)
        {
            currentCatcherState = CatcherState.FILLED_4;
        }
        else if (currentPortions.Count == 5)
        {
            currentCatcherState = CatcherState.FULL_CAPSULE;
        }
    }

    void UpdateCatcherCapsule()
    {
        if (currentCatcherState == CatcherState.EMPTY)
        {
            emptyAttachedCapsule.gameObject.SetActive(false);
            //filledAttachedCapsule.gameObject.SetActive(false);

        }
        else if ((int)currentCatcherState == 1)
        {
            emptyAttachedCapsule.gameObject.SetActive(true);
            //filledAttachedCapsule.gameObject.SetActive(false);

            skinnedMesh.SetBlendShapeWeight(0, 0);
        }
        else if ((int)currentCatcherState == 2)
        {
            skinnedMesh.SetBlendShapeWeight(0, 20);

        }
        else if ((int)currentCatcherState == 3)
        {
            skinnedMesh.SetBlendShapeWeight(0, 40);
        }
        else if ((int)currentCatcherState == 4)
        {
            skinnedMesh.SetBlendShapeWeight(0, 60);
        }
        else if ((int)currentCatcherState == 5)
        {
            skinnedMesh.SetBlendShapeWeight(0, 80);
        }
        else if (currentCatcherState == CatcherState.FULL_CAPSULE)
        {
            //emptyAttachedCapsule.gameObject.SetActive(false);
            //filledAttachedCapsule.gameObject.SetActive(true);

            skinnedMesh.SetBlendShapeWeight(0, 100);
        }

    }

    public void CatchSoup(Transform soupToCatch)
    {


        currentPortions.Add(soupToCatch.GetComponent<SoupData>().theSoup);
        soupToCatch.gameObject.SetActive(false);
        Debug.Log("Caught a portion of soup.");


        // Setting the colour if its the first portion in the catcher. //
        if (currentPortions.Count == 1)
        { 
            Material newMaterial = skinnedMesh.material;

            newMaterial.color = Colour.ConvertColour(currentPortions[0].colour);
            skinnedMesh.material = newMaterial;
        }
    }

    public void AttachCapsule()
    {
        hasCapsule = true;
    }

    public void RemoveCapsule()
    {
        hasCapsule = false;
        Debug.Log("hasCapsule is now FALSE");
    }
}
