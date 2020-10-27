using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
[Serializable]
public class Ingredient
{
    public string name;
    public float spicyness;
    public float chunkyness;

    public bool isMeat;
    Colour colour;

    public Transform prefab;
    public Ingredient(string name, float spicy, float chunky, bool isMeat)
    {
        this.name = name;
        spicyness = spicy;
        chunkyness = chunky;
        this.isMeat = isMeat;
    }

}
