﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;


public enum IngredientState
{ 
    WHOLE,
    HALF,
    QUARTER,
    BLENDED
}

//[Serializable]
public class Ingredient : MonoBehaviour
{
    GameManager gameManager;


    public string ingredientName;
    public float spicyness;
    public float chunkyness;
    public float sweetness;

    public bool isMeat;

    public Colour_Tag colourTag;
    [Tooltip("Don't set this colour value. It's automatically set by the colourTag.")]
    public Colour colour;

    public IngredientState currentState;

    public Transform prefab;
    public Transform halfedPrefab;
    public Transform quateredPrefab;
    public Transform blendedPrefab;

    void Start()
    {

        gameManager = GameManager.GetInstance();


        Debug.Log("----------------------------- HEYYYYY -------------");
        colour = Colour.ConvertColour(colourTag);



        if (blendedPrefab == null)
        {
            blendedPrefab = Instantiate(gameManager.cookingManager.blendedIngredientPrefab, Vector3.zero, Quaternion.identity);

            // Setting the colour of the occupying soup. //
            Material newMaterial = new Material(gameManager.cookingManager.theOrb.waterShader);
            newMaterial.SetColor("Color_6EDA1D08", Colour.ConvertColour(colour));
            blendedPrefab.GetComponent<Renderer>().material = newMaterial;
        }


    }
    public Ingredient(string name, float spicy, float chunky, bool isMeat)
    {
        prefab = null;
        currentState = IngredientState.WHOLE;
        this.ingredientName = name;
        spicyness = spicy;
        chunkyness = chunky;
        this.isMeat = isMeat;
        //colour = null;
    }

    public void Copy(Ingredient thingToCopy)
    {
        this.ingredientName = thingToCopy.ingredientName;
        this.spicyness = thingToCopy.spicyness;
        this.chunkyness = thingToCopy.chunkyness;
        this.isMeat = thingToCopy.isMeat;
        this.currentState = thingToCopy.currentState;
        this.prefab = thingToCopy.prefab;
        this.halfedPrefab = thingToCopy.halfedPrefab;
        this.quateredPrefab = thingToCopy.quateredPrefab;
        this.blendedPrefab = thingToCopy.blendedPrefab;

        this.colour = thingToCopy.colour;
        this.sweetness = thingToCopy.sweetness;
    }

    public static void CreateIngredient(Transform originalIngredient, Transform newIngredient, IngredientState state)
    {
        switch (state)
        {
            case IngredientState.WHOLE:
                break;
            case IngredientState.HALF:
                // Just because we don't have any art for blended foods, ill make the soup orb a blended ingredient by changing the tag and adding an Ingredient script. //
                if (!newIngredient.GetComponent<Ingredient>())
                { 
                    newIngredient.gameObject.AddComponent<Ingredient>();
                }

                newIngredient.gameObject.GetComponent<Ingredient>().Copy(originalIngredient.GetComponent<Ingredient>());
                newIngredient.tag = "Ingredient";
                newIngredient.GetComponent<Rigidbody>().isKinematic = false;



                // Editing the cunkyness value. //
                newIngredient.GetComponent<Ingredient>().chunkyness /= 2;
                newIngredient.GetComponent<Ingredient>().currentState = IngredientState.HALF;
                Debug.Log("SLICED HALF");
                break;
            case IngredientState.QUARTER:
                if (!newIngredient.GetComponent<Ingredient>())
                {
                    newIngredient.gameObject.AddComponent<Ingredient>();
                }

                newIngredient.gameObject.GetComponent<Ingredient>().Copy(originalIngredient.GetComponent<Ingredient>());
                newIngredient.tag = "Ingredient";
                newIngredient.GetComponent<Rigidbody>().isKinematic = false;



                // Editing the cunkyness value. //
                newIngredient.GetComponent<Ingredient>().chunkyness /= 4;
                newIngredient.GetComponent<Ingredient>().currentState = IngredientState.QUARTER;
                Debug.Log("SLICED QUARTER");
                break;

        }
    }

}
