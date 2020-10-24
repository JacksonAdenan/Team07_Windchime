using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Soup
{
    public float spicyValue;
    public float chunkyValue;
    //public Ingredient restrictedIngredient;
    


    public Colour colour;

    public List<Ingredient> usedIngredients;


    // Remove these at some point. //
    public bool isSpicy;
    public bool isChunky;
    public Soup(float spicyValue, float chunkyValue)
    {
        usedIngredients = new List<Ingredient>();


        this.spicyValue = spicyValue;
        this.chunkyValue = chunkyValue;
        
    }
}
