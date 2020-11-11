using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class NewOrderMonitor : MonoBehaviour
{
    public Transform mainScreen;
    public Transform secondaryScreen;

    public ScreenState currentState = ScreenState.MAIN_MENU;

    // Text/dropdownboxes and other UI elements go here. //
    public TextMeshProUGUI ingredientNameText;
    public TextMeshProUGUI spicyText;
    public TextMeshProUGUI chunkyText;
    public TextMeshProUGUI meatText;
    public TextMeshProUGUI colourText;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
