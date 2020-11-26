using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;


[Serializable]
public class Highscore
{

    public List<HighscoreEntry> highscoreEntries;

    // Start is called before the first frame update
    public void Start()
    {
        highscoreEntries = new List<HighscoreEntry>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
