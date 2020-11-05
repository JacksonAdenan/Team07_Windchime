using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public enum BlenderCoverState
{  
    JAR,
    NO_JAR,
    LOADED,
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
    BLEND,
    NOTHING

}
[Serializable]
public class Blender
{
    public Transform blender;

    // Triggers and stats for blender appliance. //
    public BlenderCoverState currentBlenderState = BlenderCoverState.JAR;
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

    public void BlenderStart()
    {
        currentBlenderIngredients = new List<Transform>();

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
            case BlenderCoverState.JAR:
                blenderCover.gameObject.SetActive(true);
                break;
            case BlenderCoverState.NO_JAR:
                blenderCover.gameObject.SetActive(false);
                break;
        }
    }

    public void UpdateBlenderButtonState()
    {
        if (currentBlenderState == BlenderCoverState.JAR && currentBlenderJointState == BlenderJointState.CLOSED && currentBlenderIngredients.Count > 0)
        {
            currentBlenderButtonState = BlenderButtonState.BLEND;
        }
        else if (currentBlenderState == BlenderCoverState.JAR && currentBlenderJointState == BlenderJointState.OPEN)
        {
            currentBlenderButtonState = BlenderButtonState.CLOSE;
        }
        else if (currentBlenderState == BlenderCoverState.JAR && currentBlenderJointState == BlenderJointState.CLOSED)
        {
            currentBlenderButtonState = BlenderButtonState.OPEN;
        }
        else if (currentBlenderState == BlenderCoverState.NO_JAR)
        {
            currentBlenderButtonState = BlenderButtonState.NOTHING;
        }
    }

    public void AddIngredientToBlender(Transform ingredientToCatch)
    {
        currentBlenderIngredients.Add(ingredientToCatch);
        Debug.Log("Ingredient added to blender.");
    }
    public void RemoveIngredientFromBlender(Transform ingredientToRemove)
    {
        currentBlenderIngredients.Remove(ingredientToRemove);
        Debug.Log("Ingredient removed from blender.");
    }

    public void AttachBlenderCover()
    {
        currentBlenderState = BlenderCoverState.JAR;
    }
    public void RemoveBlenderCover()
    {
        currentBlenderState = BlenderCoverState.NO_JAR;
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
            case BlenderButtonState.BLEND:
                Blend();
                break;
            case BlenderButtonState.NOTHING:
                Debug.Log("Could not activate button. Please attach blender cover.");
                break;
        }

        //if (currentBlenderJointState == BlenderJointState.CLOSED)
        //{
        //    currentBlenderJointState = BlenderJointState.OPEN;
        //}
        //else if (currentBlenderJointState == BlenderJointState.OPEN)
        //{
        //    currentBlenderJointState = BlenderJointState.CLOSED;
        //}

        
        //else
        //{
        //    Debug.Log("Blender could not be activated. Please put the cover on.");
        //}
    }

    public void Blend()
    {

        Debug.Log("Blender activated");
        for (int i = currentBlenderIngredients.Count - 1; i > -1; i--)
        {
            // Spawn a blended thingy in its place. //
            SpawnBlendedIngredient(currentBlenderIngredients[i]);
    
    
            currentBlenderIngredients[i].gameObject.SetActive(false);
            currentBlenderIngredients.Remove(currentBlenderIngredients[i]);
    
        }
    
        PopBlenderCover();
        RemoveBlenderCover();
    
        
    }
    public void SpawnBlendedIngredient(Transform oldIngredient)
    {
        Ingredient dataToTransfer = oldIngredient.GetComponent<Ingredient>();
        Transform newBlendedThing = UnityEngine.Object.Instantiate(oldIngredient.GetComponent<Ingredient>().blendedPrefab, blenderSpawnPoint.position, blenderSpawnPoint.rotation);

        // Incase the soupOrb we are copying isnt active. //
        newBlendedThing.gameObject.SetActive(true);

        newBlendedThing.position = blenderSpawnPoint.position;

        // Just because we don't have any art for blended foods, ill make the soup orb a blended ingredient by changing the tag and adding an Ingredient script. //
        newBlendedThing.gameObject.AddComponent<Ingredient>();
        newBlendedThing.gameObject.GetComponent<Ingredient>().Copy(dataToTransfer);
        newBlendedThing.tag = "Ingredient";
        newBlendedThing.GetComponent<Rigidbody>().isKinematic = false;

        // Because its instantiating the soup thing we have to remove its soup data script. //
        //Destroy(newBlendedThing.GetComponent<SoupData>());

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

