using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DownwardsForce : MonoBehaviour
{
    public float increment;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        Transform realObj = other.transform;
        while(realObj.parent != null)
        {
            realObj = realObj.parent;
        }
        if (realObj.gameObject.tag == "Ingredient")
        {
            //other.gameObject.GetComponent<Rigidbody>().useGravity = true;
            realObj.gameObject.GetComponent<Rigidbody>().AddForce(-transform.up * increment);
        }

    }
}
