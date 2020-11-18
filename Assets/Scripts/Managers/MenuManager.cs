using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public enum MenuState
{ 
    pauseMenu,
    orderMenu,
    none
}

public class MenuManager : MonoBehaviour
{
    // Singelton hehe. //
    GameManager gameManager;
    CookingManager cookingManager;
    OrderManager orderManager;

    // ----------------------- Appliance References ----------------------- //
    SoupCatcher theCatcher;
    // -------------------------------------------------------------------- //


    private MenuState currentState = global::MenuState.none;

    private MouseLook playersMouseLook;
   

    public Transform playerCamera;

    public Canvas orderUI;
    public Canvas pauseUI;
    public Canvas debugUI;
    public Canvas gameOverUI;
    

    // Timers //
    float orderCreatedTextTimer;
    float orderSubmittedTextTimer;

    // Time left. //
    public TextMeshProUGUI timeLeftText;

    [Header("Order Creation")]
    public TextMeshProUGUI orderCreatedText;
    public TMP_Dropdown colourDropdown;
    public TMP_Dropdown meatVegDropdown;
    public TMP_InputField spicyInput;
    public TMP_InputField chunkyInput;


    [Header("Soup Display")]
    public TextMeshProUGUI colourDisplayText;
    public TextMeshProUGUI meatVegDisplayText;
    public TextMeshProUGUI spicyDisplayText;
    public TextMeshProUGUI chunkyDisplayText;

    [Header("Player UI")]
    public TextMeshProUGUI heldItemText;
    public TextMeshProUGUI selectedItemText;
    public TextMeshProUGUI selectedApplianceText;

    [Header("Appliance Things")]
    public TextMeshProUGUI currentCookingOrbState;
    public TextMeshProUGUI currentIngredients;
    List<string> currentIngredientsNames;
    string ingredientsText = "";

    public TextMeshProUGUI currentCatcherState;

    public TextMeshProUGUI heldCapsuleData;
    public TextMeshProUGUI currentPortionsData;

    public TextMeshProUGUI currentBlenderState;
    public TextMeshProUGUI currentBlenderButtonState;
    public TextMeshProUGUI currentBlenderIngredients;
    string blenderIngredientsText = "";

    public TextMeshProUGUI defaultMaterial;

    public TextMeshProUGUI ingredientTimer;
    public TextMeshProUGUI canSpawnIngredient;

    public TextMeshProUGUI currentSlicerState;

    public TextMeshProUGUI currentCanonState;

    public TextMeshProUGUI cookingOrbTimer;

    string trackedIngredientsText = "";
    public TextMeshProUGUI trackedIngredients;

    [Header("Blender Progress Stuff")]
    public TextMeshProUGUI blenderProgress;
    public TextMeshProUGUI blendingHalfDone;
    public TextMeshProUGUI blendingComplete;
    public TextMeshProUGUI blendingHalfTimer;
    public TextMeshProUGUI blendingCompleteTimer;

    [Header("Order Monitor Display Stuff")]
    public TextMeshProUGUI unLoadedText;
    public TextMeshProUGUI soupStatsText;


    public Canvas newOrderCanvas;
    public Canvas currentOrderCanvas;
    
    [Header("Order Request Texts")]
    public TextMeshProUGUI wantedSpicy;
    public TextMeshProUGUI wantedChunky;
    public TextMeshProUGUI wantedColour;
    public TextMeshProUGUI wantedMeatVegPref;

    [Header("Current Order Text")]
    public TextMeshProUGUI requestedSpicy;
    public TextMeshProUGUI requestedChunky;
    public TextMeshProUGUI requestedColour;
    public TextMeshProUGUI requestedMeatVegPref;

    [Header("Score and Order Submit Text")]
    public TextMeshProUGUI adjustableSubmittedOrderText;
    public TextMeshProUGUI playerPoints;

    public static TextMeshProUGUI submittedOrderText;


    [Header("Throwing Mechanics")]
    public TextMeshProUGUI throwCharge;
    public TextMeshProUGUI throwTimer;
    public TextMeshProUGUI throwState;


    // Seperators for ease of access //
    Transform soupOrganiser;
    Transform orderOrganiser;
    Transform currentOrderOrganiser;

    // Start is called before the first frame update
    void Start()
    {
        // Singleton hehe. //
        gameManager = GameManager.GetInstance();
        cookingManager = gameManager.cookingManager;
        orderManager = gameManager.orderManager;

        // Setting my timers to 0 safely //
        orderSubmittedTextTimer = 0;


        orderCreatedText.gameObject.SetActive(false);

        TextMeshProUGUI colourDropdownLabel = colourDropdown.transform.Find("Label").GetComponent<TextMeshProUGUI>();
        TextMeshProUGUI meatVegDropdownLabel = meatVegDropdown.transform.Find("Label").GetComponent<TextMeshProUGUI>();

        // Initialising static texts to their adjustable counter parts. //
        submittedOrderText = adjustableSubmittedOrderText;
   
        colourDropdown.options.Clear();
        colourDropdownLabel.text = "None";

        meatVegDropdown.options.Clear();
        meatVegDropdownLabel.text = "None";


        PopulateColourDropdownOptions(colourDropdown);
        PopulateMeatVegDropdownOptions(meatVegDropdown);

        playersMouseLook = playerCamera.GetComponent<MouseLook>();


        // ---------------------- Initialising Appliance References ---------------------- //
        theCatcher = gameManager.cookingManager.theCatcher;
        // ------------------------------------------------------------------------------- //


    }

    // Update is called once per frame
    void Update()
    {
        MenuState();


        // Doing timer things //
        if (submittedOrderText.gameObject.activeInHierarchy)
        { 
            orderSubmittedTextTimer += Time.deltaTime;
            if (orderSubmittedTextTimer >= 3)
            {
                submittedOrderText.gameObject.SetActive(false);
                orderCreatedTextTimer = 0;
            }    
        }

        // Making the "Order Created" text disappear after a while.
        orderCreatedTextTimer += Time.deltaTime;

        if (orderCreatedTextTimer >= 2)
        {
            orderCreatedText.gameObject.SetActive(false);
        }


        // Display player UI stuff // 
        DisplayHeldItem();
        DisplaySelectedAppliance();
        DisplaySelectedItem();

        DisplayCurrentCookingOrbState();
        DisplayCurrentIngredients();

        DisplayCurrentCatcherState();

        DisplayHeldCapsuleData();
        DisplayCurrentPortionsData();

        DisplayBlenderIngredients();
        DisplayBlenderState();

        DisplayDefaultMaterial();

        DisplayTimeLeft();
        DisplaySlicerState();

        DisplayBlenderButtonState();
        DisplayBlenderProgress();

        DisplayThrowMechanics();

        DisplayCanonState();

        DisplayCookingOrbTimer();


        //DisplayIngredientTimer();

        // Displaying order/canon monitor ui stuff //
        DisplayCanonMonitor();

        //DisplayOrderScreens();

        // Displaying players score. //
        DisplayPlayerPoints();

        
    }

    void MenuState()
    {
        switch (currentState)
        {
            case global::MenuState.pauseMenu:
                orderUI.gameObject.SetActive(false);
                pauseUI.gameObject.SetActive(true);
                playerCamera.GetComponent<MouseLook>().currentCameraMode = CameraMode.pauseMode;
              
                if (Input.GetKeyDown(KeyCode.Escape))
                {
                    currentState = global::MenuState.none;
                    playerCamera.GetComponent<MouseLook>().currentCameraMode = CameraMode.FPS_CONTROL;
                    Cursor.lockState = CursorLockMode.Locked;
                    Cursor.visible = false;
                    Time.timeScale = 1;
                }
                break;
            case global::MenuState.orderMenu:
                orderUI.gameObject.SetActive(true);
                pauseUI.gameObject.SetActive(false);
                playerCamera.GetComponent<MouseLook>().currentCameraMode = CameraMode.pauseMode;
                if (Input.GetKeyDown(KeyCode.O))
                {
                    currentState = global::MenuState.none;
                    playerCamera.GetComponent<MouseLook>().currentCameraMode = CameraMode.FPS_CONTROL;
                    Cursor.lockState = CursorLockMode.Locked;
                }
                break;
            case global::MenuState.none:        
                orderUI.gameObject.SetActive(false);
                pauseUI.gameObject.SetActive(false);

                if (Input.GetKeyDown(KeyCode.O))
                {
                    currentState = global::MenuState.orderMenu;
                }
                else if (Input.GetKeyDown(KeyCode.Escape))
                {
                    currentState = global::MenuState.pauseMenu;
                    Cursor.visible = true;
                }
                else if (Input.GetKeyDown(KeyCode.F3))
                {
                    if (debugUI.gameObject.activeSelf)
                    {
                        debugUI.gameObject.SetActive(false);
                    }
                    else
                    {
                        debugUI.gameObject.SetActive(true);
                    }
                }
                break;
            
        }
    }

    void PopulateColourDropdownOptions(TMP_Dropdown dropDownBox)
    {
        dropDownBox.options.Add(new TMP_Dropdown.OptionData("None"));
    }

    void PopulateMeatVegDropdownOptions(TMP_Dropdown dropDownBox)
    {
        dropDownBox.options.Add(new TMP_Dropdown.OptionData("None"));
        dropDownBox.options.Add(new TMP_Dropdown.OptionData("No Meat"));
        dropDownBox.options.Add(new TMP_Dropdown.OptionData("No Greens"));
    }


    // This is the super old way of creating orders. //
    public void CreateOrder()
    {
        orderManager.SendOrder(OrderManager.ManuallyCreateOrder(colourDropdown, meatVegDropdown, spicyInput, chunkyInput));
        orderCreatedText.gameObject.SetActive(true);
        orderCreatedTextTimer = 0;

        DisplayCurrentOrder(colourDisplayText, meatVegDisplayText, spicyDisplayText, chunkyDisplayText);
          
    }

    void DisplayCurrentOrder(TextMeshProUGUI colour, TextMeshProUGUI meatVeg, TextMeshProUGUI spicy, TextMeshProUGUI chunky)
    {
        //soup.text = OrderManager.currentOrders[0].mainSoup.soupName;
        colour.text = orderManager.requestedOrders[0].colourPreference.name;

        if (!orderManager.requestedOrders[0].noMeat && !orderManager.requestedOrders[0].noVeg)
        {
            meatVeg.text = "Meat and Veg allowed";
        }
        else if (orderManager.requestedOrders[0].noMeat && !orderManager.requestedOrders[0].noVeg)
        {
            meatVeg.text = "Meat not allowed";
        }
        else if (orderManager.requestedOrders[0].noVeg && !orderManager.requestedOrders[0].noMeat)
        {
            meatVeg.text = "Veg not allowed";
        }

        spicy.text = orderManager.requestedOrders[0].spicyness.ToString();
        chunky.text = orderManager.requestedOrders[0].chunkiness.ToString();

    }

    void DisplayHeldItem()
    {
        if (MouseLook.heldItem)
        {
            heldItemText.text = "Held Item: " + MouseLook.heldItem.name;
        }
        else
        {
            heldItemText.text = "Held Item: None";
        }
    }

    void DisplaySelectedItem()
    {
        if (MouseLook.selectedItem)
        {
            selectedItemText.text = "Selected Item: " + MouseLook.selectedItem.name;
        }
        else
        {
            selectedItemText.text = "None";
        }
    }

    void DisplaySelectedAppliance()
    {
        if (MouseLook.selectedAppliance)
        {
            selectedApplianceText.text = "Selected Appliance: " + MouseLook.selectedAppliance.name;
        }
        else
        {
            selectedApplianceText.text = "None";
        }
    }

    void DisplayCurrentCookingOrbState()
    {
        currentCookingOrbState.text = "Cooking Orb State: " + gameManager.cookingManager.theOrb.currentCookingOrbState.ToString();
    }
    void DisplayCurrentIngredients()
    {
        
        for (int i = 0; i < gameManager.cookingManager.theOrb.currentIngredients.Count; i++)
        {
            ingredientsText = ingredientsText + gameManager.cookingManager.theOrb.currentIngredients[i].GetComponent<Ingredient>().ingredientName + ", ";
        }
        currentIngredients.text = ingredientsText;
        ingredientsText = "Current Ingredients: ";


        // ----------------- Tracked Ingredients ------------------- //
        for (int i = 0; i < gameManager.cookingManager.theOrb.currentlyTrackedIngredients.Count; i++)
        {
            trackedIngredientsText = trackedIngredientsText + gameManager.cookingManager.theOrb.currentlyTrackedIngredients[i].GetComponent<Ingredient>().ingredientName + ", ";
        }
        trackedIngredients.text = trackedIngredientsText;
        trackedIngredientsText = "Current Ingredients: ";

    }

    void DisplayCurrentCatcherState()
    {
        currentCatcherState.text = "Catcher State: " + gameManager.cookingManager.theCatcher.currentCatcherState.ToString();
    }

    void DisplayCanonMonitor()
    {
        if (cookingManager.theCanon.isLoaded)
        {
            unLoadedText.gameObject.SetActive(false);
            soupStatsText.gameObject.SetActive(true);
            Soup soupData = cookingManager.theCanon.canonCapsule.GetComponent<SoupData>().theSoup;
            if (cookingManager.theCanon.canonCapsule.GetComponent<SoupData>().theSoup == null)
            {
                soupStatsText.text = "NULL soup.";
            }
            else
            {
                soupStatsText.text = "Soup is " + soupData.spicyValue + " spicy and " + soupData.chunkyValue + " chunky." + "and " + soupData.sweetnessValue + " sweet.";
            }

        }
        else if (!cookingManager.theCanon.isLoaded)
        {
            unLoadedText.gameObject.SetActive(true);
            soupStatsText.gameObject.SetActive(false);
        }
    }

    void DisplayHeldCapsuleData()
    {
        if (MouseLook.heldItem && MouseLook.heldItem.tag == "Capsule")
        {
            if (MouseLook.heldItem.GetComponent<SoupData>().theSoup == null)
            {
                heldCapsuleData.text = "Held Capsule Soup Data: True";
            }
            else
            {
                heldCapsuleData.text = "Held Capsule Soup Data: False";
            }
        }
        else
        {
            heldCapsuleData.text = "Held Capsule Soup Data: NULL";
        }
    }

    void DisplayCurrentPortionsData()
    {
        if (theCatcher.currentPortions.Count > 0 && theCatcher.currentPortions[0] == null)
        {
            currentPortionsData.text = "Soup Catcher Portions: NULL";
        }
        else if (theCatcher.currentPortions.Count > 0 && theCatcher.currentPortions[0] != null)
        {
            currentPortionsData.text = "Soup Catcher Portions: " + theCatcher.currentPortions.Count;
        }
        else if (theCatcher.currentPortions.Count == 0)
        {
            currentPortionsData.text = "Soup Catcher Portions: NULL";
        }
    }

    void DisplayOrderScreens()
    {
        if (OrderManager.currentScreenState == OrderScreenState.NEW_ORDER)
        {
            newOrderCanvas.gameObject.SetActive(true);
            currentOrderCanvas.gameObject.SetActive(false);

            UpdateNewOrder();
        }
        else if (OrderManager.currentScreenState == OrderScreenState.CURRENT_ORDER)
        {
            newOrderCanvas.gameObject.SetActive(false);
            currentOrderCanvas.gameObject.SetActive(true);

            UpdateCurrentOrder();
        }
    }

    void UpdateNewOrder()
    {
        // This if statement is to make sure your not trying to access requestedOrders if the list is empty. //
        if (orderManager.requestedOrders.Count > 0)
        { 
            wantedChunky.text = "Chunkyness: " + orderManager.requestedOrders[0].chunkiness.ToString();
            wantedSpicy.text = "Spicyness: " + orderManager.requestedOrders[0].spicyness.ToString();
            wantedColour.text = "No colours yet.";

            if (orderManager.requestedOrders[0].noMeat == false && orderManager.requestedOrders[0].noVeg == false)
            {
                wantedMeatVegPref.text = "Meat and veg allowed";
            }
            else if (orderManager.requestedOrders[0].noMeat == true)
            {
                wantedMeatVegPref.text = "No meat";
            }
            else if (orderManager.requestedOrders[0].noVeg == true)
            {
                wantedMeatVegPref.text = "No veg";
            }
        }
    }

    void UpdateCurrentOrder()
    {
        if (orderManager.acceptedOrders.Count > 0)
        {
            Order theOrder = orderManager.acceptedOrders[0];

            requestedSpicy.text = "Spicy: " + theOrder.spicyness;
            requestedChunky.text = "Chunky: " + theOrder.chunkiness;
            requestedColour.text = "There are no colours yet.";


            if (theOrder.noMeat == false && theOrder.noVeg == false)
            {
                requestedMeatVegPref.text = "Meat and veg allowed";
            }
            else if (theOrder.noMeat == true)
            {
                requestedMeatVegPref.text = "No meat";
            }
            else if (theOrder.noVeg == true)
            {
                requestedMeatVegPref.text = "No veg";
            }
        }
    }

    public void DisplayPlayerPoints()
    {
        playerPoints.text = "POINTS: " + ScoreManager.currentScore.ToString();
    }

    public static void DisplayOrderSubmittedText()
    {
        submittedOrderText.gameObject.SetActive(true);
    }

    public void DisplayBlenderState()
    {
        currentBlenderState.text = "Current Blender State: " + gameManager.cookingManager.theBlender.currentBlenderState.ToString();
    }
    public void DisplayBlenderIngredients()
    {
        for (int i = 0; i < gameManager.cookingManager.theBlender.currentBlenderIngredients.Count; i++)
        {
            blenderIngredientsText = blenderIngredientsText + gameManager.cookingManager.theBlender.currentBlenderIngredients[i].GetComponent<Ingredient>().ingredientName + ", ";
        }
        currentBlenderIngredients.text = blenderIngredientsText;
        blenderIngredientsText = "Current Blender Ingredients: ";
    }

    public void DisplayDefaultMaterial()
    {
        if (playerCamera.GetComponent<MouseLook>().defaultMat != null)
        {
            defaultMaterial.text = "Default Material: " + playerCamera.GetComponent<MouseLook>().defaultMat.ToString();
        }
        else
        {
            defaultMaterial.text = "Default Material: NULL";
        }
    }

    public void DisplayGameOver()
    {
        orderUI.gameObject.SetActive(false);
        debugUI.gameObject.SetActive(false);
        gameOverUI.gameObject.SetActive(true);
    }

    public void DisplayTimeLeft()
    {
        timeLeftText.text = gameManager.gameTime.ToString();
    }

    public void DisplaySlicerState()
    {
        currentSlicerState.text = "Slicer State: " + gameManager.cookingManager.theSlicer.currentSlicerState.ToString();
    }

    public void DisplayBlenderButtonState()
    {
        currentBlenderButtonState.text = "Blender Button State: " + gameManager.cookingManager.theBlender.currentBlenderButtonState.ToString();
    }

    public void DisplayBlenderProgress()
    {
        blenderProgress.text = "Blender Progress: " + gameManager.cookingManager.theBlender.blendProgress.ToString();
        blendingHalfDone.text = "Blending Half Done: " + gameManager.cookingManager.theBlender.isHalfBlended.ToString();
        blendingComplete.text = "Blending Complete: " + gameManager.cookingManager.theBlender.isFullBlended.ToString();
        blendingHalfTimer.text = gameManager.cookingManager.theBlender.continueButtonTimer.ToString() + "s";
        blendingCompleteTimer.text = gameManager.cookingManager.theBlender.completeButtonTimer.ToString() + "s";
    }

    public void DisplayThrowMechanics()
    {
        throwCharge.text = "ThrowCharge: " + gameManager.playerController.throwCharge.ToString();
        throwTimer.text = gameManager.playerController.throwingHeldDownTimer.ToString() + "s";
        throwState.text = gameManager.playerController.currentThrowCharge.ToString();

        switch (gameManager.playerController.currentThrowCharge)
        {
            case ThrowCharge.WEAK:
                throwCharge.color = Color.white;
                throwState.color = Color.white;
                break;
            case ThrowCharge.MEDIUM:
                throwCharge.color = Color.yellow;
                throwState.color = Color.yellow;
                break;
            case ThrowCharge.STRONG:
                throwCharge.color = Color.red;
                throwState.color = Color.red;
                break;
        }
    }

    public void DisplayCanonState()
    {
        if (cookingManager.theCanon.isLoaded)
        {
            currentCanonState.text = "CanonState: LOADED";
        }
        else
        {
            currentCanonState.text = "CanonState: UNLOADED";
        }

        //currentCanonState.text = "CanonState: " + cookingManager.theCanon.currentCanonState.ToString();
    }

    public void DisplayCookingOrbTimer() 
    {
        cookingOrbTimer.text = "CookingTimer: " + cookingManager.theOrb.cookingTimer.ToString() + "s";
    }



    




}


