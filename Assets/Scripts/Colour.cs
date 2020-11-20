using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public class Colour
{

    public string name;
    public Color32 RGBValue;

    public Colour(string colourName, Color32 RGB)
    {
        name = colourName;
        RGBValue = RGB;
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
