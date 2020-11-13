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

    GameManager gameManager;

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


    // ---------------------- Alien Dudes ---------------------- //
    public Transform alien;


    public List<AlienAnimation> allAliens;

    public AlienAnimation alien1;
    public AlienAnimation alien2;
    public AlienAnimation alienStraggler;

    void Start()
    {
        gameManager = GameManager.GetInstance();

        allAliens = new List<AlienAnimation>();



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

        AlienMovementAnimation();


        for (int i = 0; i < allAliens.Count; i++)
        {
            if (allAliens[i].destroy)
            {
                allAliens[i].DeleteAlien();
            }
        }

        //if (alien1.destroy == true)
        //{
        //    alien1.DeleteAlien();
        //}
        //if (alien2.destroy == true)
        //{
        //    alien2.DeleteAlien();
        //}
    }
    void AlienMovementAnimation()
    {
        for (int i = 0; i < allAliens.Count; i++)
        {
            allAliens[i].Animate();
        }

        //if (alien2.currentState == AlienState.WAITING_2 && acceptedOrders.Count != 2)
        //{
        //    alien2.currentState = AlienState.WAITING;
        //}
        //
        //
        //if (alien1.alien != null)
        //{
        //    alien1.Animate();
        //}
        //if (alien2.alien != null)
        //{
        //    alien2.Animate();
        //}
    }

    private void AddAllColours(List<Colour> list)
    {
        list.Add(Colour.blue);
        list.Add(Colour.red);
        list.Add(Colour.green);
        list.Add(Colour.orange);
        list.Add(Colour.pink);
        list.Add(Colour.purple);
        list.Add(Colour.yellow);

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
        if (requestedOrders.Count == 1)
        {
            //alien1.GetComponent<Animator>().SetInteger("AlienPosition", 2);
            allAliens[0].currentState = AlienState.WAITING;
        }
        else if (requestedOrders.Count == 1 && acceptedOrders.Count == 1)
        {
            //alien2.GetComponent<Animator>().SetInteger("AlienPosition", 2);
            allAliens[1].currentState = AlienState.WAITING_2;
        }


        acceptedOrders.Add(orderToAccept);
        requestedOrders.Remove(requestedOrders[0]);
        currentScreenState = OrderScreenState.CURRENT_ORDER;

        isOrderAvailable = false;


    }
    public void RejectOrder()
    {
        if (requestedOrders.Count == 1 && acceptedOrders.Count == 1)
        {
            //alien1.GetComponent<Animator>().SetInteger("AlienPosition", 4);
            alien2.currentState = AlienState.LEAVING;
            alien2.destroy = true;
        }
        else if (requestedOrders.Count == 1)
        {
            //alien2.GetComponent<Animator>().SetInteger("AlienPosition", 4);
            alien1.currentState = AlienState.LEAVING;
            alien1.destroy = true;
        }


        requestedOrders.Remove(requestedOrders[0]);

        isOrderAvailable = false;

    }
    public void SendOrder(Order orderToAdd)
    {
        requestedOrders.Clear();
        requestedOrders.Add(orderToAdd);

        // Alien animation thingy //




        if (requestedOrders.Count == 1 && acceptedOrders.Count != 1)
        {

            Debug.Log("Alien created.");
            //alien1 = Instantiate(alien, alien.transform.parent);
            //alien1.GetComponent<Animator>().SetInteger("AlienPosition", 1);

            AlienAnimation newAlien = new AlienAnimation();
            newAlien.CreateAlien(alien);
            allAliens.Add(newAlien);
        }
        else if (requestedOrders.Count == 1 && acceptedOrders.Count == 1)
        {
            Debug.Log("Alien created.");

            AlienAnimation newAlien = new AlienAnimation();
            newAlien.CreateAlien(alien);
            allAliens.Add(newAlien);

            //alien2 = Instantiate(alien, alien.transform.parent);
            //alien2.GetComponent<Animator>().SetInteger("AlienPosition", 1);
            //alien2.CreateAlien(alien);
        }


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
        int desiredSweetness;


        int randomNum;
        bool noMeat = false;
        bool noVeg = false;

        Colour desiredColour;
        int colourNum;

        // For now we don't have colours so just set it to nothing. //
        // Actually we do have colours now mwhahaha. just not very many. :( //
        colourNum = Random.Range(0, availableColours.Count);
        desiredColour = availableColours[colourNum];

        desiredSpicyness = Random.Range(1, 6);
        desiredChunkyness = Random.Range(1, 6);
        desiredSweetness = Random.Range(1, 6);
        


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

        return new Order(desiredColour, desiredSpicyness, desiredChunkyness, noMeat, noVeg, desiredSweetness);
    }

    private bool CompareOrder(Soup soupToSubmit)
    {
        bool isCorrectOrder = true;

        int chunkynessDifference;
        int spicynessDifference;
        int sweetnessDifference;

        spicynessDifference = (int)soupToSubmit.spicyValue - (int)acceptedOrders[0].spicyness;
        chunkynessDifference = (int)soupToSubmit.chunkyValue - (int)acceptedOrders[0].chunkiness;
        sweetnessDifference = (int)soupToSubmit.sweetnessValue - (int)acceptedOrders[0].sweetness;

        // Spicyness and chunkyness check.
        if (spicynessDifference != 0)
        {
            isCorrectOrder = false;
        }
        if (chunkynessDifference != 0)
        {
            isCorrectOrder = false;
        }
        if (sweetnessDifference != 0)
        {
            isCorrectOrder = false;
        }

        // we do have colours now.
        if (soupToSubmit.colour.name != acceptedOrders[0].colourPreference.name)
        {
            isCorrectOrder = false;
            Debug.Log("Got colour incorrect.");
        }

        // Meat/Veg points.
        // -5 points if they give them something they didn't want. +5 if they got it right.
        if (soupToSubmit.ContainsMeat() && acceptedOrders[0].noMeat)
        {
            isCorrectOrder = false;
        }
        else if (soupToSubmit.ContainsVeg() && acceptedOrders[0].noVeg)
        {
            isCorrectOrder = false;
        }


        
        if (isCorrectOrder)
        {

            return true;
        }
        else
        {
            return false;
        }
        
    }

    public void CompleteOrder(Soup soupToSubmit)
    {

        allAliens[0].currentState = AlienState.LEAVING_HAPPILY;
        allAliens[0].destroy = true;
        //allAliens.Remove(allAliens[0]);


        if (CompareOrder(soupToSubmit) == true)
        {
            ScoreManager.currentScore += 500;
            gameManager.gameTime += 30;
            Debug.Log("Correct order");
        }
        else
        {
            Debug.Log("Incorrect order.");
        }

        acceptedOrders.Remove(acceptedOrders[0]);
        currentScreenState = OrderScreenState.NEW_ORDER;

    }
}
