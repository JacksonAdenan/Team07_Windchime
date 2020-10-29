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
    private MenuState currentState = global::MenuState.none;
    
   

    public Transform playerCamera;

    public Canvas orderUI;
    public Canvas pauseUI;
    public Canvas debugUI;
    

    // Timers //
    float orderCreatedTextTimer;
    float orderSubmittedTextTimer;

    // Order creation stuff //
    public TextMeshProUGUI orderCreatedText;
    public TMP_Dropdown colourDropdown;
    public TMP_Dropdown meatVegDropdown;
    public TMP_InputField spicyInput;
    public TMP_InputField chunkyInput;


    // Current order text stuff //
    public TextMeshProUGUI colourDisplayText;
    public TextMeshProUGUI meatVegDisplayText;
    public TextMeshProUGUI spicyDisplayText;
    public TextMeshProUGUI chunkyDisplayText;

    // Player UI stuff //
    public TextMeshProUGUI heldItemText;
    public TextMeshProUGUI selectedItemText;
    public TextMeshProUGUI selectedApplianceText;

    public TextMeshProUGUI currentCookingOrbState;
    public TextMeshProUGUI currentIngredients;
    List<string> currentIngredientsNames;
    string ingredientsText = "";

    public TextMeshProUGUI currentCatcherState;

    public TextMeshProUGUI heldCapsuleData;
    public TextMeshProUGUI currentPortionsData;

    public TextMeshProUGUI currentBlenderState;
    public TextMeshProUGUI currentBlenderIngredients;
    string blenderIngredientsText = "";

    public TextMeshProUGUI defaultMaterial;

    // Order monitor display stuff //
    public TextMeshProUGUI unLoadedText;
    public TextMeshProUGUI soupStatsText;


    public Canvas newOrderCanvas;
    public Canvas currentOrderCanvas;
    
    // These are the order request text boxes //
    public TextMeshProUGUI wantedSpicy;
    public TextMeshProUGUI wantedChunky;
    public TextMeshProUGUI wantedColour;
    public TextMeshProUGUI wantedMeatVegPref;

    // These are the current order text boxes //
    public TextMeshProUGUI requestedSpicy;
    public TextMeshProUGUI requestedChunky;
    public TextMeshProUGUI requestedColour;
    public TextMeshProUGUI requestedMeatVegPref;

    // Score and order submission UI //
    public TextMeshProUGUI adjustableSubmittedOrderText;
    public TextMeshProUGUI playerPoints;

    public static TextMeshProUGUI submittedOrderText;


    // Seperators for ease of access //
    Transform soupOrganiser;
    Transform orderOrganiser;
    Transform currentOrderOrganiser;

    // Start is called before the first frame update
    void Start()
    {
        //heldItemText = debugUI.transform.Find("heldItem").GetComponent<TextMeshProUGUI>();
        //selectedItemText = debugUI.transform.Find("selectedItem").GetComponent<TextMeshProUGUI>();
        //selectedApplianceText = debugUI.transform.Find("selectedAppliance").GetComponent<TextMeshProUGUI>();

        // Setting the "seperators" to make it easier to find children under the seperator. //
        //soupOrganiser = orderUI.transform.Find("SoupStuff");
        //orderOrganiser = orderUI.transform.Find("OrderCreationStuff");
        //currentOrderOrganiser = orderUI.transform.Find("OrderList");

        //soupDisplayText = currentOrderOrganiser.Find("soupName").GetComponent<TextMeshProUGUI>();
        //colourDisplayText = currentOrderOrganiser.Find("colourPreference").GetComponent<TextMeshProUGUI>();
        //meatVegDisplayText = currentOrderOrganiser.Find("meatVegPref").GetComponent<TextMeshProUGUI>();
        //spicyDisplayText = currentOrderOrganiser.Find("spicy").GetComponent<TextMeshProUGUI>();
        //chunkyDisplayText = currentOrderOrganiser.Find("chunky").GetComponent<TextMeshProUGUI>();

        // orderCreatedText = orderOrganiser.Find("orderCreatedText").GetComponent<TextMeshProUGUI>();

        // Setting my timers to 0 safely //
        orderSubmittedTextTimer = 0;


        orderCreatedText.gameObject.SetActive(false);


        

     

        //colourDropdown = orderOrganiser.Find("colourDropdown").GetComponent<TMP_Dropdown>();
        TextMeshProUGUI colourDropdownLabel = colourDropdown.transform.Find("Label").GetComponent<TextMeshProUGUI>();

        //meatVegDropdown = orderOrganiser.Find("meatVegDropdown").GetComponent<TMP_Dropdown>();
        TextMeshProUGUI meatVegDropdownLabel = meatVegDropdown.transform.Find("Label").GetComponent<TextMeshProUGUI>();

        //spicyInput = orderOrganiser.Find("spicyInput").GetComponent<TMP_InputField>();
        //chunkyInput = orderOrganiser.Find("chunkyInput").GetComponent<TMP_InputField>();

        // Initialising static texts to their adjustable counter parts. //
        submittedOrderText = adjustableSubmittedOrderText;

        

        
        colourDropdown.options.Clear();
        colourDropdownLabel.text = "None";

        meatVegDropdown.options.Clear();
        meatVegDropdownLabel.text = "None";


        PopulateColourDropdownOptions(colourDropdown);
        PopulateMeatVegDropdownOptions(meatVegDropdown);


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

        // Displaying available soups //
        DisplayAvailableSoups(soupOrganiser);

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


        // Displaying order/canon monitor ui stuff //
        DisplayCanonMonitor();

        DisplayOrderScreens();

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
                if (Input.GetKeyDown(KeyCode.P))
                {
                    currentState = global::MenuState.none;
                    playerCamera.GetComponent<MouseLook>().currentCameraMode = CameraMode.FPS_CONTROL;
                    Cursor.lockState = CursorLockMode.Locked;
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
                else if (Input.GetKeyDown(KeyCode.P))
                {
                    currentState = global::MenuState.pauseMenu;
                }
                break;
            
        }
    }

    //void PopulateSoupDropdownOptions(TMP_Dropdown dropDownBox)
    //{
    //    for (int i = 0; i < CookingManager.allSoups.Count; i++)
    //    {
    //        
    //        dropDownBox.options.Add(new TMP_Dropdown.OptionData(CookingManager.allSoups[i].soupName));
    //    }
    //}
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


    public void CreateOrder()
    {
        OrderManager.SendOrder(OrderManager.ManuallyCreateOrder(colourDropdown, meatVegDropdown, spicyInput, chunkyInput));
        orderCreatedText.gameObject.SetActive(true);
        orderCreatedTextTimer = 0;

        DisplayCurrentOrder(colourDisplayText, meatVegDisplayText, spicyDisplayText, chunkyDisplayText);
          
    }

    void DisplayCurrentOrder(TextMeshProUGUI colour, TextMeshProUGUI meatVeg, TextMeshProUGUI spicy, TextMeshProUGUI chunky)
    {
        //soup.text = OrderManager.currentOrders[0].mainSoup.soupName;
        colour.text = OrderManager.requestedOrders[0].colourPreference.name;

        if (!OrderManager.requestedOrders[0].noMeat && !OrderManager.requestedOrders[0].noVeg)
        {
            meatVeg.text = "Meat and Veg allowed";
        }
        else if (OrderManager.requestedOrders[0].noMeat && !OrderManager.requestedOrders[0].noVeg)
        {
            meatVeg.text = "Meat not allowed";
        }
        else if (OrderManager.requestedOrders[0].noVeg && !OrderManager.requestedOrders[0].noMeat)
        {
            meatVeg.text = "Veg not allowed";
        }

        spicy.text = OrderManager.requestedOrders[0].spicyness.ToString();
        chunky.text = OrderManager.requestedOrders[0].chunkiness.ToString();

    }

    void DisplayHeldItem()
    {
        if (MouseLook.heldItem)
        {
            heldItemText.text = MouseLook.heldItem.name;
        }
        else
        {
            heldItemText.text = "None";
        }
    }

    void DisplaySelectedItem()
    {
        if (MouseLook.selectedItem)
        {
            selectedItemText.text = MouseLook.selectedItem.name;
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
            selectedApplianceText.text = MouseLook.selectedAppliance.name;
        }
        else
        {
            selectedApplianceText.text = "None";
        }
    }

    void DisplayCurrentCookingOrbState()
    {
        currentCookingOrbState.text = CookingManager.currentCookingOrbState.ToString();
    }
    void DisplayCurrentIngredients()
    {
        
        for (int i = 0; i < CookingManager.currentIngredients.Count; i++)
        {
            ingredientsText = ingredientsText + CookingManager.currentIngredients[i].GetComponent<Ingredient>().ingredientName + ", ";
        }
        currentIngredients.text = ingredientsText;
        ingredientsText = "";
    }

    void DisplayCurrentCatcherState()
    {
        currentCatcherState.text = CookingManager.currentCatcherState.ToString();
    }

    void DisplayCanonMonitor()
    {
        if (CookingManager.isLoaded)
        {
            unLoadedText.gameObject.SetActive(false);
            soupStatsText.gameObject.SetActive(true);
            Soup soupData = CookingManager.canonCapsule.GetComponent<SoupData>().theSoup;
            if (CookingManager.canonCapsule.GetComponent<SoupData>().theSoup == null)
            {
                soupStatsText.text = "NULL soup.";
            }
            else
            {
                soupStatsText.text = "Soup is " + soupData.spicyValue + " spicy and " + soupData.chunkyValue + " chunky.";
            }

        }
        else if (!CookingManager.isLoaded)
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
                heldCapsuleData.text = "This is soup is null.";
            }
            else
            {
                heldCapsuleData.text = "This soup is NOT null.";
            }
        }
    }

    void DisplayCurrentPortionsData()
    {
        if (CookingManager.currentPortions.Count > 0 && CookingManager.currentPortions[0] == null)
        {
            currentPortionsData.text = "Portion is null.";
        }
        else if (CookingManager.currentPortions.Count > 0 && CookingManager.currentPortions[0] != null)
        {
            currentPortionsData.text = "Portion is NOT null!";
        }
        else if (CookingManager.currentPortions.Count == 0)
        {
            currentPortionsData.text = "there are no portions.";
        }
    }

    void DisplayAvailableSoups(Transform parentOfUI)
    {
        //// Setting all the soup titles. These will be stored to make finding their children easier. //
        //Transform soup1Parent = parentOfUI.transform.Find("soup1Name");
        //Transform soup2Parent = parentOfUI.transform.Find("soup2Name");
        //Transform soup3Parent = parentOfUI.transform.Find("soup3Name");
        //
        //soup1Parent.GetComponent<TextMeshProUGUI>().text = CookingManager.allSoups[0].soupName;
        //soup2Parent.GetComponent<TextMeshProUGUI>().text = CookingManager.allSoups[1].soupName;
        //soup3Parent.GetComponent<TextMeshProUGUI>().text = CookingManager.allSoups[2].soupName;
        //
        //
        //soup1Parent.transform.Find("spicyValue").GetComponent<TextMeshProUGUI>().text = CookingManager.allSoups[0].spicyValue.ToString();
        //soup1Parent.transform.Find("chunkyValue").GetComponent<TextMeshProUGUI>().text = CookingManager.allSoups[0].chunkyValue.ToString();
        //soup1Parent.transform.Find("restrictedIngredient").GetComponent<TextMeshProUGUI>().text = "Restricted: " + CookingManager.allSoups[0].restrictedIngredient.name;
        //
        //soup2Parent.transform.Find("spicyValue").GetComponent<TextMeshProUGUI>().text = CookingManager.allSoups[1].spicyValue.ToString();
        //soup2Parent.transform.Find("chunkyValue").GetComponent<TextMeshProUGUI>().text = CookingManager.allSoups[1].chunkyValue.ToString();
        //soup2Parent.transform.Find("restrictedIngredient").GetComponent<TextMeshProUGUI>().text = "Restricted: " + CookingManager.allSoups[1].restrictedIngredient.name;
        //
        //soup3Parent.transform.Find("spicyValue").GetComponent<TextMeshProUGUI>().text = CookingManager.allSoups[2].spicyValue.ToString();
        //soup3Parent.transform.Find("chunkyValue").GetComponent<TextMeshProUGUI>().text = CookingManager.allSoups[2].chunkyValue.ToString();
        //soup3Parent.transform.Find("restrictedIngredient").GetComponent<TextMeshProUGUI>().text = "Restricted: " + CookingManager.allSoups[2].restrictedIngredient.name;
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
        if (OrderManager.requestedOrders.Count > 0)
        { 
            wantedChunky.text = "Chunkyness: " + OrderManager.requestedOrders[0].chunkiness.ToString();
            wantedSpicy.text = "Spicyness: " + OrderManager.requestedOrders[0].spicyness.ToString();
            wantedColour.text = "No colours yet.";

            if (OrderManager.requestedOrders[0].noMeat == false && OrderManager.requestedOrders[0].noVeg == false)
            {
                wantedMeatVegPref.text = "Meat and veg allowed";
            }
            else if (OrderManager.requestedOrders[0].noMeat == true)
            {
                wantedMeatVegPref.text = "No meat";
            }
            else if (OrderManager.requestedOrders[0].noVeg == true)
            {
                wantedMeatVegPref.text = "No veg";
            }
        }
    }

    void UpdateCurrentOrder()
    {
        if (OrderManager.acceptedOrders.Count > 0)
        {
            Order theOrder = OrderManager.acceptedOrders[0];

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
        currentBlenderState.text = CookingManager.currentBlenderState.ToString();
    }
    public void DisplayBlenderIngredients()
    {
        for (int i = 0; i < CookingManager.currentBlenderIngredients.Count; i++)
        {
            blenderIngredientsText = blenderIngredientsText + CookingManager.currentBlenderIngredients[i].GetComponent<Ingredient>().ingredientName + ", ";
        }
        currentBlenderIngredients.text = blenderIngredientsText;
        blenderIngredientsText = "";
    }

    public void DisplayDefaultMaterial()
    {
        if (playerCamera.GetComponent<MouseLook>().defaultMat != null)
        {
            defaultMaterial.text = playerCamera.GetComponent<MouseLook>().defaultMat.ToString();
        }
        else
        {
            defaultMaterial.text = "NULL";
        }
    }

    




}


