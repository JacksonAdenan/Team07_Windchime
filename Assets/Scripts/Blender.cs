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
[Serializable]
public class Blender
{
    public Transform blender;

    // Triggers and stats for blender appliance. //
    public BlenderCoverState currentBlenderState = BlenderCoverState.JAR;
    public BlenderJointState currentBlenderJointState = BlenderJointState.OPEN;
    public List<Transform> currentBlenderIngredients;

    public Transform blenderEntryTrigger;
    public Transform blenderSpawnPoint;

    // This transform is just used to show if the blender is covered or not. //
    public Transform blenderCover;

    public void BlenderStart()
    {
        currentBlenderIngredients = new List<Transform>();

    }
    public void BlenderUpdate()
    {
        UpdateBlenderCover();
        UpdateBlenderAnimation();
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
        Transform poppedBlenderCover = UnityEngine.Object.Instantiate(blenderCover, blenderCover.position, blenderCover.rotation);
        poppedBlenderCover.tag = "InteractableBlenderCover";

        // Not only do we set the parent prefab to have a capsule tag, but also the children it has. // 
        for (int i = 0; i < poppedBlenderCover.childCount; i++)
        {
            poppedBlenderCover.GetChild(0).tag = "InteractableBlenderCover";
        }



        // TEMPORARY FIX ME //
        // Because the mesh on the blender isn't working properly, we can't put a mesh collider on it. This means we have to use a box collider and set it to be a trigger so that ingredients can
        // be inside of it.

        poppedBlenderCover.GetComponent<BoxCollider>().isTrigger = false;

        // Adding upwards force to "pop" the cover //
        poppedBlenderCover.GetComponent<Rigidbody>().isKinematic = false;
        poppedBlenderCover.GetComponent<Rigidbody>().AddForce(Vector3.up * 50);
    }

    public void BlenderButton()
    {
        if (currentBlenderJointState == BlenderJointState.CLOSED)
        {
            currentBlenderJointState = BlenderJointState.OPEN;
        }
        else if (currentBlenderJointState == BlenderJointState.OPEN)
        {
            currentBlenderJointState = BlenderJointState.CLOSED;
        }

        //if (currentBlenderState == BlenderState.COVERED)
        //{
        //    Debug.Log("Blender activated");
        //    for (int i = currentBlenderIngredients.Count - 1; i > -1; i--)
        //    {
        //        // Spawn a blended thingy in its place. //
        //        SpawnBlendedIngredient(currentBlenderIngredients[i]);
        //
        //
        //        currentBlenderIngredients[i].gameObject.SetActive(false);
        //        currentBlenderIngredients.Remove(currentBlenderIngredients[i]);
        //
        //    }
        //
        //
        //    RemoveBlenderCover();
        //    PopBlenderCover();
        //}
        //else
        //{
        //    Debug.Log("Blender could not be activated. Please put the cover on.");
        //}
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

