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



public enum CanonState
{ 
    EMPTY,
    LOADED
}


public class CookingManager : MonoBehaviour
{
    public Transform adjustablePlayerCamera;
    public static Transform playerCamera;

    // ---------------------------------------------------------------- Appliance Objects -------------------------------------------------------------------- //
    // ====================================== QUESTION ============================ WHY DON'T I NEED TO INITIALISE THESE AND IT WORKS? 
    [Header("Appliance Objects")]
    public CookingOrb theOrb;
    public Slicer theSlicer;
    public Blender theBlender;
    public SoupCatcher theCatcher;
    // ------------------------------------------------------------------------------------------------------------------------------------------------------ //

    [Header("Other")]
    public Transform canon;
    // Canon stats and current things. //
    public static CanonState currentCanonState;
    //public static Soup loadedCapsule;
    public static bool isLoaded;


    // These transforms of capsules are just like a thing to show if the canon is loaded or not. They aren't really a part of the game. //
    public Transform adjustableCanonCapsule;
    public static Transform canonCapsule;


    
 

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

        // ------------------ Appliance Start Functions ------------------ //
        theSlicer.SlicerStart();
        theBlender.BlenderStart();
        theOrb.Start();
        theCatcher.Start();
        // --------------------------------------------------------------- //


        playerCamera = adjustablePlayerCamera;

        // Setting up item fabricator static values. //
        itemSpawnPoint = adjustableItemSpawnPoint;

        // Setting static value for water from inspector value. //
        water = adjustableWater;

        // Initialising things for the water tap. //
        currentWaterTapState = WaterTapState.EMPTY;

       
        // Initialising canon things. //
        currentCanonState = CanonState.EMPTY;
        isLoaded = false;

        // Giving the capsules soup data components because i'm gonna use them to store information. //
        if (adjustableCanonCapsule.GetComponent<SoupData>() == null)
        { 
            adjustableCanonCapsule.gameObject.AddComponent<SoupData>();
        }
        canonCapsule = adjustableCanonCapsule;

        // Ingredient spawner stuff. //
        ingredientSpawnTimer = 0;
        isSpawningIngredient = false;
        
    }
    
    // Update is called once per frame
    void Update()
    {
        // ------------------ Appliance Update Functions ------------------ //
        theSlicer.SlicerUpdate();
        theBlender.BlenderUpdate();
        theOrb.Update();
        theCatcher.Update();
        // ---------------------------------------------------------------- //


        // Canon updates //
        UpdateCanonState();
        UpdateCanonCapsule();

       
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

    /// <summary>
    /// CreateSoup() grabs a child gameObject from the AllSoups gameObject in the scene. It then uses data stored in the gameObjects SoupCreator script to make a Soup instance.
    /// </summary>
    /// <param name="soupFromScene"></param>
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
