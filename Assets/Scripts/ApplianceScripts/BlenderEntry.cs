using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlenderEntry : MonoBehaviour
{
    GameManager gameManager;

    // Start is called before the first frame update
    void Start()
    {
        gameManager = GameManager.GetInstance();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnTriggerEnter(Collider obj)
    {
        if (obj.tag == "Ingredient")
        {
            gameManager.cookingManager.theBlender.AddIngredientToBlender(obj.transform);
        }
    }

    void OnTriggerExit(Collider obj)
    {
        if (obj.tag == "Ingredient")
        {
            gameManager.cookingManager.theBlender.RemoveIngredientFromBlender(obj.transform);
        }
    }
}
