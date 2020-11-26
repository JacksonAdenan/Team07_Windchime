﻿using System.Collections;
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
    SoundManager soundManager;

    public List<Order> requestedOrders;
    public List<Order> acceptedOrders;

    // We have selectedOrder instead of just swapped around the positions of the orders in the array because we still want to keep track of which alien gets what food. //
    public int selectedOrder = -1;

    public static OrderScreenState currentScreenState;

    [Header("Order Mechanics")]
    [HideInInspector]
    public float nextOrderTimer = 0;
    public float newOrderRate = 15;
    private bool isOrderAvailable = false;


    public int pointsForSuccessfulOrder = 500;
    public float timeForSuccessfulOrder = 60;

    public int perfectRequirementPoints = 100;
    public float perfectRequirementTime = 10;

    public int acaceptableRequirementPoints = 25;
    public float acceptableRequirementTime = 0;

    [Header("Randomisation Number Things")]
    public int spicyMin = 0;
    [Tooltip("Max is exclusive")]
    public int spicyMax = 6;

    public int sweetnessMin = 0;
    [Tooltip("Max is exclusive")]
    public int sweetnessMax = 6;

    public int chunkynessMin = 0;
    [Tooltip("Max is exclusive")]
    public int chunkynessMax = 6;





    // ---------------------- Colour Shenanigans ---------------------- //
    private List<Colour> availableColours;
    // ---------------------------------------------------------------- //


    // ---------------------- Alien Dudes ---------------------- //
    public Transform alien;


    public List<AlienAnimation> activeAliens;
    public List<AlienAnimation> destroyingAliens;

    //public AlienAnimation alien1;
    //public AlienAnimation alien2;
    //public AlienAnimation alienStraggler;

    void Start()
    {
        gameManager = GameManager.GetInstance();
        soundManager = gameManager.soundManager;

        activeAliens = new List<AlienAnimation>();
        destroyingAliens = new List<AlienAnimation>();


        // Initialising all the lists. //
        requestedOrders = new List<Order>();
        acceptedOrders = new List<Order>();
        availableColours = new List<Colour>();

        currentScreenState = OrderScreenState.NEW_ORDER;

        // Initialising available colours //
        AddAllColours(availableColours);


        selectedOrder = -1;

     
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


        for (int i = 0; i < destroyingAliens.Count; i++)
        {
            if (destroyingAliens[i].destroy)
            {
                destroyingAliens[i].DeleteAlien();
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
        //if (activeAliens.Count > 1 && activeAliens[1].currentState == AlienState.WAITING_2 && acceptedOrders.Count != 2)
        //{
        //    activeAliens[0].currentState = AlienState.WAITING;
        //}

        for (int i = 0; i < activeAliens.Count; i++)
        {
            activeAliens[i].Animate();
            
        }

        for (int i = 0; i < destroyingAliens.Count; i++)
        {
            destroyingAliens[i].Animate();
        }

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
        list.Add(gameManager.colourManager.blue);
        list.Add(gameManager.colourManager.red);
        list.Add(gameManager.colourManager.lightGreen);
        list.Add(gameManager.colourManager.darkGreen);
        list.Add(gameManager.colourManager.orange);
        list.Add(gameManager.colourManager.pink);
        list.Add(gameManager.colourManager.orchid);
        list.Add(gameManager.colourManager.yellow);
        list.Add(gameManager.colourManager.salmon);
        list.Add(gameManager.colourManager.violet);
        list.Add(gameManager.colourManager.aqua);
        list.Add(gameManager.colourManager.darkRed);

    }

    public void UpdateSelectedOrder()
    {
        // If they only have one accepted order. //
        if (acceptedOrders.Count == 0)
        {
            if (selectedOrder != -1)
            { 
                selectedOrder = -1;
            }

        }
        else if (acceptedOrders.Count > 0 && selectedOrder == -1)
        {
            selectedOrder = 0;
        }
    }
    public void SwapSelectedOrder()
    {
        // If they have 2 accepted orders. //
        if (acceptedOrders.Count > 1)
        {
            if (selectedOrder == 0)
            {
                Debug.Log("ASSIGNED SELECTED ORDER");
                selectedOrder = 1;
            }
            else
            {
                Debug.Log("ASSIGNED SELECTED ORDER");
                selectedOrder = 0;
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


        if (activeAliens.Count > 0 && acceptedOrders.Count == 0 && requestedOrders.Count == 1)
        {
            if (activeAliens[0].isOrderReady)
            {
                requestedOrders[0].isReady = true;
            }
        }
        else if (activeAliens.Count > 1 && acceptedOrders.Count == 1 && requestedOrders.Count == 1)
        {
            if (activeAliens[1].isOrderReady)
            {
                requestedOrders[0].isReady = true;
            }
        }
    }
    public void AcceptOrder(Order orderToAccept)
    {
        if (requestedOrders.Count == 1 && acceptedOrders.Count == 0)
        {

            activeAliens[0].currentState = AlienState.WAITING;
        }
        else if (requestedOrders.Count == 1 && acceptedOrders.Count == 1)
        {
            activeAliens[1].currentState = AlienState.WAITING_2;
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
            activeAliens[1].currentState = AlienState.LEAVING;


            // Can't really be bothered thinking about the details of where what is in terms of position for the list... but I know that setting the thing to destroy while its still in active list makes it
            // so we don't have to find it once it's in the destroyingAliens list.
            activeAliens[1].destroy = true;

            destroyingAliens.Add(activeAliens[1]);


            activeAliens.Remove(activeAliens[1]);
        }
        else if (requestedOrders.Count == 1 && acceptedOrders.Count == 0)
        {
            //alien2.GetComponent<Animator>().SetInteger("AlienPosition", 4);
            activeAliens[0].currentState = AlienState.LEAVING;

            destroyingAliens.Add(activeAliens[0]);
            destroyingAliens[0].destroy = true;

            activeAliens.Remove(activeAliens[0]);
        }


        requestedOrders.Remove(requestedOrders[0]);

        isOrderAvailable = false;

    }

    public void OrderAppear()
    {
        
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
            activeAliens.Add(newAlien);
        }
        else if (requestedOrders.Count == 1 && acceptedOrders.Count == 1)
        {
            Debug.Log("Alien created.");

            AlienAnimation newAlien = new AlienAnimation();
            newAlien.CreateAlien(alien);
            activeAliens.Add(newAlien);

            //alien2 = Instantiate(alien, alien.transform.parent);
            //alien2.GetComponent<Animator>().SetInteger("AlienPosition", 1);
            //alien2.CreateAlien(alien);
        }
    }

    public Order ManuallyCreateOrder(TMP_Dropdown colourPreference, TMP_Dropdown meatVegPref, TMP_InputField spicy, TMP_InputField chunky)
    {
        Order newOrder = new Order();

        //newOrder.mainSoup = GetSoupFromDropdown(soup.value, soup);
        newOrder.colourPreference = gameManager.colourManager.blue;

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

        desiredSpicyness = Random.Range(spicyMin, spicyMax);
        desiredChunkyness = Random.Range(chunkynessMin, chunkynessMax);
        desiredSweetness = Random.Range(sweetnessMin, sweetnessMax);
        


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
        bool isSuccessfulOrder = true;

        int chunkynessDifference;
        int spicynessDifference;
        int sweetnessDifference;

        spicynessDifference = (int)soupToSubmit.spicyValue - (int)acceptedOrders[0].spicyness;
        chunkynessDifference = (int)soupToSubmit.chunkyValue - (int)acceptedOrders[0].chunkiness;
        sweetnessDifference = (int)soupToSubmit.sweetnessValue - (int)acceptedOrders[0].sweetness;


        // Early out if the player submitted empty capsule. //
        if (soupToSubmit.usedIngredients.Count == 0)
        {
            isSuccessfulOrder = false;
            return false;
        }



        // --------------------------------- Spicyness --------------------------------- //
        if (spicynessDifference == 0)
        {
            RewardPoints(perfectRequirementPoints);
            RewardTime(perfectRequirementTime);

            Debug.Log("Perfect spicyness acheived.");
        }
        else if (spicynessDifference > CalculateLowerHalf((float)acceptedOrders[0].spicyness) && spicynessDifference < CalculateUpperHalf((int)acceptedOrders[0].spicyness))
        {
            RewardPoints(acaceptableRequirementPoints);
            RewardTime(acceptableRequirementTime);

            Debug.Log("Acceptable spicyness acheived.");
        }
        else
        {
            Debug.Log("Failed spicyness");
            isSuccessfulOrder = false;
        }
        // ----------------------------------------------------------------------------- //

        // --------------------------------- Chunkyness --------------------------------- //
        if (chunkynessDifference == 0)
        {
            RewardPoints(perfectRequirementPoints);
            RewardTime(perfectRequirementTime);

            Debug.Log("Perfect chunkyness acheived.");
        }
        else if (chunkynessDifference > CalculateLowerHalf((float)acceptedOrders[0].chunkiness) && chunkynessDifference < CalculateUpperHalf((int)acceptedOrders[0].chunkiness))
        {
            RewardPoints(acaceptableRequirementPoints);
            RewardTime(acceptableRequirementTime);

            Debug.Log("Acceptable chunkyness acheived.");
        }
        else
        {
            Debug.Log("Failed chunkyness");
            isSuccessfulOrder = false;
        }
        // -------------------------------------------------------------------------------- //

        // --------------------------------- Sweetness --------------------------------- //
        if (sweetnessDifference == 0)
        {
            RewardPoints(perfectRequirementPoints);
            RewardTime(perfectRequirementTime);

            Debug.Log("Perfect sweetness acheived.");
        }
        else if (sweetnessDifference > CalculateLowerHalf((float)acceptedOrders[0].sweetness) && sweetnessDifference < CalculateUpperHalf((int)acceptedOrders[0].sweetness))
        {
            RewardPoints(acaceptableRequirementPoints);
            RewardTime(acceptableRequirementTime);

            Debug.Log("Acceptable sweetness acheived.");
        }
        else
        {
            Debug.Log("Failed sweetness");
            isSuccessfulOrder = false;
        }
        // ------------------------------------------------------------------------------ //

        // --------------------------------- Colours --------------------------------- //
        if (soupToSubmit.colour.name == acceptedOrders[0].colourPreference.name)
        {
            RewardPoints(perfectRequirementPoints);
            RewardTime(perfectRequirementTime);
            Debug.Log("Got colour correct.");
        }
        else
        {
            isSuccessfulOrder = false;
            Debug.Log("Got colour incorrect.");
        }
        // --------------------------------------------------------------------------- //


        // --------------------------------- Food Requirement --------------------------------- //
        if (soupToSubmit.ContainsMeat() && acceptedOrders[0].noMeat)
        {
            isSuccessfulOrder = false;
        }
        else if (soupToSubmit.ContainsVeg() && acceptedOrders[0].noVeg)
        {
            isSuccessfulOrder = false;
        }
        else
        {
            // This if statement makes sure the player doesn't just submit empty capsules. //
            if (soupToSubmit.usedIngredients.Count > 0)
            { 
                RewardPoints(perfectRequirementPoints);
                RewardTime(perfectRequirementTime);

                Debug.Log("Got food specification requirement correct.");
            }
        }
        // ------------------------------------------------------------------------------------- //


        
        // Returning true or false depending if the order was successful. //
        if (isSuccessfulOrder)
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

        activeAliens[0].currentState = AlienState.LEAVING_HAPPILY;

        Debug.Log("ACCEPTED ORDERS COUNT: " + acceptedOrders.Count + "========================");
        Debug.Log("ACTIVE ALIENS ORDERS COUNT: " + activeAliens.Count + "========================");


        if (activeAliens.Count > 1 && activeAliens[1].currentState == AlienState.WAITING_2 && acceptedOrders.Count == 2)
        {
            Debug.Log("AHHHHHHHHHHHL OOK OVA HERE!");
            activeAliens[1].currentState = AlienState.WAITING;
        }

        destroyingAliens.Add(activeAliens[0]);
        destroyingAliens[0].destroy = true;

        activeAliens.Remove(activeAliens[0]);



        if (CompareOrder(soupToSubmit) == true)
        {
            RewardPoints(pointsForSuccessfulOrder);
            RewardTime(timeForSuccessfulOrder);
            Debug.Log("Correct order");
        }
        else
        {
            Debug.Log("Incorrect order.");
        }


        // Clearing order from list. //
        selectedOrder = -1;
        acceptedOrders.Remove(acceptedOrders[0]);
        currentScreenState = OrderScreenState.NEW_ORDER;

    }

    private void RewardPoints(int points)
    {
        ScoreManager.currentScore += points;
        Debug.Log("Rewared " + points + " to the player.");
    }
    private void RewardTime(float time)
    {
        gameManager.gameTime += time;
        Debug.Log("Rewarded " + time + " seconds to the player.");
    }

    public static float CalculateLowerHalf(float number)
    {
        float half = number / 2;
        return number - half;
    }
    public static float CalculateUpperHalf(float number)
    {
        float half = number / 2;
        return number + half;
    }
}
