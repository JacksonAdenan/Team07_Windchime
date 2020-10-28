using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CubeCut : MonoBehaviour
{
    public static bool Cut(Transform victim, Vector3 _pos)
    {
        Vector3 pos = new Vector3(_pos.x, victim.position.y, victim.position.z);
        Vector3 victimScale = victim.localScale;
        float distance = Vector3.Distance(victim.position, pos);
        if (distance >= victimScale.x / 2)
        {
            return false;
        }

        Vector3 leftPoint = victim.position - Vector3.right * victimScale.x / 2;
        Vector3 rightPoint = victim.position + Vector3.right * victimScale.x / 2;
        Material mat = victim.GetComponent<MeshRenderer>().material;
        Destroy(victim.gameObject);

        GameObject rightSideObj = GameObject.CreatePrimitive(PrimitiveType.Cube); //primitivetype determines the shape after cutting
        rightSideObj.transform.position = (rightPoint + pos) / 2;
        float rightwidth = Vector3.Distance(pos, rightPoint);
        rightSideObj.transform.localScale = new Vector3(rightwidth, victimScale.y, victimScale.z);
        rightSideObj.AddComponent<Rigidbody>().mass = 100.0f;
        rightSideObj.GetComponent<MeshRenderer>().material = mat;

        GameObject leftSideObj = GameObject.CreatePrimitive(PrimitiveType.Cube); //primitivetype determines the shape after cutting
        leftSideObj.transform.position = (leftPoint + pos) / 2;
        float leftwidth = Vector3.Distance(pos, leftPoint);
        leftSideObj.transform.localScale = new Vector3(leftwidth, victimScale.y, victimScale.z);
        leftSideObj.AddComponent<Rigidbody>().mass = 100.0f;
        leftSideObj.GetComponent<MeshRenderer>().material = mat;

        return true;
    }
}
