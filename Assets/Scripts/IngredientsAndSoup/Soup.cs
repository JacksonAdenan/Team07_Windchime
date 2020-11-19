using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public class Soup
{
    public float spicyValue;
    public float chunkyValue;
    public float sweetnessValue;
    //public Ingredient restrictedIngredient;
    


    public Colour colour;

    public List<Ingredient> usedIngredients;


    // Remove these at some point. //
    public bool isSpicy;
    public bool isChunky;

    void Start()
    {
        usedIngredients = new List<Ingredient>();
    }
    //public void SetSoup(float spicyValue, float chunkyValue)
    //{
    //    this.spicyValue = spicyValue;
    //    this.chunkyValue = chunkyValue;
    //}
    public Soup(float spicyValue, float chunkyValue, float sweetnessValue)
    {
        usedIngredients = new List<Ingredient>();
    
    
        this.spicyValue = spicyValue;
        this.chunkyValue = chunkyValue;
        this.sweetnessValue = sweetnessValue;
        
    }

    public bool ContainsMeat()
    {
        for (int i = 0; i < usedIngredients.Count; i++)
        {
            if (usedIngredients[i].isMeat == true)
            {
                return true;
            }
        }

        return false;
    }

    public bool ContainsVeg()
    {
        for (int i = 0; i < usedIngredients.Count; i++)
        {
            if (usedIngredients[i].isMeat == false)
            {
                return true;
            }
        }

        return false;
    }
}
