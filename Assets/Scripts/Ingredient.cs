using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ingredient
{
    public string name;
    public float spicyness;
    public float chunkyness;

    Colour colour;

    public Transform prefab;
    public Ingredient(string name, float spicy, float chunky)
    {
        this.name = name;
        spicyness = spicy;
        chunkyness = chunky;
    }

}
