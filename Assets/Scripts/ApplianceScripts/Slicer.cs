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
    public void CutHalf(Transform ingredientTransform)
    {

        if (ingredientTransform.GetComponent<Ingredient>().currentState == IngredientState.WHOLE)
        {

            Ingredient dataToTransfer = ingredientTransform.GetComponent<Ingredient>();
            Transform half1 = UnityEngine.Object.Instantiate(dataToTransfer.halfedPrefab, exitTrigger.position, exitTrigger.rotation);
            Transform half2 = UnityEngine.Object.Instantiate(dataToTransfer.halfedPrefab, exitTrigger.position, exitTrigger.rotation);

            // ======================= NEW INGREDIENT CREATION ===================== //

            half1.position = exitTrigger.position;
            half2.position = exitTrigger.position;

            // Just because we don't have any art for blended foods, ill make the soup orb a blended ingredient by changing the tag and adding an Ingredient script. //
            //half1.gameObject.AddComponent<Ingredient>();
            //half1.gameObject.GetComponent<Ingredient>().Copy(dataToTransfer);
            //half1.tag = "Ingredient";
            //half1.GetComponent<Rigidbody>().isKinematic = false;
            //
            //half2.gameObject.AddComponent<Ingredient>();
            //half2.gameObject.GetComponent<Ingredient>().Copy(dataToTransfer);
            //half2.tag = "Ingredient";
            //half2.GetComponent<Rigidbody>().isKinematic = false;
            //
            //
            //// Editing the cunkyness value. //
            //half1.GetComponent<Ingredient>().chunkyness /= 2;
            //half2.GetComponent<Ingredient>().chunkyness /= 2;
            //Debug.Log("SLICED HALF");

            Ingredient.CreateIngredient(ingredientTransform, half1, IngredientState.HALF);
            Ingredient.CreateIngredient(ingredientTransform, half2, IngredientState.HALF);

            // ====================================================================== //

            Vector3 ingredientScale = ingredientTransform.localScale;
            Vector3 leftPoint = ingredientTransform.position - Vector3.right * ingredientScale.x / 2;
            Vector3 rightPoint = ingredientTransform.position + Vector3.right * ingredientScale.x / 2;
            ingredientTransform.gameObject.SetActive(false);

            // Setting the new ingredients to be active just incase.//
            half1.gameObject.SetActive(true);
            half2.gameObject.SetActive(true);


            // Setting them to be halved. //
            half1.GetComponent<Ingredient>().currentState = IngredientState.HALF;
            half2.GetComponent<Ingredient>().currentState = IngredientState.HALF;


            // Shooting the new peices upwards. //

            ShootUp(half1);
            ShootUp(half2);
        }
        else
        {
            ingredientTransform.position = exitTrigger.position;
            ShootUp(ingredientTransform);
            Debug.Log("Could not cut this ingredient. It is already cut.");
        }
    }

    public void CutQuarter(Transform ingredientTransform)
    {
        if (ingredientTransform.GetComponent<Ingredient>().currentState == IngredientState.WHOLE)
        {

            Ingredient dataToTransfer = ingredientTransform.GetComponent<Ingredient>();
            Transform quarter1 = UnityEngine.Object.Instantiate(dataToTransfer.quateredPrefab, exitTrigger.position, exitTrigger.rotation);
            Transform quarter2 = UnityEngine.Object.Instantiate(dataToTransfer.quateredPrefab, exitTrigger.position, exitTrigger.rotation);
            Transform quarter3 = UnityEngine.Object.Instantiate(dataToTransfer.quateredPrefab, exitTrigger.position, exitTrigger.rotation);
            Transform quarter4 = UnityEngine.Object.Instantiate(dataToTransfer.quateredPrefab, exitTrigger.position, exitTrigger.rotation);

            // ======================= NEW INGREDIENT CREATION ===================== //

            quarter1.position = exitTrigger.position;
            quarter2.position = exitTrigger.position;
            quarter3.position = exitTrigger.position;
            quarter4.position = exitTrigger.position;


            Ingredient.CreateIngredient(ingredientTransform, quarter1, IngredientState.QUARTER);
            Ingredient.CreateIngredient(ingredientTransform, quarter2, IngredientState.QUARTER);
            Ingredient.CreateIngredient(ingredientTransform, quarter3, IngredientState.QUARTER);
            Ingredient.CreateIngredient(ingredientTransform, quarter4, IngredientState.QUARTER);

            // ====================================================================== //

            Vector3 ingredientScale = ingredientTransform.localScale;
            Vector3 leftPoint = ingredientTransform.position - Vector3.right * ingredientScale.x / 2;
            Vector3 rightPoint = ingredientTransform.position + Vector3.right * ingredientScale.x / 2;
            ingredientTransform.gameObject.SetActive(false);

            // Setting the new ingredients to be active just incase.//
            quarter1.gameObject.SetActive(true);
            quarter2.gameObject.SetActive(true);
            quarter3.gameObject.SetActive(true);
            quarter4.gameObject.SetActive(true);


            // Setting them to be halved. //
            quarter1.GetComponent<Ingredient>().currentState = IngredientState.QUARTER;
            quarter2.GetComponent<Ingredient>().currentState = IngredientState.QUARTER;
            quarter3.GetComponent<Ingredient>().currentState = IngredientState.QUARTER;
            quarter4.GetComponent<Ingredient>().currentState = IngredientState.QUARTER;


            // Shooting the new peices upwards. //

            ShootUp(quarter1);
            ShootUp(quarter2);
            ShootUp(quarter3);
            ShootUp(quarter4);
        }
        else
        {
            ingredientTransform.position = exitTrigger.position;
            ShootUp(ingredientTransform);
            Debug.Log("Could not cut this ingredient. It is already cut.");
        }
    }

    public void Slice(Transform ingredient)
    {
        switch (currentSlicerState)
        {
            case SlicerState.EXTREME_POWER_MODE:
                DestroyIngredient(ingredient);
                break;
            case SlicerState.HALFING_MODE:
                CutHalf(ingredient);
                break;
            case SlicerState.QUARTERING_MODE:
                CutQuarter(ingredient);
                break;
            case SlicerState.NO_POWER_MODE:
                PassIngredient(ingredient);
                break;
        }
    }

    private void DestroyIngredient(Transform ingredient)
    {
        GameObject.Destroy(ingredient.gameObject);
    }
    private void PassIngredient(Transform ingredient)
    {
        ingredient.position = exitTrigger.position;
        ShootUp(ingredient);
    }

    private void ShootUp(Transform objToShootUp)
    {
        objToShootUp.GetComponent<Rigidbody>().velocity = Vector3.zero;
        objToShootUp.GetComponent<Rigidbody>().AddForce(Vector3.up * cutterEjectionSpeed);
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
