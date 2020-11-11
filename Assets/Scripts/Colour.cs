using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public enum Colour_Tag
{ 
    BLUE,
    RED,
    GREEN
}

[Serializable]
public class Colour
{
    public string name;
    public Vector3 RGBValue;


    public static Colour blue;
    public static Colour red;
    public static Colour green;

    public Colour(string colourName, Vector3 RGB)
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

    public static void InitColours()
    {
        //blue = new Colour("blue", new Vector3(17, 80, 240));
        blue = new Colour("blue", new Vector3(25, 130, 150));
        green = new Colour("green", new Vector3(50, 100, 30));
        red = new Colour("red", new Vector3(100, 30, 40));
    }

    public static Color ConvertColour(Colour normalColour)
    {
        Color unityColour = new Color32((byte)normalColour.RGBValue.x, (byte)normalColour.RGBValue.y, (byte)normalColour.RGBValue.z, 0);
        return unityColour; 

    }
    public static Colour ConvertColour(Colour_Tag colourTag)
    {
        switch (colourTag)
        {
            case Colour_Tag.RED:
                return Colour.red;
                break;
            case Colour_Tag.BLUE:
                return Colour.blue;
                break;
            case Colour_Tag.GREEN:
                return Colour.green;
                break;
        }

        return null;
        Debug.Log("ConvertColour() returned null!");
    }
}
