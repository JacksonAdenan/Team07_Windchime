﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CanonEntry : MonoBehaviour
{
    GameManager gameManager;

    // Start is called before the first frame update
    void Start()
    {
        gameManager = GameManager.GetInstance();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnTriggerEnter(Collider obj)
    {
        Transform parentPrefab = obj.transform;
        // Because the item we are holding is actually the parent prefab and the obj the trigger detected is just the mesh, we need to get the parentPrefab manually. 
        if (obj.transform.parent != null)
        { 
            parentPrefab = obj.transform.parent;
        }

        if (parentPrefab.tag == "Capsule" && MouseLook.heldItem != parentPrefab.gameObject.transform && !gameManager.cookingManager.theCanon.isLoaded)
        {
            gameManager.cookingManager.theCanon.LoadCanon(parentPrefab.GetComponent<SoupData>().theSoup);
            Destroy(parentPrefab.gameObject);
        }
    }
}
