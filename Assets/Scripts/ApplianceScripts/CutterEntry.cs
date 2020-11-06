using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CutterEntry : MonoBehaviour
{
    public CookingManager theCookingManager;
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
        if (otherObj.tag == "Ingredient")
        {
            theCookingManager.theSlicer.Slice(otherObj.transform);
            Debug.Log("Cutter entry trigger activated.");
        }
    }
}
