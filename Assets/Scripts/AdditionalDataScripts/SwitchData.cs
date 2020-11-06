using System.Collections;
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

    ERROR
}
public class SwitchData : MonoBehaviour
{

    public SwitchType type;
    

    Ingredient ingredientInfo;
    GameManager gameManager;

    // Cool down timers. //
    public float switchSpamCooldown = 0;
    public float switchCooldownTimer;
    public bool onCooldown;

    // Start is called before the first frame update
    void Start()
    {
        ingredientInfo = GetComponent<Ingredient>();
        gameManager = GameManager.GetInstance();

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
        switch (type)
        {
            case SwitchType.CUTTER_SWITCH_1:
                gameManager.cookingManager.theSlicer.CutterSwitch1();
                break;
            case SwitchType.CUTTER_SWITCH_2:
                gameManager.cookingManager.theSlicer.CutterSwitch2();
                break;
            case SwitchType.WATER_TAP:
                CookingManager.WaterTapSwitch();
                break;
            case SwitchType.ORDER_ACCEPT:
                OrderManager.AcceptOrder(OrderManager.requestedOrders[0]);
                break;
            case SwitchType.ORDER_REJECT:
                OrderManager.RejectOrder();
                break;
            case SwitchType.CANON_BUTTON:
                CookingManager.ShootCapsule();
                break;
            case SwitchType.BLENDER_BUTTON:
                gameManager.cookingManager.theBlender.BlenderButton();
                break;
            case SwitchType.ITEM_SPAWNER:
                if (onCooldown == false)
                {
                    onCooldown = true;
                    CookingManager.IngredientSpawnTimer();
                    Debug.Log("Spawning an ingredient.");
                }
                else
                {
                    Debug.Log("You can't spawn an ingredient that fast!");
                }
                break;
            case SwitchType.MONITOR_FORWARD:
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
                {
                    MonitorScreen monitor = FindMonitorFromSwitch(gameManager.playerController.selectedSwitch);
                    monitor.SetScreenState(ScreenState.MAIN_MENU);
                    break;
                }
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
