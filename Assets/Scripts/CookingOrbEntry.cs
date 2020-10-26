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
            CookingManager.TrackIngredient(obj.transform);
        }
    }
    void OnTriggerExit(Collider obj)
    {
        if (obj.tag == "Ingredient")
        {
            if (CookingManager.currentCookingOrbState == CookingOrbState.EMPTY || CookingManager.currentCookingOrbState == CookingOrbState.INGREDIENTS_NOWATER)
            {
                CookingManager.StopTrackingIngredient(obj.transform);
            }
            else if (CookingManager.currentCookingOrbState == CookingOrbState.EMPTY_WATER || CookingManager.currentCookingOrbState == CookingOrbState.INGREDIENTS_AND_WATER)
            {
                CookingManager.RemoveIngredient(obj.transform);
            }
        }
    }
    
}
