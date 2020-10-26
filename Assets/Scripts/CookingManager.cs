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

public enum BlenderState
{ 
    NOT_COVERED,
    COVERED
}
public class CookingManager : MonoBehaviour
{
    public static List<Ingredient> allIngridients;
    

    public GameObject allSoupsObject;

    

    public static float currentSpicy;
    public static float currentChunky;
    public static Colour currentColour;

    public static Transform occupyingSoup;

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

    // Cookingorb stats and current things. //
    public static CookingOrbState currentCookingOrbState;
    public static List<Transform> currentIngredients;

    public Transform adjustableSoup;
    public static Transform soupOrb;


    // Triggers and stats for blender appliance. //
    public static BlenderState currentBlenderState;
    public static List<Transform> currentBlenderIngredients;

    public static Transform blenderEntryTrigger;
    public static Transform blenderSpawnPoint;

    public Transform adjustableBlenderEntryTrigger;
    public Transform adjustableBlenderSpawnPoint;

    // This transform is just used to show if the blender is covered or not. //
    public static Transform blenderCover;
    public Transform adjustableBlenderCover;


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

        emptyAttachedCapsule = adjustableEmptyAttachedCapsule;
        filledAttachedCapsule = adjustableFilledAttachedCapsule;

        hasCapsule = true;

        // Initialising canon things. //
        currentCanonState = CanonState.EMPTY;
        isLoaded = false;

        // Initialising blender things. //
        currentBlenderIngredients = new List<Transform>();
        currentBlenderState = BlenderState.COVERED;
        blenderCover = adjustableBlenderCover;
        
        blenderEntryTrigger = adjustableBlenderEntryTrigger;
        blenderSpawnPoint = adjustableBlenderSpawnPoint;



        // Giving the capsules soup data components because i'm gonna use them to store information. //
        if (adjustableCanonCapsule.GetComponent<SoupData>() == null)
        { 
            adjustableCanonCapsule.gameObject.AddComponent<SoupData>();
        }
        canonCapsule = adjustableCanonCapsule;
    }
    
    // Update is called once per frame
    void Update()
    {

        // Cooking orb updates. //
        UpdateCookingOrbState();

        // Catcher updates //
        UpdateCatcherState();
        UpdateCatcherCapsule();

        // Canon updates //
        UpdateCanonState();
        UpdateCanonCapsule();

        // Blender updates // 
        UpdateBlenderCover();
    }


    void UpdateBlenderCover()
    {
        switch (currentBlenderState)
        {
            case BlenderState.COVERED:
                blenderCover.gameObject.SetActive(true);
                break;
            case BlenderState.NOT_COVERED:
                blenderCover.gameObject.SetActive(false);
                break;
        }
    }
    void UpdateCanonCapsule()
    {
        switch (currentCanonState)
        {
            case CanonState.EMPTY:
                canonCapsule.gameObject.SetActive(false);
                break;
            case CanonState.LOADED:
                canonCapsule.gameObject.SetActive(true);
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
    //Soup CreateSoup(Transform soupFromScene)
    //{
    //    SoupCreator soupsData = soupFromScene.GetComponent<SoupCreator>();
    //    Ingredient restrictedIngredient = ConvertTextToIngredient(soupsData.restrictedIngredient);
    //    float spicyValue = soupsData.spicyValue;
    //    float chunkyValue = soupsData.chunkyValue;
    //
    //    Soup newSoup = new Soup(spicyValue, chunkyValue);
    //    return newSoup;
    //    
    //}

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
        allIngridients.Add(new Ingredient("apple", 10, 25, false));
        allIngridients.Add(new Ingredient("banana", 2, 5, false));
        allIngridients.Add(new Ingredient("steak", 8, 14, true));

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

    public static void AddIngredientToBlender(Transform ingredientToCatch)
    {
        currentBlenderIngredients.Add(ingredientToCatch);
        Debug.Log("Ingredient added to blender.");
    }
    public static void RemoveIngredientFromBlender(Transform ingredientToRemove)
    {
        currentBlenderIngredients.Remove(ingredientToRemove);
        Debug.Log("Ingredient removed from blender.");
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

        for (int i = currentIngredients.Count - 1; i > -1; i--)
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

    public static void AttachBlenderCover()
    {
        currentBlenderState = BlenderState.COVERED;
    }
    public static void RemoveBlenderCover()
    {
        currentBlenderState = BlenderState.NOT_COVERED;
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
    public static void PopBlenderCover()
    {
        Transform poppedBlenderCover = Instantiate(blenderCover, blenderCover.position, blenderCover.rotation);
        poppedBlenderCover.tag = "InteractableBlenderCover";

        // Not only do we set the parent prefab to have a capsule tag, but also the children it has. // 
        for (int i = 0; i < poppedBlenderCover.childCount; i++)
        {
            poppedBlenderCover.GetChild(0).tag = "InteractableBlenderCover";
        }
       
       

        // TEMPORARY FIX ME //
        // Because the mesh on the blender isn't working properly, we can't put a mesh collider on it. This means we have to use a box collider and set it to be a trigger so that ingredients can
        // be inside of it.

        poppedBlenderCover.GetComponent<BoxCollider>().isTrigger = false;

        // Adding upwards force to "pop" the cover //
        poppedBlenderCover.GetComponent<Rigidbody>().isKinematic = false;
        poppedBlenderCover.GetComponent<Rigidbody>().AddForce(Vector3.up * 50);
    }
    public static void ActivateBlender()
    {
        if (currentBlenderState == BlenderState.COVERED)
        {
            Debug.Log("Blender activated");
            for (int i = currentBlenderIngredients.Count - 1; i > -1; i--)
            {
                // Spawn a blended thingy in its place. //
                SpawnBlendedIngredient(currentBlenderIngredients[i]);


                currentBlenderIngredients[i].gameObject.SetActive(false);
                currentBlenderIngredients.Remove(currentBlenderIngredients[i]);

            }


            RemoveBlenderCover();
            PopBlenderCover();
        }
        else
        {
            Debug.Log("Blender could not be activated. Please put the cover on.");
        }
    }

    public static void SpawnBlendedIngredient(Transform oldIngredient)
    {
        IngredientData dataToTransfer = oldIngredient.GetComponent<IngredientData>();
        Transform newBlendedThing = Object.Instantiate(soupOrb, blenderSpawnPoint.position, blenderSpawnPoint.rotation);

        // Incase the soupOrb we are copying isnt active. //
        newBlendedThing.gameObject.SetActive(true);

        newBlendedThing.localScale /= 2;

        newBlendedThing.position = blenderSpawnPoint.position;

        // Just because we don't have any art for blended foods, ill make the soup orb a blended ingredient by changing the tag and adding a IngredientData script. //
        newBlendedThing.gameObject.AddComponent<IngredientData>();
        newBlendedThing.gameObject.GetComponent<IngredientData>().ingredientName = dataToTransfer.ingredientName;
        newBlendedThing.tag = "Ingredient";
        newBlendedThing.GetComponent<Rigidbody>().isKinematic = false;

        // Because its instantiating the soup thing we have to remove its soup data script. //
        Destroy(newBlendedThing.GetComponent<SoupData>());

    }

}
