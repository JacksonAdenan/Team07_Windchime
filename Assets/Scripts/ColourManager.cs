using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public enum Colour_Tag
{ 
    BLUE,
    RED,
    YELLOW,
    PINK,
    ORANGE,
    DARK_RED,
    AQUA,
    ORCHID,
    SALMON,
    LIGHT_GREEN,
    DARK_GREEN,
    VIOLET
}

[Serializable]
public class ColourManager
{
    


    public Colour blue;
    public Colour red;
    public Colour lightGreen;
    public Colour darkGreen;
    public Colour yellow;
    public Colour pink;
    public Colour orange;
    public Colour darkRed;
    public Colour aqua;
    public Colour orchid;
    public Colour salmon;
    public Colour violet;

    public ColourManager()
    {
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    // This function is old man, get with the times.
    public void InitColours()
    {
        ////blue = new Colour("blue", new Vector3(17, 80, 240));
        //blue = new Colour("blue", new Color(25, 130, 150));
        //green = new Colour("green", new Color(50, 100, 30));
        //red = new Colour("red", new Color(200, 30, 50));
        //
        //purple = new Colour("purple", new Color(130, 25, 110));
        //yellow = new Colour("yellow", new Color(220, 200, 20));
        //pink = new Colour("pink", new Color(250, 100, 200));
        //orange = new Colour("orange", new Color(230, 120, 30));

    }

    public Color ConvertColour(Colour normalColour)
    {
        Color unityColour = new Color32(normalColour.RGBValue.r, normalColour.RGBValue.g, normalColour.RGBValue.b, 0);
        return unityColour; 

    }
    public Colour ConvertColour(Colour_Tag colourTag)
    {
        switch (colourTag)
        {
            case Colour_Tag.RED:
                return red;

            case Colour_Tag.BLUE:
                return blue;

            case Colour_Tag.LIGHT_GREEN:
                return lightGreen;
            case Colour_Tag.DARK_GREEN:
                return darkGreen;
            case Colour_Tag.ORANGE:
                return orange;
            case Colour_Tag.PINK:
                return pink;
            case Colour_Tag.YELLOW:
                return yellow;
            case Colour_Tag.DARK_RED:
                return darkRed;
            case Colour_Tag.AQUA:
                return aqua;
            case Colour_Tag.ORCHID:
                return orchid;
            case Colour_Tag.SALMON:
                return salmon;
            case Colour_Tag.VIOLET:
                return violet;

        }

        Debug.Log("ConvertColour() returned null!");
        return null;
    }
}
