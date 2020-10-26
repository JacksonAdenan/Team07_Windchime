using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlenderEntry : MonoBehaviour
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
            CookingManager.AddIngredientToBlender(obj.transform);
        }
    }

    void OnTriggerExit(Collider obj)
    {
        if (obj.tag == "Ingredient")
        {
            CookingManager.RemoveIngredientFromBlender(obj.transform);
        }
    }
}
