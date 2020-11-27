using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public class CapsuleVendor
{

    public Transform capsule;
    public Transform capsuleSpawnLocation;
    [Header("Appliance reference for sound")]
    public Transform vendorBody;


    private bool isSpinning = false;


    public Animator vendorAnimator;
    public void SpawnCapsule()
    {
        Transform newCapsule = GameObject.Instantiate(capsule, capsuleSpawnLocation.position, capsuleSpawnLocation.rotation);
        newCapsule.gameObject.SetActive(true);
        isSpinning = true;
    }
    // Start is called before the first frame update
    public void Start()
    {
        //vendorAnimator = vendorBody.GetComponent<Animator>();
    }

    // Update is called once per frame
    public void Update()
    {
        CompleteAnimation();
        Animate();
    }

    private void CompleteAnimation()
    {
        if (vendorAnimator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1.0f)
        {
            isSpinning = false;
        }
    }


    private void Animate()
    {
        CompleteAnimation();

        if (isSpinning == true)
        {
            vendorAnimator.SetBool("IsSpawned", true);
            isSpinning = false;
        }
        else if (!isSpinning)
        {
            vendorAnimator.SetBool("IsSpawned", false);
        }
        
    }
}
