using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CutterEntry : MonoBehaviour
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
        if (otherObj.tag == "Ingredient")
        {
            CookingManager.Cut(otherObj.transform, transform.position, otherObj.transform);
            CubeCut.Cut(otherObj.transform, transform.position);
            Debug.Log("Cutter entry trigger activated.");
        }
    }
}
