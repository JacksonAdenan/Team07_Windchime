using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public class Slicer
{
    public Slicer()
    { }

    public SkinnedMeshRenderer cutterSkinnedMesh;
    private Mesh cutterMesh;

    private int cutterBlendShapeCount;

    public Transform cutterGauge1;
    public Transform cutterGauge2;

    public Transform entryTrigger;
    public Transform exitTrigger;

    public float cutterEjectionSpeed;


    public void SlicerStart()
    {
        cutterMesh = cutterSkinnedMesh.sharedMesh;
    }
    public void SlicerUpdate()
    {
        cutterSkinnedMesh.SetBlendShapeWeight(0, cutterGauge1.GetComponent<Gauge>().currentAmount);
        cutterSkinnedMesh.SetBlendShapeWeight(1, cutterGauge2.GetComponent<Gauge>().currentAmount);
    }
    public void CutHalf(Transform victim)
    {

        if (victim.GetComponent<Ingredient>().currentState == IngredientState.WHOLE)
        {
            Debug.Log("Cut ingredient.");

            Vector3 victimScale = victim.localScale;
            Vector3 leftPoint = victim.position - Vector3.right * victimScale.x / 2;
            Vector3 rightPoint = victim.position + Vector3.right * victimScale.x / 2;
            victim.gameObject.SetActive(false);

            // Spawning the right object. //
            Transform rightSideObj = GameObject.Instantiate(victim, exitTrigger.position, victim.rotation); //primitivetype determines the shape after cutting
            rightSideObj.gameObject.SetActive(true);
            rightSideObj.transform.localScale = victim.localScale / 2;

            // Spawning the left object. //
            Transform leftSideObj = GameObject.Instantiate(victim, exitTrigger.position, victim.rotation); //primitivetype determines the shape after cutting
            leftSideObj.gameObject.SetActive(true);
            leftSideObj.transform.localScale = victim.localScale / 2;


            // Setting them to be halved. //
            rightSideObj.GetComponent<Ingredient>().currentState = IngredientState.HALF;
            leftSideObj.GetComponent<Ingredient>().currentState = IngredientState.HALF;


            Debug.Log(leftSideObj.GetComponent<Ingredient>().currentState);

            Debug.Log("Cut peices have been set to half.");



            // Shooting the new peices upwards. //

            leftSideObj.GetComponent<Rigidbody>().velocity = Vector3.zero;
            leftSideObj.GetComponent<Rigidbody>().AddForce(Vector3.up * cutterEjectionSpeed);

            rightSideObj.GetComponent<Rigidbody>().velocity = Vector3.zero;
            rightSideObj.GetComponent<Rigidbody>().AddForce(Vector3.up * cutterEjectionSpeed);
        }
        else
        {
            Debug.Log("Could not cut this ingredient. It is already cut.");
        }
    }

    public void CutterSwitch1()
    {
        Debug.Log("Cutter switch 1 activated.");
        cutterGauge1.GetComponent<Gauge>().Increase();
    }
    public void CutterSwitch2()
    {
        Debug.Log("Cutter switch 2 activated.");
        cutterGauge2.GetComponent<Gauge>().Increase();
    }
}
