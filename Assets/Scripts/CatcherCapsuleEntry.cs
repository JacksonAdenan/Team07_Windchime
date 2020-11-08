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
        if (obj.tag == "Capsule" && !theCatcher.hasCapsule && obj.transform != MouseLook.heldItem)
        {
            theCatcher.AttachCapsule();
            Destroy(obj.gameObject);
        }
    }
}
