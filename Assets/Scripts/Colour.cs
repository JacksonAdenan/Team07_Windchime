using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public enum Colour_Tag
{ 
    BLUE,
    RED,
    GREEN,
    PURPLE,
    YELLOW,
    PINK,
    ORANGE
}

[Serializable]
public class Colour
{
    public string name;
    public Vector3 RGBValue;


    public static Colour blue;
    public static Colour red;
    public static Colour green;
    public static Colour purple;
    public static Colour yellow;
    public static Colour pink;
    public static Colour orange;

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

        purple = new Colour("purple", new Vector3(130, 25, 110));
        yellow = new Colour("yellow", new Vector3(220, 15, 30));
        pink = new Colour("pink", new Vector3(100, 30, 40));
        orange = new Colour("orange", new Vector3(230, 120, 30));

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

            case Colour_Tag.BLUE:
                return Colour.blue;

            case Colour_Tag.GREEN:
                return Colour.green;
            case Colour_Tag.ORANGE:
                return Colour.orange;
            case Colour_Tag.PINK:
                return Colour.pink;
            case Colour_Tag.PURPLE:
                return Colour.purple;
            case Colour_Tag.YELLOW:
                return Colour.yellow;

        }

        return null;
        Debug.Log("ConvertColour() returned null!");
    }
}
