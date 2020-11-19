using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;


public enum MonitorType
{ 
    ITEM_FABRICATOR_MONITOR,
    NEW_ORDER_MONITOR,
    CURRENT_ORDER_MONITOR,
    CANON_MONITOR
}
public enum ScreenState
{ 
    
    MAIN_MENU,
    SECONDARY,
    OFFLINE,
}
public class MonitorScreen : MonoBehaviour
{

    GameManager gameManager;
    OrderManager orderManager;
    Canon theCanon;


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


    // Text for canon display. //
    public TextMeshProUGUI canonTitle;

    public TextMeshProUGUI canonSpicyText;
    public TextMeshProUGUI canonChunkyText;
    public TextMeshProUGUI canonMeatText;
    public TextMeshProUGUI canonColourText;

    public TextMeshProUGUI canonSweetnessText;





    // Start is called before the first frame update
    void Start()
    {
        gameManager = GameManager.GetInstance();
        orderManager = gameManager.orderManager;

        theCanon = gameManager.cookingManager.theCanon;
    }

    // Update is called once per frame
    void Update()
    {
        DisplayCurrentScreen();
        if (Input.GetKeyDown(KeyCode.H))
        {
            if (currentState == ScreenState.OFFLINE)
            {
                currentState = ScreenState.MAIN_MENU;
            }
            else
            {
                currentState = ScreenState.OFFLINE;
            }
        }
    }


	


	public void DisplayCurrentScreen()
    {
        switch (currentState)
        {
            case ScreenState.OFFLINE:
                mainScreen.gameObject.SetActive(false);
                if (secondaryScreen != null)
                { 
                    secondaryScreen.gameObject.SetActive(false);
                }
                break;
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
                else if (thisMonitor == MonitorType.CANON_MONITOR)
                {
                    if (orderManager.acceptedOrders.Count > 0)
                    {

                        if (theCanon.currentSoup != null)
                        {
                            DisplayMainMenu(theCanon.currentSoup.theSoup, orderManager.acceptedOrders[0]);
                        }
                        else
                        {
                            DisplayMainMenu(null, orderManager.acceptedOrders[0]);
                        }
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

    public void DisplayMainMenu(Soup soup, Order orderToDisplay)
    {
        DisplayMainMenu(orderToDisplay);

        if (soup != null)
        {
            canonSpicyText.text = "Spicy " + "[" + soup.spicyValue.ToString() + "]";
            canonChunkyText.text = "Chunky " + "[" + soup.chunkyValue.ToString() + "]";
            canonSweetnessText.text = "Sweet " + "[" + soup.sweetnessValue.ToString() + "]";


            if (soup.spicyValue >= OrderManager.CalculateLowerHalf(orderToDisplay.spicyness) && soup.spicyValue <= OrderManager.CalculateUpperHalf(orderToDisplay.spicyness))
            {
                canonSpicyText.color = Color.green;
            }
            else
            {
                canonSpicyText.color = Color.red;
            }

            if (soup.chunkyValue >= OrderManager.CalculateLowerHalf(orderToDisplay.chunkiness) && soup.chunkyValue <= OrderManager.CalculateUpperHalf(orderToDisplay.chunkiness))
            {
                canonChunkyText.color = Color.green;
            }
            else
            {
                canonChunkyText.color = Color.red;
            }

            if (soup.sweetnessValue >= OrderManager.CalculateLowerHalf(orderToDisplay.sweetness) && soup.sweetnessValue <= OrderManager.CalculateUpperHalf(orderToDisplay.sweetness))
            {
                canonSweetnessText.color = Color.green;
            }
            else
            {
                canonSweetnessText.color = Color.red;
            }


            if (soup.colour.name == orderToDisplay.colourPreference.name)
            {
                canonColourText.color = Color.green;
            }
            else
            {
                canonColourText.color = Color.red;
            }

            // Displaying meat veg preference //
            if (soup.ContainsMeat() && soup.ContainsVeg())
            {
                canonMeatText.text = "Contains meat and veg";

                if (!orderToDisplay.noMeat && !orderToDisplay.noVeg)
                {
                    canonMeatText.color = Color.green;
                }
                else
                {
                    canonMeatText.color = Color.red;
                }
            }
            else if (soup.ContainsMeat())
            {
                canonMeatText.text = "Contains meat";
                if (!orderToDisplay.noMeat)
                {
                    canonMeatText.color = Color.green;
                }
                else
                {
                    canonMeatText.color = Color.red;
                }
            }
            else if (soup.ContainsVeg())
            {
                canonMeatText.text = "Contains veg";
                if (!orderToDisplay.noVeg)
                {
                    canonMeatText.color = Color.green;
                }
                else
                {
                    canonMeatText.color = Color.red;
                }
            }
            else
            {
                canonMeatText.text = "Theres no ingredients";
                canonMeatText.color = Color.red;
            }
            // ----------------------------- //

            canonColourText.text = "Colour " + "[" + soup.colour.name + "]";
        }
        else
        {
            canonSpicyText.text = "Spicy " + "[" + "-" + "]";
            canonChunkyText.text = "Chunky " + "[" + "-" + "]";
            canonSweetnessText.text = "Sweet " + "[" + "-" + "]";

            // Displaying meat veg preference //
            canonMeatText.text = "Meat/Veg [-]";
            // ----------------------------- //

            canonColourText.text = "Colour " + "[" + "-" + "]";

            // Resetting Colours //
            canonSpicyText.color = Color.white;
            canonSweetnessText.color = Color.white;
            canonChunkyText.color = Color.white;
            canonColourText.color = Color.white;
            canonMeatText.color = Color.white;

        }
    }
    public void DisplayMainMenu(Order orderToDisplay)
    {
        if (!mainScreen.gameObject.activeSelf)
        { 
            mainScreen.gameObject.SetActive(true);
        }
        //secondaryScreen.gameObject.SetActive(false);

        spicyText.text = "Make It Spicy " + "[" + OrderManager.CalculateLowerHalf(orderToDisplay.spicyness).ToString() + " - " + OrderManager.CalculateUpperHalf(orderToDisplay.spicyness).ToString() + "]";
        chunkyText.text = "Make It Chunky " + "[" + OrderManager.CalculateLowerHalf(orderToDisplay.chunkiness).ToString() + " - " + OrderManager.CalculateUpperHalf(orderToDisplay.chunkiness).ToString() + "]";
        sweetnessText.text = "Make It Sweet " + "[" + OrderManager.CalculateLowerHalf(orderToDisplay.sweetness).ToString() + " - " + OrderManager.CalculateUpperHalf(orderToDisplay.sweetness).ToString() + "]";

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
