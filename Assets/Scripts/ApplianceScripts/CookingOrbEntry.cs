﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CookingOrbEntry : MonoBehaviour
{
    GameManager gameManager;
    CookingOrb orb;
    // Start is called before the first frame update
    void Start()
    {
        gameManager = GameManager.GetInstance();
        orb = gameManager.cookingManager.theOrb;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnTriggerEnter(Collider obj)
    {
        if (obj.tag == "Ingredient")
        {
            gameManager.cookingManager.theOrb.TrackIngredient(obj.transform);
        }
        if (obj.tag == "Water" && obj.transform != MouseLook.heldItem && (orb.currentCookingOrbState != CookingOrbState.EMPTY_WATER && orb.currentCookingOrbState != CookingOrbState.INGREDIENTS_AND_WATER))
        {
            gameManager.cookingManager.theOrb.AddWater(obj.transform);
            Destroy(obj.gameObject);
        }
    }
    void OnTriggerExit(Collider obj)
    {
        if (obj.tag == "Ingredient")
        {
            if (gameManager.cookingManager.theOrb.currentCookingOrbState == CookingOrbState.EMPTY || gameManager.cookingManager.theOrb.currentCookingOrbState == CookingOrbState.INGREDIENTS_NOWATER)
            {
                gameManager.cookingManager.theOrb.StopTrackingIngredient(obj.transform);
            }
            else if (gameManager.cookingManager.theOrb.currentCookingOrbState == CookingOrbState.EMPTY_WATER || gameManager.cookingManager.theOrb.currentCookingOrbState == CookingOrbState.INGREDIENTS_AND_WATER)
            {
                gameManager.cookingManager.theOrb.RemoveIngredient(obj.transform);
            }
        }
    }
    
}
