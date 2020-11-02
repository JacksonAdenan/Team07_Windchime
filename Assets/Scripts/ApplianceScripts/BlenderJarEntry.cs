using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlenderJarEntry : MonoBehaviour
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
        if (obj.tag == "InteractableBlenderCover" && MouseLook.heldItem != obj.gameObject.transform)
        {
            gameManager.cookingManager.theBlender.AttachBlenderCover();
            Destroy(obj.gameObject);
        }
    }
}
