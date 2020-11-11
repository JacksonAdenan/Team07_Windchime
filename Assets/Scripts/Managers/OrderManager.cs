using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public enum OrderScreenState
{ 
    NEW_ORDER,
    CURRENT_ORDER,
    CURRENT_ORDER2
}
public class OrderManager : MonoBehaviour
{
    public List<Order> requestedOrders;
    public List<Order> acceptedOrders;

    // We have selectedOrder instead of just swapped around the positions of the orders in the array because we still want to keep track of which alien gets what food. //
    public Order selectedOrder;

    public static OrderScreenState currentScreenState;

    [Header("Order Mechanics")]
    public float nextOrderTimer = 0;
    public float newOrderRate = 15;
    private bool isOrderAvailable = false;


    // ---------------------- Colour Shenanigans ---------------------- //
    private List<Colour> availableColours;
    // ---------------------------------------------------------------- //

    void Start()
    {
        // Initialising all the lists. //
        requestedOrders = new List<Order>();
        acceptedOrders = new List<Order>();
        availableColours = new List<Colour>();

        currentScreenState = OrderScreenState.NEW_ORDER;

        // Initialising available colours //
        AddAllColours(availableColours);


        selectedOrder = null;
    }
    void Update()
    {
        UpdateOrders();
        UpdateSelectedOrder();


        if (isOrderAvailable == false)
        {
            OrderTimer();
        }
    }

    private void AddAllColours(List<Colour> list)
    {
        list.Add(Colour.blue);
        list.Add(Colour.red);
        list.Add(Colour.green);
    }

    public void UpdateSelectedOrder()
    {
        // If they only have one accepted order. //
        if (acceptedOrders.Count == 0)
        {
            selectedOrder = null;
            Debug.Log("selected order is null");
        }
        else if (acceptedOrders.Count == 1)
        {
            selectedOrder = acceptedOrders[0];
            Debug.Log("selected order is NOT null");
        }
    }
    public void SwapSelectedOrder()
    {
        // If they have 2 accepted orders. //
        if (acceptedOrders.Count > 1)
        {
            if (selectedOrder == acceptedOrders[0])
            {
                selectedOrder = acceptedOrders[1];
            }
            else
            {
                selectedOrder = acceptedOrders[0];
            }
        }    
    }
    private void OrderTimer()
    {
        nextOrderTimer += Time.deltaTime;
        if (nextOrderTimer >= newOrderRate)
        {
            nextOrderTimer = 0;
            isOrderAvailable = true;
        }
    }
    void UpdateOrders()
    {
        if (acceptedOrders.Count < 2 && requestedOrders.Count == 0 && isOrderAvailable)
        {
            SendOrder(CreateOrder());
        }
    }
    public void AcceptOrder(Order orderToAccept)
    {
        acceptedOrders.Add(orderToAccept);
        requestedOrders.Remove(requestedOrders[0]);
        currentScreenState = OrderScreenState.CURRENT_ORDER;

        isOrderAvailable = false;
    }
    public void RejectOrder()
    {
        requestedOrders.Remove(requestedOrders[0]);

        isOrderAvailable = false;
    }
    public void SendOrder(Order orderToAdd)
    {
        requestedOrders.Clear();
        requestedOrders.Add(orderToAdd);
    }

    public static Order ManuallyCreateOrder(TMP_Dropdown colourPreference, TMP_Dropdown meatVegPref, TMP_InputField spicy, TMP_InputField chunky)
    {
        Order newOrder = new Order();

        //newOrder.mainSoup = GetSoupFromDropdown(soup.value, soup);
        newOrder.colourPreference = Colour.blue;

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

    public Order CreateOrder()
    {
        // Making these ints here because I don't want the random function to return something lie 0.45345. //
        int desiredSpicyness;
        int desiredChunkyness;

        int randomNum;
        bool noMeat = false;
        bool noVeg = false;

        Colour desiredColour;
        int colourNum;

        // For now we don't have colours so just set it to nothing. //
        // Actually we do have colours now mwhahaha. just not very many. :( //
        colourNum = Random.Range(0, availableColours.Count);
        desiredColour = availableColours[colourNum];

        desiredSpicyness = Random.Range(1, 50);
        desiredChunkyness = Random.Range(1, 50);


        // Setting the vegetarian status. //
        randomNum = Random.Range(1, 4);
        if (randomNum == 1)
        {
            noMeat = true;
        }
        else if (randomNum == 2)
        {
            noVeg = true;
        }
        else
        {
            // Do nothing but just so its clearer ill write it again.
            noMeat = false;
            noVeg = false;
        }

        return new Order(desiredColour, desiredSpicyness, desiredChunkyness, noMeat, noVeg);
    }

    private int CompareOrder(Soup soupToSubmit)
    {
        int accumulatedScore = 0;

        int chunkynessPoints = 0;
        int spicynessPoints = 0;
        int colourPoints = 0;
        int meatVegPrefPoints = 0;



        int chunkynessDifference;
        int spicynessDifference;

        spicynessDifference = Mathf.Abs((int)soupToSubmit.spicyValue - (int)acceptedOrders[0].spicyness);
        chunkynessDifference = Mathf.Abs((int)soupToSubmit.chunkyValue - (int)acceptedOrders[0].chunkiness);

        chunkynessPoints -= chunkynessDifference;
        spicynessPoints -= spicynessDifference;


        // Since we don't have colours yet I'll just set it to 0.
        // we do have colours now.

        if (soupToSubmit.colour.name == acceptedOrders[0].colourPreference.name)
        {
            colourPoints = 10;
            Debug.Log("Got colour correct.");
        }
        else
        {
            Debug.Log("Got colour incorrect.");
            colourPoints = 0;
        }

        if (soupToSubmit.ContainsMeat() && acceptedOrders[0].noMeat)
        {
            meatVegPrefPoints -= 5;
        }
        if (soupToSubmit.ContainsVeg() && acceptedOrders[0].noVeg)
        {
            meatVegPrefPoints -= 5;
        }

        accumulatedScore += (chunkynessPoints + spicynessPoints + colourPoints + meatVegPrefPoints);
        
        return accumulatedScore;
    }

    public void CompleteOrder(Soup soupToSubmit)
    {
        ScoreManager.currentScore += CompareOrder(soupToSubmit);
        acceptedOrders.Remove(acceptedOrders[0]);
        currentScreenState = OrderScreenState.NEW_ORDER;

    }
}
