using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;


public enum ScreenState
{ 
    MAIN_MENU,
    SECONDARY
}
public class MonitorScreen : MonoBehaviour
{

    public Transform mainScreen;
    public Transform secondaryScreen;

    public ScreenState currentState = ScreenState.MAIN_MENU;


    // Because our item fabricator needs to display ingredient stats, we need a reference to the ingredient. //
    public Ingredient currentIngredientDisplay;

    // Text/dropdownboxes and other UI elements go here. //
    public TextMeshProUGUI ingredientNameText;
    public TextMeshProUGUI spicyText;
    public TextMeshProUGUI chunkyText;
    public TextMeshProUGUI meatText;
    public TextMeshProUGUI colourText;


    // Start is called before the first frame update
    void Start()
    {

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
                DisplayMainMenu();
                break;
            case ScreenState.SECONDARY:
                if (currentIngredientDisplay)
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
        colourText.text = "Colour " + "[" + "-" + "]";
    }
    public void SetScreenState(ScreenState newState)
    {
        currentState = newState;
    }
}
