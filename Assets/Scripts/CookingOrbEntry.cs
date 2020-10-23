using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CookingOrbEntry : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnTriggerEnter(Collider obj)
    {
        if (obj.tag == "Ingredient")
        {
            CookingManager.AddIngredient(obj.GetComponent<IngredientData>().ingredientName);
        }
        else if (obj.tag == "Water")
        { 
            
        }
    }
}
