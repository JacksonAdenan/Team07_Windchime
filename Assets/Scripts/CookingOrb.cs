using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CookingOrb : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnTriggerEnter(Collider otherObj)
    {
        if (otherObj.transform.tag == "Ingredient")
        {
            CookingManager.AddIngredient(otherObj.GetComponent<IngredientData>().name);
        }
    }

    void OnTriggerExit(Collider otherObj)
    {
        if (otherObj.transform.tag == "Ingredient")
        { 
            CookingManager.RemoveIngredient(otherObj.GetComponent<IngredientData>().name)
        }
    }
}
