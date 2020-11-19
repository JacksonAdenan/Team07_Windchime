using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CutterEntry : MonoBehaviour
{
    GameManager gameManager;
    CookingManager theCookingManager;

    // Start is called before the first frame update
    void Start()
    {
        gameManager = GameManager.GetInstance();
        theCookingManager = gameManager.cookingManager;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnTriggerEnter(Collider otherObj)
    {

        Transform realObj = otherObj.transform;
        while (realObj.parent != null)
        {
            realObj = realObj.parent;
        }
        if (realObj.tag == "Ingredient" && realObj.transform != MouseLook.heldItem)
        {
            theCookingManager.theSlicer.Slice(realObj.transform);
            Debug.Log("Cutter entry trigger activated.");
        }
    }
}
