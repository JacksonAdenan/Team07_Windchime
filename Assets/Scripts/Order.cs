using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
public class Order
{
    public string orderName;
    public Colour colourPreference;
    public float spicyness;
    public float chunkiness;
    public bool noMeat;
    public bool noVeg;
    public Order(Colour colourPreference, float spicyValue, float chunkyValue, bool meatPreference, bool vegPreference)
    {
        this.orderName = "NOT ASSIGNED BY ORDERMANAGER";

        this.colourPreference = colourPreference;
        this.spicyness = spicyValue;
        this.chunkiness = chunkyValue;
    }
    public Order()
    { 
    }

    static Soup GetSoupFromDropdown(int selected, TMP_Dropdown soupDropdown)
    {
        for (int i = 0; i < CookingManager.allSoups.Count; i++)
        {
            if (soupDropdown.options[selected].text == CookingManager.allSoups[i].soupName)
            {
                return CookingManager.allSoups[i];
            }
        }
        return null;
    }

    
}
