using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class OrderManager : MonoBehaviour
{
    public static List<Order> currentOrders;

    void Start()
    {
        currentOrders = new List<Order>();
    }

    public static void AddOrder(Order orderToAdd)
    {
        currentOrders.Clear();
        currentOrders.Add(orderToAdd);
    }

    public static Order CreateOrder(TMP_Dropdown colourPreference, TMP_Dropdown meatVegPref, TMP_InputField spicy, TMP_InputField chunky)
    {
        Order newOrder = new Order();

        //newOrder.mainSoup = GetSoupFromDropdown(soup.value, soup);
        newOrder.colourPreference = new Colour("none");

        try
        {
            newOrder.spicyness = float.Parse(spicy.text);
        }
        catch
        {
            newOrder.spicyness = 0;
            Debug.Log("Entered spicyness value was not a number.");
        }
        try
        {
            newOrder.chunkiness = float.Parse(chunky.text);
        }
        catch
        {
            newOrder.chunkiness = 0;
            Debug.Log("Entered chunkiness value was not a number.");
        }
        

        if (meatVegPref.value == 0)
        {
            newOrder.noMeat = false;
            newOrder.noVeg = false;
        }
        else if (meatVegPref.value == 1)
        {
            newOrder.noMeat = true;
            newOrder.noVeg = false;
        }
        else if (meatVegPref.value == 2)
        {
            newOrder.noVeg = true;
            newOrder.noMeat = false;
        }

        return newOrder;
    }
}
