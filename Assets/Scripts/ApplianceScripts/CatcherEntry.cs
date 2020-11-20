using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CatcherEntry : MonoBehaviour
{
    GameManager gameManager;
    SoupCatcher theCatcher;

    // Start is called before the first frame update
    void Start()
    {
        gameManager = GameManager.GetInstance();
        theCatcher = gameManager.cookingManager.theCatcher;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnTriggerEnter(Collider obj)
    {
        if (obj.tag == "SoupPortion" && theCatcher.currentPortions.Count < 5 && theCatcher.currentCatcherState != CatcherState.EMPTY)
        {
            // These two if statements will stop different soups from mixing into the one capsule.
            if (theCatcher.currentPortions.Count >= 1 && obj.GetComponent<SoupData>().theSoup.colour.name == theCatcher.currentPortions[0].colour.name)
            {
                theCatcher.CatchSoup(obj.transform);
            }
            else if (theCatcher.currentPortions.Count == 0)
            {
                theCatcher.CatchSoup(obj.transform);
            }        
        }
        
    }
}
