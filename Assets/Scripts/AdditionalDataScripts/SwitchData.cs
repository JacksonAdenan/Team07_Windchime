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
    NEXT_ORDER,
    COOKING_ORB_HATCH,
    CAPSULE_VENDOR,
    CANON_INCINERATE_BUTTON,

    // ------- MAIN MENU BUTTONS ------- //
    MAIN_MENU_PLAY,
    MAIN_MENU_QUIT,
    MAIN_MENU_OPTIONS,

    ERROR
}
public class SwitchData : MonoBehaviour
{

    public SwitchType type;
    

    Ingredient ingredientInfo;
    GameManager gameManager;
    CookingManager cookingManager;
    SoundManager soundManager;

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
        soundManager = gameManager.soundManager;

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
                    SoundManager.StopPlayingSound(gameManager.cookingManager.theSlicer.cutterGauge1Transform.GetComponent<AudioSource>());
                    SoundManager.PlaySound(gameManager.cookingManager.theSlicer.cutterGauge1Transform.GetComponent<AudioSource>());
                    break;
                case SwitchType.CUTTER_SWITCH_2:
                    onCooldown = true;
                    gameManager.cookingManager.theSlicer.CutterSwitch2();
                    SoundManager.StopPlayingSound(gameManager.cookingManager.theSlicer.cutterGauge2Transform.GetComponent<AudioSource>());
                    SoundManager.PlaySound(gameManager.cookingManager.theSlicer.cutterGauge2Transform.GetComponent<AudioSource>());
                    break;
                case SwitchType.WATER_TAP:
                    onCooldown = true;
                    CookingManager.WaterTapSwitch();
                    SoundManager.PlaySound(gameManager.cookingManager.waterTap.GetComponent<AudioSource>());
                    break;
                case SwitchType.ORDER_ACCEPT:
                    onCooldown = true;

                    SoundManager.PlaySound(soundManager.newOrderMonitorSource);

                    gameManager.orderManager.AcceptOrder(gameManager.orderManager.requestedOrders[0]);
                    break;
                case SwitchType.ORDER_REJECT:
                    onCooldown = true;

                    SoundManager.PlaySound(soundManager.newOrderMonitorSource);

                    gameManager.orderManager.RejectOrder();
                    break;
                case SwitchType.CANON_BUTTON:
                    onCooldown = true;
                    cookingManager.theCanon.ShootCapsule();
                    break;
                case SwitchType.CANON_INCINERATE_BUTTON:
                    onCooldown = true;
                    cookingManager.theCanon.IncinerateCapsule();
                    break;
                case SwitchType.BLENDER_BUTTON:
                    onCooldown = true;
                    gameManager.cookingManager.theBlender.BlenderButton();
                    SoundManager.PlaySound(soundManager.blenderButtonSource);
                    break;
                case SwitchType.ITEM_SPAWNER:
                    onCooldown = true;

                    SoundManager.StopPlayingSound(soundManager.itemFabMonitorSource);
                    SoundManager.PlaySound(soundManager.itemFabMonitorSource);

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
                            soundManager.PlayMonitorSound(monitor.thisMonitor);
                            monitor.currentIngredientDisplay = gameManager.playerController.selectedSwitch.GetComponent<Ingredient>();
                            monitor.SetScreenState(ScreenState.SECONDARY);
                        }
                        else
                        {
                            soundManager.PlayMonitorSound(monitor.thisMonitor);
                            monitor.SetScreenState(ScreenState.SECONDARY);
                        }
                        break;
                    }
                case SwitchType.MONITOR_BACK:
                    onCooldown = true;
                    {
                        MonitorScreen monitor = FindMonitorFromSwitch(gameManager.playerController.selectedSwitch);

                        soundManager.PlayMonitorSound(monitor.thisMonitor);

                        monitor.SetScreenState(ScreenState.MAIN_MENU);
                        break;
                    }
                case SwitchType.NEXT_ORDER:
                    onCooldown = true;

                    SoundManager.PlaySound(soundManager.currentOrderMonitorSource);

                    gameManager.orderManager.SwapSelectedOrder();
                    break;
                case SwitchType.COOKING_ORB_HATCH:
                    onCooldown = true;
                    if (gameManager.cookingManager.theOrb.currentCookingOrbState == CookingOrbState.INGREDIENTS_AND_WATER)
                    {
                        gameManager.cookingManager.theOrb.BeginCooking();
                    }
                    break;
                case SwitchType.CAPSULE_VENDOR:
                    onCooldown = true;
                    SoundManager.StopPlayingSound(soundManager.capsuleVendorSource);
                    SoundManager.PlaySound(soundManager.capsuleVendorSource);
                    gameManager.cookingManager.theVendor.SpawnCapsule();
                    break;
                case SwitchType.MAIN_MENU_PLAY:
                    onCooldown = true;
                    SoundManager.StopPlayingSound(soundManager.mainMenuSource);
                    SoundManager.PlaySound(soundManager.mainMenuSource);
                    gameManager.StartGame();

                    gameManager.playerController.selectedSwitch = null;
                    break;
                case SwitchType.MAIN_MENU_QUIT:
                    onCooldown = true;
                    SoundManager.StopPlayingSound(soundManager.mainMenuSource);
                    SoundManager.PlaySound(soundManager.mainMenuSource);
                    Application.Quit();
                    Debug.Log("Quitting game...");
                    gameManager.playerController.selectedSwitch = null;
                    break;
                case SwitchType.MAIN_MENU_OPTIONS:
                    onCooldown = true;
                    SoundManager.StopPlayingSound(soundManager.mainMenuSource);
                    SoundManager.PlaySound(soundManager.mainMenuSource);
                    gameManager.menuManager.ActivateMenu(MenuState.optionMenu);
                    gameManager.playerController.selectedSwitch = null;
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
