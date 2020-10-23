using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Orginally I intended for CookingManager to not be a MonoBehaviour, but it seems that making it a MonoBehaviour will do nothing but make things easier. Specifically, as of writing, I need a reference to the
/// gameObject allSoups which contains all the soups the designers will make. I can use GameObject.Find() but just having a reference is easier and probably faster. Perhaps I will remove the MonoBehaviour from this class
/// if I ever make a custom .data file which could hold all the premade soups rather than reading from a gameObject in the Unity Editor.
/// </summary>
public class CookingManager : MonoBehaviour
{
    public static List<Ingredient> allIngridients;
    public static List<Soup> allSoups;

    public GameObject allSoupsObject;

    

    float currentSpicy;
    float currentChunky;
    Colour currentColour;

    public static List<Ingredient> currentIngredients;

    // Start is called before the first frame update
    void Start()
    {
        allIngridients = new List<Ingredient>();
        allSoups = new List<Soup>();

        currentIngredients = new List<Ingredient>();

        // Creating all basic ingridients //
        CreateBasicIngridients();

        // Reading in and creating all the soups //
        PopulateSoupList();

        // Printing out names of all soups.
        DisplaySoups();
    }

    // Update is called once per frame
    void Update()
    {
        
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

        Soup newSoup = new Soup(soupsData.soupName, spicyValue, chunkyValue);
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

    void PopulateSoupList()
    {
        for (int i = 0; i < allSoupsObject.transform.childCount; i++)
        {
            allSoups.Add(CreateSoup(allSoupsObject.transform.GetChild(i)));
        }
    }

    void DisplaySoups()
    {
        for (int i = 0; i < allSoups.Count; i++)
        {
            Debug.Log(allSoups[i].soupName);
        }
    }

    public static void AddIngredient(string ingredientName)
    {
        currentIngredients.Add(ConvertTextToIngredient(ingredientName));
    }

    public static void RemoveIngredient(string ingredientName)
    {
        currentIngredients.Remove(ConvertTextToIngredient(ingredientName));
    }

    void CombineIngredient(Ingredient ingredient)
    {
        currentSpicy += ingredient.spicyness;
        currentChunky += ingredient.chunkyness;
            
    }

    //Soup Cook()
    //{
    //    bool finishedCook = false;
    //    while (!finishedCook)
    //    {
    //        for (int i = 0; i < currentIngredients.Count; i++)
    //        {
    //            CombineIngredient(currentIngredients[i]);
    //        }
    //    }
    //}
}
