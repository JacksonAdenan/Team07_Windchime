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
        Transform realObj = obj.transform;
        while (realObj.parent != null)
        {
            realObj = realObj.parent;
        }


        if (realObj.tag == "Ingredient" && realObj.GetComponent<Ingredient>().currentState != IngredientState.BLENDED)
        {
            gameManager.cookingManager.theBlender.AddIngredientToBlender(realObj.transform);
        }
    }

    void OnTriggerExit(Collider obj)
    {
        Transform realObj = obj.transform;
        while (realObj.parent != null && realObj.parent.tag == "Ingredient")
        {
            realObj = realObj.parent;
        }

        if (realObj.tag == "Ingredient")
        {
            gameManager.cookingManager.theBlender.RemoveIngredientFromBlender(realObj.transform);
        }
    }
}
