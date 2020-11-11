﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum SwitchType
{
    CUTTER_SWITCH_1,
    CUTTER_SWITCH_2,
    WATER_TAP,
    CANON_BUTTON,
    ORDER_ACCEPT,
    ORDER_REJECT,
    BLENDER_BUTTON,
    ITEM_SPAWNER,
    MONITOR_FORWARD,
    MONITOR_BACK,
    NEXT_ORDER,

    ERROR
}
public class SwitchData : MonoBehaviour
{

    public SwitchType type;
    

    Ingredient ingredientInfo;
    GameManager gameManager;
    CookingManager cookingManager;

    // Cool down timers. //
    public float switchSpamCooldown = 0;
    public float switchCooldownTimer;
    public bool onCooldown;

    // Start is called before the first frame update
    void Start()
    {
        ingredientInfo = GetComponent<Ingredient>();
        gameManager = GameManager.GetInstance();
        cookingManager = gameManager.cookingManager;

        switchCooldownTimer = 0;
        onCooldown = false;
    }

    // Update is called once per frame
    void Update()
    {
        SwitchTimer();
    }

    void Poop()
    { }
    public void ActivateSwitch()
    {
        if (onCooldown == false)
        {
            switch (type)
            {
                case SwitchType.CUTTER_SWITCH_1:
                    onCooldown = true;
                    gameManager.cookingManager.theSlicer.CutterSwitch1();
                    break;
                case SwitchType.CUTTER_SWITCH_2:
                    onCooldown = true;
                    gameManager.cookingManager.theSlicer.CutterSwitch2();
                    break;
                case SwitchType.WATER_TAP:
                    onCooldown = true;
                    CookingManager.WaterTapSwitch();
                    break;
                case SwitchType.ORDER_ACCEPT:
                    onCooldown = true;
                    gameManager.orderManager.AcceptOrder(gameManager.orderManager.requestedOrders[0]);
                    break;
                case SwitchType.ORDER_REJECT:
                    onCooldown = true;
                    gameManager.orderManager.RejectOrder();
                    break;
                case SwitchType.CANON_BUTTON:
                    onCooldown = true;
                    cookingManager.theCanon.ShootCapsule();
                    break;
                case SwitchType.BLENDER_BUTTON:
                    onCooldown = true;
                    gameManager.cookingManager.theBlender.BlenderButton();
                    break;
                case SwitchType.ITEM_SPAWNER:
                    onCooldown = true;
                    cookingManager.IngredientSpawnTimer();
                    break;
                case SwitchType.MONITOR_FORWARD:
                    onCooldown = true;
                    {
                        MonitorScreen monitor = FindMonitorFromSwitch(gameManager.playerController.selectedSwitch);

                        // I know this is really bad but don't judge me. If the switch had an ingredient component, I'm going to assume that its the item fabricator monitor. Giving a reference to the ingredient
                        // will help in displaying the ingredients stats and stuff. //
                        if (gameManager.playerController.selectedSwitch.GetComponent<Ingredient>())
                        {
                            monitor.currentIngredientDisplay = gameManager.playerController.selectedSwitch.GetComponent<Ingredient>();
                            monitor.SetScreenState(ScreenState.SECONDARY);
                        }
                        else
                        {
                            monitor.SetScreenState(ScreenState.SECONDARY);
                        }
                        break;
                    }
                case SwitchType.MONITOR_BACK:
                    onCooldown = true;
                    {
                        MonitorScreen monitor = FindMonitorFromSwitch(gameManager.playerController.selectedSwitch);
                        monitor.SetScreenState(ScreenState.MAIN_MENU);
                        break;
                    }
                case SwitchType.NEXT_ORDER:
                    onCooldown = true;
                    gameManager.orderManager.SwapSelectedOrder();
                    break;
            }
        }
        else
        {
            Debug.Log("You can't spam this button that fast!");
        }

    }

    private MonitorScreen FindMonitorFromSwitch(Transform switchTransform)
    {
        Transform currentObj = switchTransform;
        while (currentObj.GetComponent<MonitorScreen>() == null)
        {
            Debug.Log(currentObj.name);
            currentObj = currentObj.parent;
        }
        return currentObj.GetComponent<MonitorScreen>();
    }
    public void SwitchTimer()
    { 
        // Cooldown for ingredient spawning. //
        if (onCooldown == true)
        {
            switchCooldownTimer += Time.deltaTime;
            if (switchCooldownTimer >= switchSpamCooldown)
            {
                onCooldown = false;
                switchCooldownTimer = 0;
            }
        }
    }
}
