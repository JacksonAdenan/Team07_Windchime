using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;


public enum MonitorType
{ 
    ITEM_FABRICATOR_MONITOR,
    NEW_ORDER_MONITOR,
    CURRENT_ORDER_MONITOR
}
public enum ScreenState
{ 
    MAIN_MENU,
    SECONDARY
}
public class MonitorScreen : MonoBehaviour
{

    GameManager gameManager;
    OrderManager orderManager;

    public Transform mainScreen;
    public Transform secondaryScreen;

    public ScreenState currentState = ScreenState.MAIN_MENU;
    public MonitorType thisMonitor;

    // Because our item fabricator needs to display ingredient stats, we need a reference to the ingredient. //
    public Ingredient currentIngredientDisplay;

    // Text/dropdownboxes and other UI elements go here. //
    public TextMeshProUGUI title;
    public TextMeshProUGUI ingredientNameText;
    public TextMeshProUGUI spicyText;
    public TextMeshProUGUI chunkyText;
    public TextMeshProUGUI meatText;
    public TextMeshProUGUI colourText;

    public TextMeshProUGUI sweetnessText;

    public SpriteRenderer foodIcon;

    public 


    // Start is called before the first frame update
    void Start()
    {
        gameManager = GameManager.GetInstance();
        orderManager = gameManager.orderManager;
    }

    // Update is called once per frame
    void Update()
    {
        DisplayCurrentScreen();
    }


    public void DisplayCurrentScreen()
    {
        switch (currentState)
        {
            case ScreenState.MAIN_MENU:
                if (thisMonitor == MonitorType.NEW_ORDER_MONITOR)
                {
                    if (orderManager.requestedOrders.Count > 0)
                    {
                        DisplayMainMenu(orderManager.requestedOrders[0]);
                    }
                    else
                    {
                        mainScreen.gameObject.SetActive(false);
                    }
                }
                else if (thisMonitor == MonitorType.CURRENT_ORDER_MONITOR)
                {
                    if (orderManager.selectedOrder != null)
                    {
                        DisplayMainMenu(orderManager.selectedOrder);
                    }
                    else
                    {
                        mainScreen.gameObject.SetActive(false);
                    }
                }
                else
                {
                    DisplayMainMenu();
                }
                break;
            case ScreenState.SECONDARY:
                if (currentIngredientDisplay != null)
                {
                    DisplaySecondaryMenu(currentIngredientDisplay);
                }
                else
                { 
                    DisplaySecondaryMenu();
                }
                break;
        }
    }

    public void DisplayMainMenu()
    {
        mainScreen.gameObject.SetActive(true);
        secondaryScreen.gameObject.SetActive(false);

        // If we had stored a ingredient we are going to null it now and all the text. //
        currentIngredientDisplay = null;
    }

    public void DisplayMainMenu(Order orderToDisplay)
    {
        if (!mainScreen.gameObject.activeSelf)
        { 
            mainScreen.gameObject.SetActive(true);
        }
        //secondaryScreen.gameObject.SetActive(false);

        spicyText.text = "Make It Spicy " + "[" + orderToDisplay.spicyness.ToString() + "]";
        chunkyText.text = "Make It Chunky " + "[" + orderToDisplay.chunkiness.ToString() + "]";
        sweetnessText.text = "Make It Sweet " + "[" + orderToDisplay.sweetness.ToString() + "]";

        // Displaying meat veg preference //
        if (orderToDisplay.noMeat == false && orderToDisplay.noVeg == false)
        {
            meatText.text = "Meat and veg allowed";
        }
        else if (orderToDisplay.noMeat == true)
        {
            meatText.text = "No meat";
        }
        else if (orderToDisplay.noVeg == true)
        {
            meatText.text = "No veg";
        }
        // ----------------------------- //

        colourText.text = "Colour " + "[" + orderToDisplay.colourPreference.name + "]";

        // ----------------- Displaying proper title for the current order monitor ----------------- //
        if (thisMonitor == MonitorType.CURRENT_ORDER_MONITOR)
        {
            if (orderManager.acceptedOrders.Count > 0 && orderManager.selectedOrder == orderManager.acceptedOrders[0])
            {
                title.text = "Current Order";
            }
            else
            {
                title.text = "Next Order";
            }
        }


    }
    public void DisplaySecondaryMenu()
    {
        secondaryScreen.gameObject.SetActive(true);
        mainScreen.gameObject.SetActive(false);
    }



    // Override to help display a secondary screen with ingredient stats. //
    public void DisplaySecondaryMenu(Ingredient ingredient)
    {
        secondaryScreen.gameObject.SetActive(true);
        mainScreen.gameObject.SetActive(false);

        ingredientNameText.text = ingredient.ingredientName;
        spicyText.text = "Spicy " + "[" + ingredient.spicyness.ToString() + "]";
        chunkyText.text = "Chunky " + "[" + ingredient.chunkyness.ToString() + "]";
        meatText.text = "Meat " + "[" + ingredient.isMeat.ToString() + "]";
        colourText.text = "Colour " + "[" + ingredient.colour.name + "]";
        sweetnessText.text = "Sweet " + "[" + ingredient.sweetness.ToString() + "]";

        foodIcon.sprite = ingredient.icon.sprite;
    }
    public void SetScreenState(ScreenState newState)
    {
        currentState = newState;
    }
}
