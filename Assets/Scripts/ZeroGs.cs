using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZeroGs : MonoBehaviour
{
    const float G = 667.4f;

    public static List<ZeroGs> zeroGs;

    public Rigidbody rb;



    void FixedUpdate()
    {
        foreach (ZeroGs zeroGs in zeroGs)
        {
            if (zeroGs != this)
                ZeroG(zeroGs);
        }
    }

    void OnEnable()
    {
        if (zeroGs == null)
            zeroGs = new List<ZeroGs>();

        zeroGs.Add(this);
    }

    void OnDisable()
    {
        zeroGs.Remove(this);
    }

    void ZeroG(ZeroGs objToFloat)
    {
        Rigidbody rbToFloat = objToFloat.rb;

        Vector3 direction = rb.position - rbToFloat.position;
        float distance = direction.magnitude;

        if (distance == 0f)
            return;

        float forceMagnitude = (G * (rb.mass * rbToFloat.mass) / Mathf.Pow(distance, 2)) * 0;
        Vector3 force = direction.normalized * forceMagnitude;

        rbToFloat.AddForce(force);
    }

}
