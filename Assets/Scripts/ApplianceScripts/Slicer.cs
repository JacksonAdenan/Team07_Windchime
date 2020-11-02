using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public enum SlicerState
{ 
    NO_POWER_MODE,
    EXTREME_POWER_MODE,
    HALFING_MODE,
    QUARTERING_MODE
}
[Serializable]
public class Slicer
{
    public Slicer()
    { }

    public SkinnedMeshRenderer cutterSkinnedMesh;
    private Mesh cutterMesh;

    private int cutterBlendShapeCount;

    public Transform cutterGauge1Transform;
    public Transform cutterGauge2Transform;

    private Gauge cutterGauge1;
    private Gauge cutterGauge2;

    public Transform entryTrigger;
    public Transform exitTrigger;

    public float cutterEjectionSpeed;

    public SlicerState currentSlicerState = SlicerState.NO_POWER_MODE;


    public void SlicerStart()
    {
        cutterMesh = cutterSkinnedMesh.sharedMesh;

        cutterGauge1 = cutterGauge1Transform.GetComponent<Gauge>();
        cutterGauge2 = cutterGauge2Transform.GetComponent<Gauge>();
    }
    public void SlicerUpdate()
    {
        cutterSkinnedMesh.SetBlendShapeWeight(0, cutterGauge1Transform.GetComponent<Gauge>().currentAmount);
        cutterSkinnedMesh.SetBlendShapeWeight(1, cutterGauge2Transform.GetComponent<Gauge>().currentAmount);

        UpdateSlicerState();
    }


    private void UpdateSlicerState()
    {
        // Setting no power mode. //
        if (cutterGauge1.currentState == GaugeState.LOW && cutterGauge2.currentState == GaugeState.LOW)
        {
            currentSlicerState = SlicerState.NO_POWER_MODE;
        }
        if (cutterGauge1.currentState == GaugeState.HIGH || cutterGauge2.currentState == GaugeState.HIGH)
        {
            currentSlicerState = SlicerState.EXTREME_POWER_MODE;
        }
        // Setting extreme power mode. //
        //else if (cutterGauge1.currentState == GaugeState.HIGH && cutterGauge2.currentState == GaugeState.HIGH)
        //{
        //    currentSlicerState = SlicerState.EXTREME_POWER_MODE;
        //}
        //else if (cutterGauge1.currentState == GaugeState.MID && cutterGauge2.currentState == GaugeState.HIGH)
        //{
        //    currentSlicerState = SlicerState.EXTREME_POWER_MODE;
        //}
        //else if (cutterGauge1.currentState == GaugeState.HIGH && cutterGauge2.currentState == GaugeState.MID)
        //{
        //    currentSlicerState = SlicerState.EXTREME_POWER_MODE;
        //}

        // Setting the inbetween modes. //
        else if (cutterGauge1.currentState == GaugeState.MID && cutterGauge2.currentState == GaugeState.MID)
        {
            currentSlicerState = SlicerState.QUARTERING_MODE;
        }
        else if (cutterGauge1.currentState == GaugeState.MID && cutterGauge2.currentState == GaugeState.LOW)
        {
            currentSlicerState = SlicerState.HALFING_MODE;
        }

        else if (cutterGauge1.currentState == GaugeState.LOW && cutterGauge2.currentState == GaugeState.MID)
        {
            currentSlicerState = SlicerState.HALFING_MODE;
        }

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
        cutterGauge1Transform.GetComponent<Gauge>().Increase();
    }
    public void CutterSwitch2()
    {
        Debug.Log("Cutter switch 2 activated.");
        cutterGauge2Transform.GetComponent<Gauge>().Increase();
    }
}
