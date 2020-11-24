using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public class CapsuleVendor
{

    public Transform capsule;
    public Transform capsuleSpawnLocation;
    public void SpawnCapsule()
    {
        Transform newCapsule = GameObject.Instantiate(capsule, capsuleSpawnLocation.position, capsuleSpawnLocation.rotation);
        newCapsule.gameObject.SetActive(true);
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
