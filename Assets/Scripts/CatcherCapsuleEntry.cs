using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CatcherCapsuleEntry : MonoBehaviour
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
        // Because the item we are holding is actually the parent prefab and the obj the trigger detected is just the mesh, we need to get the parentPrefab manually. 
        Transform parentPrefab = obj.transform.parent;

        if (MouseLook.heldItem != parentPrefab && obj.tag == "Capsule" && !theCatcher.hasCapsule)
        {
            theCatcher.AttachCapsule();
            Destroy(parentPrefab.gameObject);
        }
    }
}
