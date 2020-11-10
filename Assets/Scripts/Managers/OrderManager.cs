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

    public static OrderScreenState currentScreenState;

    [Header("Order Mechanics")]
    public float nextOrderTimer = 0;
    public float newOrderRate = 15;
    private bool isOrderAvailable = false;

    void Start()
    {
        // Initialising all the lists. //
        requestedOrders = new List<Order>();
        acceptedOrders = new List<Order>();

        currentScreenState = OrderScreenState.NEW_ORDER;
    }
    void Update()
    {
        UpdateOrders();

        if (isOrderAvailable == false)
        {
            OrderTimer();
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
            SendOrder(OrderManager.CreateOrder());
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

    public static Order CreateOrder()
    {
        // Making these ints here because I don't want the random function to return something lie 0.45345. //
        int desiredSpicyness;
        int desiredChunkyness;

        int randomNum;
        bool noMeat = false;
        bool noVeg = false;

        Colour desiredColour;

        // For now we don't have colours so just set it to nothing. //
        desiredColour = null;

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
        colourPoints = 0;

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
