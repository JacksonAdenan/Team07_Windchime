using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System;

[Serializable]
public class Order
{
    public string orderName;
    public Colour colourPreference;
    public float spicyness;
    public float chunkiness;
    public bool noMeat;
    public bool noVeg;

    public float sweetness;

    [HideInInspector]
    public bool isReady = false;
    public Order(Colour colourPreference, float spicyValue, float chunkyValue, bool meatPreference, bool vegPreference, float sweetness)
    {
        this.orderName = "NOT ASSIGNED BY ORDERMANAGER";

        this.colourPreference = colourPreference;
        this.spicyness = spicyValue;
        this.chunkiness = chunkyValue;

        noMeat = meatPreference;
        noVeg = vegPreference;

        this.sweetness = sweetness;
    }
    public Order()
    { 
    }

    //static Soup GetSoupFromDropdown(int selected, TMP_Dropdown soupDropdown)
    //{
    //    for (int i = 0; i < CookingManager.allSoups.Count; i++)
    //    {
    //        if (soupDropdown.options[selected].text == CookingManager.allSoups[i].soupName)
    //        {
    //            return CookingManager.allSoups[i];
    //        }
    //    }
    //    return null;
    //}

    
}
