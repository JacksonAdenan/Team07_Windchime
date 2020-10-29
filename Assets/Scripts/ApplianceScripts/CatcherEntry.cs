using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CatcherEntry : MonoBehaviour
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
        if (obj.tag == "SoupPortion" && CookingManager.currentPortions.Count < 5 && CookingManager.currentCatcherState != CatcherState.EMPTY)
        {
            CookingManager.CatchSoup(obj.transform);

        }
    }
}
