using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Orginally I intended for CookingManager to not be a MonoBehaviour, but it seems that making it a MonoBehaviour will do nothing but make things easier. Specifically, as of writing, I need a reference to the
/// gameObject allSoups which contains all the soups the designers will make. I can use GameObject.Find() but just having a reference is easier and probably faster. Perhaps I will remove the MonoBehaviour from this class
/// if I ever make a custom .data file which could hold all the premade soups rather than reading from a gameObject in the Unity Editor.
/// </summary>
/// 

public enum CookingOrbState
{ 
    EMPTY,
    EMPTY_WATER,
    INGREDIENTS_NOWATER,
    INGREDIENTS_AND_WATER,
    OCCUPIED_SOUP
}

public enum WaterTapState
{ 
    EMPTY,
    OCCUPIED
}

public enum CatcherState
{ 
    EMPTY,
    FILLED_1,
    FILLED_2,
    FILLED_3,
    FILLED_4,
    FILLED_5
}
public class CookingManager : MonoBehaviour
{
    public static List<Ingredient> allIngridients;
    

    public GameObject allSoupsObject;

    

    public static float currentSpicy;
    public static float currentChunky;
    public static Colour currentColour;

    public static Transform occupyingSoup;

    // Soup catcher stats and current things. //
    public static List<Soup> currentPortions;
    public static CatcherState currentCatcherState;

    // Cookingorb stats and current things. //
    public static CookingOrbState currentCookingOrbState;
    public static List<Transform> currentIngredients;

    public Transform adjustableSoup;
    public static Transform soupOrb;


    // Triggers and stats for cutter appliance. //
    public static Transform entryTrigger;
    public static Transform exitTrigger;

    public Transform adjustableEntryTrigger;
    public Transform adjustableExitTrigger;

    public float adjustableCutterEjectionSpeed;
    public static float cutterEjectionSpeed;


    // Water tap stuff. //

    public Transform adjustableWater;
    public static Transform water;

    public static WaterTapState currentWaterTapState;

    // Start is called before the first frame update
    void Start()
    {
        allIngridients = new List<Ingredient>();
  

        currentIngredients = new List<Transform>();

        // Creating all basic ingridients //
        CreateBasicIngridients();

        // Reading in and creating all the soups //
        //PopulateSoupList();

        // Printing out names of all soups.
        //DisplaySoups();


        // Setting static values from non static inspector values //
        cutterEjectionSpeed = adjustableCutterEjectionSpeed;
        entryTrigger = adjustableEntryTrigger;
        exitTrigger = adjustableExitTrigger;

        // Setting static value for water from inspector value. //
        water = adjustableWater;

        // Setting static value for soup from inspector value. //
        soupOrb = adjustableSoup;

        // Initialising things for the water tap. //
        currentWaterTapState = WaterTapState.EMPTY;

        // Initialising cooking orb things. //
        currentCookingOrbState = CookingOrbState.EMPTY;
        currentSpicy = 0;
        currentChunky = 0;

        // Initialising catcher things. //
        currentCatcherState = CatcherState.EMPTY;
        currentPortions = new List<Soup>();
    }
    
    // Update is called once per frame
    void Update()
    {
        UpdateCookingOrbState();
        UpdateCatcherState();
    }

    void UpdateCatcherState()
    {
        if (currentPortions.Count == 0)
        {
            currentCatcherState = CatcherState.EMPTY;
        }
        else if (currentPortions.Count == 1)
        {
            currentCatcherState = CatcherState.FILLED_1;
        }
        else if (currentPortions.Count == 2)
        {
            currentCatcherState = CatcherState.FILLED_2;
        }
        else if (currentPortions.Count == 3)
        {
            currentCatcherState = CatcherState.FILLED_3;
        }
        else if (currentPortions.Count == 4)
        {
            currentCatcherState = CatcherState.FILLED_4;
        }
        else if (currentPortions.Count == 5)
        {
            currentCatcherState = CatcherState.FILLED_5;
        }
    }
    void UpdateCookingOrbState()
    {
        if (occupyingSoup != null && occupyingSoup.GetComponent<SoupData>().currentPortions <= 0)
        {
            occupyingSoup.gameObject.SetActive(false);
            occupyingSoup = null;

            // Freeing the cooking orb. //
            currentCookingOrbState = CookingOrbState.EMPTY;
        }

        if (occupyingSoup != null)
        {
            currentCookingOrbState = CookingOrbState.OCCUPIED_SOUP;
        }
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

    /// <summary>
    /// CreateSoup() grabs a child gameObject from the AllSoups gameObject in the scene. It then uses data stored in the gameObjects SoupCreator script to make a Soup instance.
    /// </summary>
    /// <param name="soupFromScene"></param>
    Soup CreateSoup(Transform soupFromScene)
    {
        SoupCreator soupsData = soupFromScene.GetComponent<SoupCreator>();
        Ingredient restrictedIngredient = ConvertTextToIngredient(soupsData.restrictedIngredient);
        float spicyValue = soupsData.spicyValue;
        float chunkyValue = soupsData.chunkyValue;

        Soup newSoup = new Soup(spicyValue, chunkyValue);
        return newSoup;
        
    }

    public static Ingredient ConvertTextToIngredient(string textToConvert)
    {
        for (int i = 0; i < allIngridients.Count; i++)
        {
            if (allIngridients[i].name == textToConvert)
            {
                return allIngridients[i]; 
            }
        }
        return null;
    }

    void CreateBasicIngridients()
    {
        allIngridients.Add(new Ingredient("apple"));
        allIngridients.Add(new Ingredient("banana"));
        allIngridients.Add(new Ingredient("orange"));

    }

    //void PopulateSoupList()
    //{
    //    for (int i = 0; i < allSoupsObject.transform.childCount; i++)
    //    {
    //        allSoups.Add(CreateSoup(allSoupsObject.transform.GetChild(i)));
    //    }
    //}
    //
    //void DisplaySoups()
    //{
    //    for (int i = 0; i < allSoups.Count; i++)
    //    {
    //        Debug.Log(allSoups[i].soupName);
    //    }
    //}

    public static void AddIngredient(Transform ingredient)
    {
        currentIngredients.Add(ingredient);
        Debug.Log("Ingredient added to cooking orb.");
    }

    public static void RemoveIngredient(Transform ingredient)
    {
        currentIngredients.Remove(ingredient);
        Debug.Log("Ingredient removed from cooking orb.");
    }

    public static void CombineIngredient(Ingredient ingredient)
    {
        currentSpicy += ingredient.spicyness;
        currentChunky += ingredient.chunkyness;
            
    }

    public static void CatchSoup(Transform soupToCatch)
    {
        currentPortions.Add(soupToCatch.GetComponent<SoupData>().theSoup);
        soupToCatch.gameObject.SetActive(false);
        Debug.Log("Caught a portion of soup.");
    }

    public static Soup CookSoup()
    {
        //bool finishedCook = false;
 
        for (int i = 0; i < currentIngredients.Count; i++)
        {
            CombineIngredient(CookingManager.ConvertTextToIngredient(currentIngredients[i].GetComponent<IngredientData>().ingredientName));
        }
        
        Soup newSoup = new Soup(currentSpicy, currentChunky);
        for (int i = 0; i < currentIngredients.Count; i++)
        {
            newSoup.usedIngredients.Add(CookingManager.ConvertTextToIngredient(currentIngredients[i].GetComponent<IngredientData>().ingredientName));
        }


        // Resetting current cooking orb values to be ready for next soup
        currentSpicy = 0;
        currentChunky = 0;
        currentCookingOrbState = CookingOrbState.EMPTY;

        for (int i = currentIngredients.Count - 1; i == 0; i--)
        {
            currentIngredients[i].gameObject.SetActive(false);
            currentIngredients.Remove(currentIngredients[i]);
        }
        
        return newSoup;
    }

    public static void MakeSoup()
    {


        // Making the cooking orb state OCCUPIED. //
        currentCookingOrbState = CookingOrbState.OCCUPIED_SOUP;

        Soup newSoup = CookSoup();
        Transform newSoupOrb = Object.Instantiate(soupOrb, soupOrb.position, soupOrb.rotation);
        newSoupOrb.gameObject.SetActive(true);
        SoupData newSoupsData = newSoupOrb.GetComponent<SoupData>();
        newSoupsData.theSoup = newSoup;

        // Setting max amount of "portions" of soup. The player can keep grabbing soup until they've depleted all the portions. //

        // Hey get out of here! If you want to set the portion sizes do it through the inspector ! //

        //newSoupsData.currentPortions = 5;
        //newSoupsData.maxPortions = 5;

        occupyingSoup = newSoupOrb;

        Debug.Log("CREATED SOUP!");
    }

       
    public static void Cut(Transform ingredientObj)
    {
        ingredientObj.position = exitTrigger.position;
        ingredientObj.GetComponent<Rigidbody>().velocity = Vector3.zero;
        ingredientObj.GetComponent<Rigidbody>().AddForce(Vector3.up * cutterEjectionSpeed);
    }

    public static void CutterSwitch1()
    {
        Debug.Log("Cutter switch 1 activated.");
    }
    public static void CutterSwitch2()
    {
        Debug.Log("Cutter switch 2 activated.");
    }
    public static void WaterTapSwitch()
    {
        if (currentWaterTapState == WaterTapState.EMPTY)
        {
            Debug.Log("Water tap switch activated.");
            Transform newWater = Object.Instantiate(water, water.position, water.rotation);
            newWater.gameObject.SetActive(true);
            currentWaterTapState = WaterTapState.OCCUPIED;
        }
        else
        {
            Debug.Log("Could not activate water tap switch. There is already a water on the tap.");
        }
    }

    public static void AddWater()
    {
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
    }
}
