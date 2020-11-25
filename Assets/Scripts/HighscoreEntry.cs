using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public class HighscoreEntry
{
    public int score;
    public string name;

    public HighscoreEntry(int score, string name)
    {
        this.score = score;
        this.name = name;
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
