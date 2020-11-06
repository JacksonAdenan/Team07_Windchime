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
    OCCUPIED_SOUP,
    COOKING
}

public enum WaterTapState
{ 
    EMPTY,
    OCCUPIED
}

public enum CatcherState
{ 
    EMPTY,
    EMPTY_CAPSULE,
    FILLED_1,
    FILLED_2,
    FILLED_3,
    FILLED_4,
    FULL_CAPSULE
}

public enum CanonState
{ 
    EMPTY,
    LOADED
}


public class CookingManager : MonoBehaviour
{
    public Transform adjustablePlayerCamera;
    public static Transform playerCamera;

    // ---------------------------------------------------------------- APPLIANCES ---------------------------------------------------------------- //
    public CookingOrb theOrb;
    public Slicer theSlicer;
    public Blender theBlender;
    // -------------------------------------------------------------------------------------------------------------------------------------------- //

    // Appliance prefabs //
    public Transform canon;
    

    // Canon stats and current things. //
    public static CanonState currentCanonState;
    //public static Soup loadedCapsule;
    public static bool isLoaded;


    // These transforms of capsules are just like a thing to show if the canon is loaded or not. They aren't really a part of the game. //
    public Transform adjustableCanonCapsule;
    public static Transform canonCapsule;


    // Soup catcher stats and current things. //
    public static List<Soup> currentPortions;
    public static CatcherState currentCatcherState;

    public Transform adjustableEmptyAttachedCapsule;
    public Transform adjustableFilledAttachedCapsule;

    public static Transform emptyAttachedCapsule;
    public static Transform filledAttachedCapsule;

    public static bool hasCapsule;
 

    // Item fabricator stuff. //
    public Transform adjustableItemSpawnPoint;
    public static Transform itemSpawnPoint;


    // Water tap stuff. //

    public Transform adjustableWater;
    public static Transform water;

    public static WaterTapState currentWaterTapState;

    // Ingredient spawner stuff. //
    static float ingredientSpawnTimer;
    static bool isSpawningIngredient;
    public float ingredientSpawnerLength;

    static Ingredient cachedIngredientToSpawn = null;


    // Start is called before the first frame update
    void Start()
    {

        playerCamera = adjustablePlayerCamera;

        // Setting up item fabricator static values. //
        itemSpawnPoint = adjustableItemSpawnPoint;

        // Setting static value for water from inspector value. //
        water = adjustableWater;

        // Initialising things for the water tap. //
        currentWaterTapState = WaterTapState.EMPTY;

        // Initialising catcher things. //
        currentCatcherState = CatcherState.EMPTY;
        currentPortions = new List<Soup>();

        emptyAttachedCapsule = adjustableEmptyAttachedCapsule;
        filledAttachedCapsule = adjustableFilledAttachedCapsule;

        hasCapsule = true;

        // Initialising canon things. //
        currentCanonState = CanonState.EMPTY;
        isLoaded = false;

        // Blender start //
        theBlender.BlenderStart();

        // CookingOrb start. //
        theOrb.Start();


        
       
        // Giving the capsules soup data components because i'm gonna use them to store information. //
        if (adjustableCanonCapsule.GetComponent<SoupData>() == null)
        { 
            adjustableCanonCapsule.gameObject.AddComponent<SoupData>();
        }
        canonCapsule = adjustableCanonCapsule;


        // Slicer Start. //
        theSlicer.SlicerStart();


        // Ingredient spawner stuff. //
        ingredientSpawnTimer = 0;
        isSpawningIngredient = false;
        
    }
    
    // Update is called once per frame
    void Update()
    {

        

        // Catcher updates //
        UpdateCatcherState();
        UpdateCatcherCapsule();

        // Canon updates //
        UpdateCanonState();
        UpdateCanonCapsule();

        


        

        
        // Slicer Updates. //
        theSlicer.SlicerUpdate();

        // Blender Updates. //
        theBlender.BlenderUpdate();

        // CookingOrb Updates. //
        theOrb.Update();

        // Ingredient spawner update. //
        if (isSpawningIngredient == true)
        {
            ingredientSpawnTimer += Time.deltaTime;
            if (ingredientSpawnTimer >= ingredientSpawnerLength)
            {
                ingredientSpawnTimer = 0;
                isSpawningIngredient = false;
                SpawnIngredient();
                cachedIngredientToSpawn = null;
            }
        }

    }


   
    void UpdateCanonCapsule()
    {
        switch (currentCanonState)
        {
            case CanonState.EMPTY:
                canonCapsule.gameObject.SetActive(false);
                canon.GetComponent<Animator>().SetBool("IsOpen", true);

                break;
            case CanonState.LOADED:
                canonCapsule.gameObject.SetActive(true);
                canon.GetComponent<Animator>().SetBool("IsOpen", false);
                break;
        }
    }
    void UpdateCanonState()
    {
        if (!isLoaded)
        {
            currentCanonState = CanonState.EMPTY;
        }
        else
        {
            currentCanonState = CanonState.LOADED;
        }
    }
    void UpdateCatcherState()
    {
        if (!hasCapsule)
        {
            currentPortions.Clear();
            currentCatcherState = CatcherState.EMPTY;
        }
        else if (currentPortions.Count == 0)
        {
            currentCatcherState = CatcherState.EMPTY_CAPSULE;
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
            currentCatcherState = CatcherState.FULL_CAPSULE;
        }
    }

    void UpdateCatcherCapsule()
    {
        if (currentCatcherState == CatcherState.EMPTY)
        {
            emptyAttachedCapsule.gameObject.SetActive(false);
            filledAttachedCapsule.gameObject.SetActive(false);
        }
        else if ((int)currentCatcherState >= 1 && (int)currentCatcherState < 5)
        {
            emptyAttachedCapsule.gameObject.SetActive(true);
            filledAttachedCapsule.gameObject.SetActive(false);
        }
        else if (currentCatcherState == CatcherState.FULL_CAPSULE)
        {
            emptyAttachedCapsule.gameObject.SetActive(false);
            filledAttachedCapsule.gameObject.SetActive(true);
        }

    }
    
    

    /// <summary>
    /// CreateSoup() grabs a child gameObject from the AllSoups gameObject in the scene. It then uses data stored in the gameObjects SoupCreator script to make a Soup instance.
    /// </summary>
    /// <param name="soupFromScene"></param>


    // --------------------------------------- PREVIOUS WAY OF HANDLING INGREDIENTS --------------------------------------- //

    //public static Ingredient ConvertTextToIngredient(string textToConvert)
    //{
    //    for (int i = 0; i < allIngridients.Count; i++)
    //    {
    //        if (allIngridients[i].name == textToConvert)
    //        {
    //            return allIngridients[i]; 
    //        }
    //    }
    //    return null;
    //}
    //
    //public void CopyOverCreatedIngredients()
    //{
    //    allIngridients = adjustableAllIngredients;
    //}

    // -------------------------------------------------------------------------------------------------------------------- //

    

    public static void CatchSoup(Transform soupToCatch)
    {
        currentPortions.Add(soupToCatch.GetComponent<SoupData>().theSoup);
        soupToCatch.gameObject.SetActive(false);
        Debug.Log("Caught a portion of soup.");
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

    

    
    public static void AttachCapsule()
    {
        hasCapsule = true;
    }
    public static void RemoveCapsule()
    {
        hasCapsule = false;
        Debug.Log("hasCapsule is now FALSE");
    }

    public static void LoadCanon(Soup theDataToLoad)
    {

        isLoaded = true;


        // Actual loading of soup data. //
        canonCapsule.GetComponent<SoupData>().theSoup = theDataToLoad;
        Debug.Log("Canon loaded and received data successfully.");
    }
    public static void UnloadCanon()
    {
        canonCapsule.GetComponent<SoupData>().theSoup = null;
        isLoaded = false;
        Debug.Log("Canon unloaded and removed soup data.");
    }

    public static void ShootCapsule()
    {
        if (OrderManager.acceptedOrders.Count > 0)
        {
            OrderManager.CompleteOrder(canonCapsule.GetComponent<SoupData>().theSoup);
            canonCapsule.GetComponent<SoupData>().theSoup = null;
            isLoaded = false;
            Debug.Log("Canon submitted order and removed soup data.");

            // Display points
            MenuManager.DisplayOrderSubmittedText();
        }
        else
        {
            Debug.Log("Tried to submit soup but you do not currently have any orders.");
        }

    }
    public static void SpawnIngredient()
    {      
        Instantiate(cachedIngredientToSpawn.prefab, itemSpawnPoint.position, itemSpawnPoint.rotation);
    }

    public static void IngredientSpawnTimer()
    {
        cachedIngredientToSpawn = playerCamera.GetComponent<MouseLook>().selectedSwitch.GetComponent<Ingredient>();
        isSpawningIngredient = true;
    }

}
