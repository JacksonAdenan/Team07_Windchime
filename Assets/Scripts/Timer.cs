using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class Timer : MonoBehaviour
{
    public Text timeRemaining;
    public float time = 10;

    private void Update()
    {
        time -= Time.deltaTime;
        timeRemaining.text = "Time Remaing: " + time.ToString("F2");
    }
}
