using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AddRandomForce : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        RandomForce();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void RandomForce()
    {
        transform.eulerAngles = new Vector3(transform.eulerAngles.x, Random.Range(0, 360), transform.eulerAngles.z);
        float speed = 2f;
        Vector3 force = transform.forward;
        force = new Vector3(force.x, 1, force.z);
        gameObject.GetComponent<Rigidbody>().AddForce(force * speed);
    }
}
