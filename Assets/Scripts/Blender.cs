using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public enum Blend
{ 
    HALF,
    FULL
}
public enum BlenderState
{  
    JAR,
    NO_JAR,
    BLENDING,
}
public enum BlenderJointState
{
    OPEN,
    CLOSED
}

public enum BlenderButtonState
{ 
    OPEN,
    CLOSE,
    START_BLENDING,
    BLENDING,
    NOTHING

}
[Serializable]
public class Blender
{
    public Transform blender;

    // Triggers and stats for blender appliance. //
    public BlenderState currentBlenderState = BlenderState.JAR;
    public BlenderJointState currentBlenderJointState = BlenderJointState.OPEN;
    public BlenderButtonState currentBlenderButtonState = BlenderButtonState.OPEN;
    public List<Transform> currentBlenderIngredients;

    public Transform blenderEntryTrigger;
    public Transform blenderSpawnPoint;

    // This transform is just used to show if the blender is covered or not. //
    public Transform blenderCover;

    // Invulnerability timer for the blender. This is so if the blender "pops" off it doesn't instantly reattach istelf. //
    private float invulnerabilityTimer = 0;
    public bool isInvulnverable = true;

    // Blending progress. //
    public float blendCompletionTime = 0;
    public float continueButtonDuration = 0;
    public float completeButtonDuration = 0;


    // DO NOT SET THESE IN INSPECTOR, they are only public so that the horrible "menu manager" can access them! //
    [Header("Do not modify these.")]
    [Tooltip("This should be 0.")]
    [HideInInspector]
    public float continueButtonTimer = 0;
    [Tooltip("This should be 0.")]
    [HideInInspector]
    public float completeButtonTimer = 0;

    // blendProgress increases each time blendingDuration hits 1. blendingDuration increases by deltaTime every frame. //
    [HideInInspector]
    public float blendProgress = 0;
    [HideInInspector]
    private float blendingDuration = 0;

    [HideInInspector]
    public bool isHalfBlended = false;
    [HideInInspector    ]
    public bool isFullBlended = false;

    private bool isHalfFinished = false;
    private bool isFullFinished = false;

    [Header("Ingredient Shrinking/Movement")]
    public float ingredientShrinkTime = 0.01f;
    public float ingredientCenteringSpeed = 0.01f;
    public float ingredientShrinkSize = 0.38f;


    private List<Transform> shrinkingIngredients;

    public void BlenderStart()
    {
        currentBlenderIngredients = new List<Transform>();

        shrinkingIngredients = new List<Transform>();

    }
    public void BlenderUpdate()
    {
        UpdateBlenderCover();
        UpdateBlenderAnimation();
        UpdateBlenderButtonState();

        if (isInvulnverable == true)
        {
            Invulnerability();
        }

        // Do the blending thing. //
        if (currentBlenderState == BlenderState.BLENDING)
        {
            BlendProgress();
        }


        // Shrinking ingredients. //
        ShrinkIngredients();

    }


    public void ShrinkIngredients()
    {
        for (int i = shrinkingIngredients.Count - 1; i >= 0; i--)
        {

            shrinkingIngredients[i].localScale = Vector3.Lerp(shrinkingIngredients[i].localScale, (Vector3.one * ingredientShrinkSize), ingredientShrinkTime);

            shrinkingIngredients[i].position = Vector3.Lerp(shrinkingIngredients[i].position, blenderSpawnPoint.position, ingredientCenteringSpeed);

            if (Vector3.Distance(shrinkingIngredients[i].localScale, (Vector3.one * ingredientShrinkSize)) < 0.5f && Vector3.Distance(shrinkingIngredients[i].position, blenderSpawnPoint.position) < 0.05f)
            {
                shrinkingIngredients[i].tag = "Ingredient";
                shrinkingIngredients.Remove(shrinkingIngredients[i]);
            }
        }
    }
    public void Invulnerability()
    {
        invulnerabilityTimer += Time.deltaTime;
        if (invulnerabilityTimer >= 2)
        {
            invulnerabilityTimer = 0;
            isInvulnverable = false;
        }
    }

    void UpdateBlenderCover()
    {
        switch (currentBlenderState)
        {
            case BlenderState.JAR:
                blenderCover.gameObject.SetActive(true);
                break;
            case BlenderState.NO_JAR:
                blenderCover.gameObject.SetActive(false);
                break;
        }
    }

    public void UpdateBlenderButtonState()
    {
        if (currentBlenderState == BlenderState.JAR && currentBlenderJointState == BlenderJointState.CLOSED && currentBlenderIngredients.Count > 0)
        {
            currentBlenderButtonState = BlenderButtonState.START_BLENDING;
        }
        else if (currentBlenderState == BlenderState.JAR && currentBlenderJointState == BlenderJointState.OPEN)
        {
            currentBlenderButtonState = BlenderButtonState.CLOSE;
        }
        else if (currentBlenderState == BlenderState.JAR && currentBlenderJointState == BlenderJointState.CLOSED)
        {
            currentBlenderButtonState = BlenderButtonState.OPEN;
        }
        else if (currentBlenderState == BlenderState.NO_JAR)
        {
            currentBlenderButtonState = BlenderButtonState.NOTHING;
        }
        else if (currentBlenderState == BlenderState.BLENDING)
        {
            currentBlenderButtonState = BlenderButtonState.BLENDING;
        }
    }

    public void AddIngredientToBlender(Transform ingredientToCatch)
    {


        ingredientToCatch.GetComponent<Rigidbody>().velocity = Vector3.zero;

        // Making it's tag "shrinking"
        ingredientToCatch.tag = "Shrinking";
        
        // Adding it to the shrinking list.
        shrinkingIngredients.Add(ingredientToCatch);

        currentBlenderIngredients.Add(ingredientToCatch);
        Debug.Log("Ingredient added to blender.");

        //ingredientToCatch.position = blenderSpawnPoint.position;
        
        
        // Disabling the collider so other things don't bounce off of it. I'm doing this by making it a trigger. (I know it's bad.)
        ingredientToCatch.GetComponent<Collider>().isTrigger = true;
       
    }
    public void RemoveIngredientFromBlender(Transform ingredientToRemove)
    {
        currentBlenderIngredients.Remove(ingredientToRemove);
        Debug.Log("Ingredient removed from blender.");
    }

    public void AttachBlenderCover()
    {
        currentBlenderState = BlenderState.JAR;
    }
    public void RemoveBlenderCover()
    {
        currentBlenderState = BlenderState.NO_JAR;
    }
    
    public void PopBlenderCover()
    {
        // Must set blender to invulnerable so that the cover doesn't reattach straight away. //
        isInvulnverable = true;


        Transform poppedBlenderCover = GameObject.Instantiate(blenderCover, blenderCover.position, blenderCover.rotation);

        // Incase its invisible. //
        poppedBlenderCover.gameObject.SetActive(true);
        poppedBlenderCover.tag = "InteractableBlenderCover";

        // Not only do we set the parent prefab to have a capsule tag, but also the children it has. // 
        for (int i = 0; i < poppedBlenderCover.childCount; i++)
        {
            poppedBlenderCover.GetChild(0).tag = "InteractableBlenderCover";
        }

        // TEMPORARY FIX ME //
        // Because there is a mesh collider on the blender, we have to swap it out for a box collider if we want the cover to float around and behave with physics. //

        if (poppedBlenderCover.GetComponent<MeshCollider>())
        {
            GameObject.Destroy(poppedBlenderCover.GetComponent<MeshCollider>());
            poppedBlenderCover.gameObject.AddComponent<BoxCollider>();
        }

        // Adding upwards force to "pop" the cover //
        poppedBlenderCover.GetComponent<Rigidbody>().isKinematic = false;
        poppedBlenderCover.GetComponent<Rigidbody>().AddForce(Vector3.up * 100);

        RemoveBlenderCover();
    }

    public void BlenderButton()
    {
        switch (currentBlenderButtonState)
        {
            case BlenderButtonState.OPEN:
                currentBlenderJointState = BlenderJointState.OPEN;
                break;
            case BlenderButtonState.CLOSE:
                currentBlenderJointState = BlenderJointState.CLOSED;
                break;
            case BlenderButtonState.START_BLENDING:
                StartBlending();
                break;
            case BlenderButtonState.BLENDING:
                if (isHalfBlended)
                {
                    ContinueBlend();
                    
                }
                else if (isFullBlended)
                {
                    Blend(global::Blend.FULL);
                }
                else
                {
                    //Blend();
                }
                break;
            case BlenderButtonState.NOTHING:
                Debug.Log("Could not activate button. Please attach blender cover.");
                break;
        }
    }

    public void ContinueBlend()
    {
        isHalfBlended = false;
    }

    public void CompleteDuration()
    {
        completeButtonTimer += Time.deltaTime;
        if (completeButtonTimer >= completeButtonDuration)
        {
            isFullBlended = false;
            completeButtonTimer = 0;
            Blend(global::Blend.FULL);
            PopBlenderCover();
        }
    }
    public void ContinueDuration()
    {
        continueButtonTimer += Time.deltaTime;
        if (continueButtonTimer >= continueButtonDuration)
        {
            isHalfBlended = false;
            continueButtonTimer = 0;
            Blend(global::Blend.HALF);
            PopBlenderCover();
        }
    }
    private void ResetBlendProgress()
    {
        blendProgress = 0;
        isHalfBlended = false;
        isFullBlended = false;
        isHalfFinished = false;
        isFullFinished = false;

        continueButtonTimer = 0;
        completeButtonTimer = 0;
    }
    public void BlendProgress()
    {
        // Main tracking of blending progress. //
        blendingDuration += Time.deltaTime;
        if (blendingDuration >= 1)
        {
            blendProgress += 1;
            blendingDuration = 0;
        }

        // Timers for how long the player will be able to press the button to "continue" and "complete". //
        if (isHalfBlended)
        {
            ContinueDuration();
        }
        else if (isFullBlended)
        {
            CompleteDuration();
        }


        // Setting whether it's half blended or finished blending. //

        if ((blendProgress >= blendCompletionTime / 2) && !isHalfFinished)
        {
            isHalfBlended = true;
            isFullBlended = false;

            // This bool will make it so this only gets called the first time blend progress reaches half way. //
            isHalfFinished = true;
        }
        else if (blendProgress >= blendCompletionTime)
        {
            isFullBlended = true;
            isHalfBlended = false;
        }
    }

    public void StartBlending()
    {
        currentBlenderState = BlenderState.BLENDING;
    }
    public void Blend(Blend type)
    {

        Debug.Log("Blender activated");
        for (int i = currentBlenderIngredients.Count - 1; i > -1; i--)
        {
            // Spawn a blended thingy in its place. //
            SpawnBlendedIngredient(currentBlenderIngredients[i], type);
    
    
            currentBlenderIngredients[i].gameObject.SetActive(false);
            currentBlenderIngredients.Remove(currentBlenderIngredients[i]);
    
        }

        currentBlenderState = BlenderState.JAR;
        ResetBlendProgress();
    }
    public void SpawnBlendedIngredient(Transform oldIngredient, Blend type)
    {
        Ingredient dataToTransfer = oldIngredient.GetComponent<Ingredient>();
        //Transform newBlendedThing = UnityEngine.Object.Instantiate(oldIngredient.GetComponent<Ingredient>().blendedPrefab, blenderSpawnPoint.position, blenderSpawnPoint.rotation);
        Transform newBlendedThing = dataToTransfer.CreateBlended(blenderSpawnPoint.position, blenderSpawnPoint.rotation);
        // Incase the soupOrb we are copying isnt active and is missing important components. //
        newBlendedThing.gameObject.SetActive(true);

        if (newBlendedThing.GetComponent<Rigidbody>() == null)
        {
            newBlendedThing.gameObject.AddComponent<Rigidbody>();
        }
        if (newBlendedThing.GetComponent<Collider>() == null)
        {
            newBlendedThing.gameObject.AddComponent<SphereCollider>();
        }
        if (newBlendedThing.gameObject.tag != "Ingredient")
        {
            newBlendedThing.gameObject.tag = "Ingredient";
        }

        newBlendedThing.position = blenderSpawnPoint.position;

        // Just because we don't have any art for blended foods, ill make the soup orb a blended ingredient by changing the tag and adding an Ingredient script. //
        newBlendedThing.gameObject.AddComponent<Ingredient>();
        newBlendedThing.gameObject.GetComponent<Ingredient>().Copy(dataToTransfer);
        newBlendedThing.tag = "Ingredient";
        newBlendedThing.GetComponent<Rigidbody>().isKinematic = false;



        // Editing the cunkyness value. //
        if (type == global::Blend.HALF)
        {
            newBlendedThing.GetComponent<Ingredient>().chunkyness /= 2;
            Debug.Log("BLENDED HALF");
        }
        else if (type == global::Blend.FULL)
        {
            newBlendedThing.GetComponent<Ingredient>().chunkyness = 0;
            Debug.Log("BLENDED FULL");
        }

        // Setting it to BLENDED mode. //
        newBlendedThing.GetComponent<Ingredient>().currentState = IngredientState.BLENDED;

    }

    public void UpdateBlenderAnimation()
    {
        switch (currentBlenderJointState)
        {
            case BlenderJointState.CLOSED:
                blender.GetComponent<Animator>().SetBool("IsOpen", false);
                break;
            case BlenderJointState.OPEN:
                blender.GetComponent<Animator>().SetBool("IsOpen", true);
                break;
        }
    }

}

