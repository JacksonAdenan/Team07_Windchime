using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class SwitchData : MonoBehaviour
{

    public SwitchType type;
    

    Ingredient ingredientInfo;
    GameManager gameManager;

    // Cool down timers. //
    public float switchSpamCooldown = 2;
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
                CookingManager.ActivateBlender();
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

        }
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
