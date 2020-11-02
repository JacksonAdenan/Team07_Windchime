using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;


public enum IngredientState
{ 
    WHOLE,
    HALF,
    QUARTER
}

//[Serializable]
public class Ingredient : MonoBehaviour
{
    public string ingredientName;
    public float spicyness;
    public float chunkyness;

    public bool isMeat;
    //Colour colour;

    public IngredientState currentState;

    public Transform prefab;
    public Transform halfedPrefab;
    public Transform quateredPrefab;
    public Transform blendedPrefab;

    void Start()
    { 
        
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
    }

}
