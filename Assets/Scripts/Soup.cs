using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Soup
{
    public string soupName;
    public float spicyValue;
    public float chunkyValue;
    public Ingridient restrictedIngredient;
    
    public Colour colour;



    // Remove these at some point. //
    public bool isSpicy;
    public bool isChunky;
    public Soup(string name, float spicyValue, float chunkyValue, Ingridient restrictedIngredient)
    {
        soupName = name;
        this.spicyValue = spicyValue;
        this.chunkyValue = chunkyValue;
        this.restrictedIngredient = restrictedIngredient;
    }
}
