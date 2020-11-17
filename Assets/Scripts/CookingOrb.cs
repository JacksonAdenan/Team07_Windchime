using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public class CookingOrb
{
    GameManager gameManager;

    // Appliance prefab. //
    public Transform cookingOrb;

    // I don't know why I have this?? Okay I think I'm using this as some sort of weird boolean where if it's set to null I treat that as false. I will probably change this to a real boolean one day. //
    public Transform occupyingSoup;

    // Cookingorb stats and current things. //
    public CookingOrbState currentCookingOrbState;
    public List<Transform> currentIngredients;
    public List<Transform> currentlyTrackedIngredients;

    [Header("Don't modify these in inspector.")]
    public float currentSpicy;
    public float currentChunky;
    public float currentSweet;
    public Colour currentColour;

    [Header("Don't modify the timer. If you want to increase cooking time change Cooking Duration")]
    public float cookingTimer = 0;
    [Tooltip("The time it takes to cook a soup.")]
    public float cookingDuration = 0;
    [Tooltip("Speed at which the thrown water lerps to the centre.")]
    public float waterCenteringSpeed = 0;
    private bool isCentered = false;

    // Prefab that will be set to active if there is a soup in the cooking orb. //
    [Header("Prefabs to display whats in the orb.")]
    public Transform soupOrb;

    // DONT SET THIS INSPECTOR //
    [Tooltip("Please don't set this in inspector.")]
    public Transform water;

    [Header("Soup Colour Things")]
    public Shader waterShader;


    // Start is called before the first frame update
    public void Start()
    {

        gameManager = GameManager.GetInstance();

        currentIngredients = new List<Transform>();
        currentlyTrackedIngredients = new List<Transform>();

        // Initialising cooking orb things. //
        currentCookingOrbState = CookingOrbState.EMPTY;
        currentSpicy = 0;
        currentChunky = 0;
        currentSweet = 0;
    }

    // Update is called once per frame
    public void Update()
    {
        // Adding tracked ingredients to the cooking orb. //
        if (currentCookingOrbState == CookingOrbState.EMPTY_WATER || currentCookingOrbState == CookingOrbState.INGREDIENTS_AND_WATER)
        {
            AddTrackedIngredients();
        }

        // Doing cooking timer. //
        if (currentCookingOrbState == CookingOrbState.COOKING)
        {
            cookingTimer += Time.deltaTime;
            if (cookingTimer >= 3)
            {
                // Resetting cookingTimer after cook. //
                cookingTimer = 0;

                currentCookingOrbState = CookingOrbState.OCCUPIED_SOUP;
                occupyingSoup.gameObject.SetActive(true);
                cookingOrb.GetComponent<Animator>().SetBool("IsOpen", false);
            }
        }


        // Moving water to the centre of the orb. //
        if (isCentered == false && water != null)
        {
            Debug.Log("CENTERING WATER...");
            MoveWaterToCenter();
        }

        // Cooking orb updates. //

        UpdateCookingOrbState();

        UpdateCookingOrbAnimation();

    }

    void UpdateCookingOrbAnimation()
    {
        switch (currentCookingOrbState)
        {
            case CookingOrbState.EMPTY:
                cookingOrb.GetComponent<Animator>().SetBool("IsOpen", true);
                break;
            case CookingOrbState.EMPTY_WATER:
                cookingOrb.GetComponent<Animator>().SetBool("IsOpen", true);
                break;
            case CookingOrbState.INGREDIENTS_AND_WATER:
                cookingOrb.GetComponent<Animator>().SetBool("IsOpen", true);
                break;
            case CookingOrbState.INGREDIENTS_NOWATER:
                cookingOrb.GetComponent<Animator>().SetBool("IsOpen", true);
                break;
            case CookingOrbState.OCCUPIED_SOUP:
                cookingOrb.GetComponent<Animator>().SetBool("IsOpen", true);
                break;
            case CookingOrbState.COOKING:
                cookingOrb.GetComponent<Animator>().SetBool("IsOpen", false);
                break;
        }
    }

    void UpdateCookingOrbState()
    {
        // ------------------------------------ Deleting the soup orb if it's currentPortions hit 0 ------------------------------------ //
        if (occupyingSoup != null && occupyingSoup.GetComponent<SoupData>().currentPortions <= 0)
        {
            occupyingSoup.gameObject.SetActive(false);
            occupyingSoup = null;

            // Freeing the cooking orb. //
            currentCookingOrbState = CookingOrbState.EMPTY;
        }
        // ----------------------------------------------------------------------------------------------------------------------------- //

        else if (currentIngredients.Count > 0 && currentCookingOrbState == CookingOrbState.EMPTY)
        {
            currentCookingOrbState = CookingOrbState.INGREDIENTS_NOWATER;
        }
        else if (currentIngredients.Count > 0 && currentCookingOrbState == CookingOrbState.EMPTY_WATER)
        {
            currentCookingOrbState = CookingOrbState.INGREDIENTS_AND_WATER;
        }
        else if (currentIngredients.Count == 0 && currentCookingOrbState == CookingOrbState.INGREDIENTS_AND_WATER)
        {
            currentCookingOrbState = CookingOrbState.EMPTY_WATER;
        }

    }

    // To be able to check if there is anything currently being tracked by the cooking orb.
    public bool IsTracking()
    {
        if (currentlyTrackedIngredients.Count > 0)
        {
            return true;
        }
        return false;
    }
    public void AddTrackedIngredients()
    {
        for (int i = currentlyTrackedIngredients.Count - 1; i > -1; i--)
        {
            currentIngredients.Add(currentlyTrackedIngredients[i]);
            currentlyTrackedIngredients.Remove(currentlyTrackedIngredients[i]);
        }
    }
    public void TrackIngredient(Transform ingredientToTrack)
    {
        currentlyTrackedIngredients.Add(ingredientToTrack);
        Debug.Log("Ingredient being tracked by cooking orb.");
    }
    public void StopTrackingIngredient(Transform ingredientToTrack)
    {
        currentlyTrackedIngredients.Remove(ingredientToTrack);
        Debug.Log("Ingredient stopped being tracked by cooking orb.");
    }
    public void AddIngredient(Transform ingredient)
    {
        currentIngredients.Add(ingredient);
        Debug.Log("Ingredient added to cooking orb.");
    }

    public void RemoveIngredient(Transform ingredient)
    {
        currentIngredients.Remove(ingredient);
        Debug.Log("Ingredient removed from cooking orb.");
    }

    public void CombineIngredient(Ingredient ingredient)
    {
        currentSpicy += ingredient.spicyness;
        currentChunky += ingredient.chunkyness;
        currentSweet += ingredient.sweetness;

    }


    // -------------------------------------------- Combining all the ingredients and returning a soup with all the stats. -------------------------------------------- //
    public Soup CookSoup()
    {
        //bool finishedCook = false;

        for (int i = 0; i < currentIngredients.Count; i++)
        {
            CombineIngredient(currentIngredients[i].GetComponent<Ingredient>());
        }

        Soup newSoup = new Soup(currentSpicy, currentChunky, currentSweet);

        for (int i = 0; i < currentIngredients.Count; i++)
        {
            newSoup.usedIngredients.Add(currentIngredients[i].GetComponent<Ingredient>());
        }

        // Resetting current cooking orb values to be ready for next soup
        currentSpicy = 0;
        currentChunky = 0;
        currentSweet = 0;
        
        //currentCookingOrbState = CookingOrbState.EMPTY;

        for (int i = currentIngredients.Count - 1; i > -1; i--)
        {
            currentIngredients[i].gameObject.SetActive(false);
            currentIngredients.Remove(currentIngredients[i]);
        }

        // Setting the colour by last ingredient in the soup //
        newSoup.colour = newSoup.usedIngredients[newSoup.usedIngredients.Count - 1].colour;

        return newSoup;
    }
    // ---------------------------------------------------------------------------------------------------------------------------------------------------------------- //


    // ------------------------- After we "CookSoup()", we use the returned soup and create a Soup object and attach the appropriate Soup stats to the SoupData script. ------------------------- //
    public void MakeSoup()
    {
        // Making the cooking orb state OCCUPIED. //
        currentCookingOrbState = CookingOrbState.COOKING;

        Soup newSoup = CookSoup();

        Transform newSoupOrb = GameObject.Instantiate(soupOrb, soupOrb.position, soupOrb.rotation);

        // The setting active is moved to the State Machine because of the added cooking timer mechanic. //
        //newSoupOrb.gameObject.SetActive(true);

        SoupData newSoupsData = newSoupOrb.GetComponent<SoupData>();

        newSoupsData.theSoup = newSoup;

        // Setting max amount of "portions" of soup. The player can keep grabbing soup until they've depleted all the portions. //

        // Hey get out of here! If you want to set the portion sizes do it through the inspector ! //

        //newSoupsData.currentPortions = 5;
        //newSoupsData.maxPortions = 5;

        occupyingSoup = newSoupOrb;

        // Setting the colour of the occupying soup. //
        Material newMaterial = new Material(waterShader);
        newMaterial.SetColor("Color_6EDA1D08", Colour.ConvertColour(newSoup.colour));
        occupyingSoup.GetComponent<Renderer>().material = newMaterial;


        // Removing and resetting stuff to do with the water display. //
        RemoveWater();
    }
    // ------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------ //

    public void AddWater(Transform waterOrb)
    {

        // This if statement prevents the added water from remaining the selected material. //
        if (MouseLook.selectedItem != null && MouseLook.selectedItem == waterOrb)
        {
            waterOrb.GetComponent<Renderer>().material = gameManager.playerController.defaultMat;

            // Have to have this otherwise default mat doesn't reset and the next item you look out will turn into water. //
            gameManager.playerController.defaultMat = null;
            MouseLook.selectedItem = null;
        }
        // -------------------------------------------------------------------------------- //

        water = GameObject.Instantiate(waterOrb, waterOrb.position, waterOrb.rotation);
        water.tag = "NonInteractableWater";
        water.GetComponent<SphereCollider>().isTrigger = true;

        // ------------------------------------------ Setting the CookingOrbState ------------------------------------------ //

        if (currentCookingOrbState == CookingOrbState.EMPTY)
        {
            currentCookingOrbState = CookingOrbState.EMPTY_WATER;
        }
        else if (currentCookingOrbState == CookingOrbState.INGREDIENTS_NOWATER)
        {
            currentCookingOrbState = CookingOrbState.INGREDIENTS_AND_WATER;
        }
        else if (currentCookingOrbState == CookingOrbState.INGREDIENTS_AND_WATER || currentCookingOrbState == CookingOrbState.EMPTY_WATER)
        {
            Debug.Log("Tried to add water to cooking orb but there is already water!");
        }
        Debug.Log(currentCookingOrbState);

        // ----------------------------------------------------------------------------------------------------------------- //
    }

    private void MoveWaterToCenter()
    {
        water.position = Vector3.Lerp(water.position, soupOrb.position, waterCenteringSpeed);
        if (water.position == soupOrb.position)
        {
            isCentered = true;
        }
    }

    private void RemoveWater()
    {
        GameObject.Destroy(water.gameObject);
        water = null;
        isCentered = false;
    }
}
