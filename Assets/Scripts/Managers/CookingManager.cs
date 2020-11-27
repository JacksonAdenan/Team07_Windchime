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
    public Canon theCanon;
    public CapsuleVendor theVendor;
    // ------------------------------------------------------------------------------------------------------------------------------------------------------ //

    [Header("Monitors")]
    public MonitorScreen itemFabricator;


    [Header("Other")]
    
    // Item fabricator stuff. //
    public Transform adjustableItemSpawnPoint;
    public static Transform itemSpawnPoint;


    // Water tap stuff. //

    public Transform adjustableWater;
    public static Transform water;
    public Transform waterTap;

    public static WaterTapState currentWaterTapState;

    // Ingredient spawner stuff. //
    static float ingredientSpawnTimer;
    static bool isSpawningIngredient;
    public float ingredientSpawnerLength;

    static Ingredient cachedIngredientToSpawn = null;


    // Blended Ingredient //
    public Transform blendedIngredientPrefab;


    // Start is called before the first frame update
    void Start()
    {

        // ------------------ Appliance Start Functions ------------------ //
        theSlicer.SlicerStart();
        theBlender.BlenderStart();
        theOrb.Start();
        theCatcher.Start();
        theCanon.Start();
        theVendor.Start();
        // --------------------------------------------------------------- //


        playerCamera = adjustablePlayerCamera;

        // Setting up item fabricator static values. //
        itemSpawnPoint = adjustableItemSpawnPoint;

        // Setting static value for water from inspector value. //
        water = adjustableWater;

        // Initialising things for the water tap. //
        currentWaterTapState = WaterTapState.EMPTY;

      

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
        theCanon.Update();
        theVendor.Update();
        // ---------------------------------------------------------------- //
      
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
            SoundManager.PlaySound(newWater.GetComponent<AudioSource>());
        }
        else
        {
            Debug.Log("Could not activate water tap switch. There is already a water on the tap.");
        }
    }

    public static void SpawnIngredient()
    {      
        Transform newIngredient = Instantiate(cachedIngredientToSpawn.prefab, itemSpawnPoint.position, itemSpawnPoint.rotation);
    }

    public void IngredientSpawnTimer()
    {
        //cachedIngredientToSpawn = playerCamera.GetComponent<MouseLook>().selectedSwitch.GetComponent<Ingredient>();
        if (itemFabricator.currentIngredientDisplay != null)
        {
            cachedIngredientToSpawn = itemFabricator.currentIngredientDisplay;
            isSpawningIngredient = true;
        }
        else
        {
            Debug.Log("IngredientSpawnTiimer() could not find ingredient.");
        }
    }

}
