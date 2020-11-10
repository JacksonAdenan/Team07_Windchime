using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCollision : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        // Player layer ignores Ingredient layer.
        Physics.IgnoreLayerCollision(9, 8);
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    // Ignoring collisions.
    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Ingredient")
        { 
            Debug.Log("PLAYER COLLIDED WITH INGREDIENT");
        }
    }



    void OnCollisionExit(Collision collision)
    {
        Debug.Log("PLAYER UNCOLLIDED WITH INGREDIENT");
    }
}
