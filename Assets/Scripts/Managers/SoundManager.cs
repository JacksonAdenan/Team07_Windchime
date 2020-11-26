using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{

    GameManager gameManager;


    public float audioVolume = 50;

    [Header("Main Menu Sounds")]
    public AudioClip menuButtonSound;

    [HideInInspector]
    public AudioSource mainMenuSource;

    [Header("Player Sounds")]
    public List<AudioClip> playerSounds;

    [Header("Switches and Button Sounds")]
    public List<AudioClip> switchSounds;

    [Header("Cooking Orb Sounds")]
    public AudioClip cookingOrbCookingSound;
    public AudioClip cookingOrbCookingSound2;
    public AudioClip cookingOrbSuccessSound;
    public AudioClip hatchOpen;

    [HideInInspector]
    public AudioSource cookingOrbSource;
    [HideInInspector]
    public AudioSource cookingOrbSource2;
    [HideInInspector]
    public AudioSource cookingOrbHatchSource;

    [Header("Blender Sounds")]
    public AudioClip blendingSound;
    public AudioClip blendingLaunchSound;
    public AudioClip blendingPromptSound;
    public AudioClip blenderButtonSound;

    [HideInInspector]
    public AudioSource blenderSource;
    [HideInInspector]
    public AudioSource blenderButtonSource;


    [Header("Catcher Sounds")]
    public AudioClip catchingSound;

    [HideInInspector]
    public AudioSource catcherSource;

    [Header("UI Clicky Sounds")]
    public AudioClip UI_Interaction_1;
    public AudioClip UI_Interaction_2;
    public AudioClip UI_Fail;

    [HideInInspector]
    public AudioSource newOrderMonitorSource;
    [HideInInspector]
    public AudioSource itemFabMonitorSource;
    [HideInInspector]
    public AudioSource currentOrderMonitorSource;
    [HideInInspector]
    public AudioSource canonMonitorSource;

    [Header("Order Sounds")]
    public AudioClip newOrderSound;

    [HideInInspector]
    public AudioSource orderSource;


    [Header("Canon Sounds")]
    public AudioClip cannonShotSound;
    public AudioClip canonIncinerationSound;
    public AudioClip canonButtonSound;
    public AudioClip canonButtonFailSound;

    [HideInInspector]
    public AudioSource canonSource;
    [HideInInspector]
    public AudioSource canonButtonSource;


    [Header("Button Press Sounds")]
    public AudioClip buttonPressSound;

    [HideInInspector]
    public AudioSource capsuleVendorSource;

    [HideInInspector]
    public AudioSource playerSource;

    [Header("Object Collision Sounds")]
    public AudioClip ingredientCollisionSound;
    public AudioClip waterCollisionSound;
    public AudioClip objectCollisionSound;



    public static void PlaySound(AudioSource audioSource)
    {
        if (audioSource.isPlaying == false)
        { 
            audioSource.Play();
        }
    }
    public static void StopPlayingSound(AudioSource audioSource)
    {
        audioSource.Stop();
    }
    public static void AddAudioSource(Transform obj)
    {
        if (obj.GetComponent<AudioSource>() == null)
        {
            Debug.Log("Added AudioSource component to " + obj.name);
            obj.gameObject.AddComponent<AudioSource>();
            return;
        }

        Debug.Log("Added AudioSource component to " + obj.name + ". This object now has multiple AudioSource components.");
        obj.gameObject.AddComponent<AudioSource>();
    }
    public static void SetSound(AudioSource audioSource, AudioClip sound, bool loop)
    {
        audioSource.clip = sound;
        audioSource.playOnAwake = false;
        audioSource.loop = loop;

    }
    public void SetAudioSource(Transform obj, out AudioSource sourceToHookUp)
    {
        SoundManager.AddAudioSource(obj);

        if (obj.GetComponents<AudioSource>().Length > 1)
        {
            AudioSource[] audioSources;
            audioSources = obj.GetComponents<AudioSource>();
            sourceToHookUp = audioSources[audioSources.Length - 1];
            sourceToHookUp.spatialBlend = 1;
        }
        else
        { 
            sourceToHookUp = obj.GetComponent<AudioSource>();
            sourceToHookUp.spatialBlend = 1;
        }

    }

    // Start is called before the first frame update
    void Start()
    {
        gameManager = GameManager.GetInstance();

        // ----------------------------- Cooking Orb Sound Setup ----------------------------- //
        SetAudioSource(gameManager.cookingManager.theOrb.cookingOrb, out cookingOrbSource);
        SetAudioSource(gameManager.cookingManager.theOrb.cookingOrb, out cookingOrbHatchSource);
        SetAudioSource(gameManager.cookingManager.theOrb.cookingOrb, out cookingOrbSource2);

        // ------------------------------------------------------------------------------------ //

        // ----------------------------- Blender Sound Setup ----------------------------- //
        SetAudioSource(gameManager.cookingManager.theBlender.blender, out blenderSource);
        SetAudioSource(gameManager.cookingManager.theBlender.blenderButton, out blenderButtonSource);
        // We can set the sound for the button here because it probably wont have to change it during the game. //
        SetSound(blenderButtonSource, blenderButtonSound, false);
        // ------------------------------------------------------------------------------------ //

        // ----------------------------- Catcher Sound Setup ----------------------------- //
        SetAudioSource(gameManager.cookingManager.theCatcher.catcherBody, out catcherSource);
        SetSound(catcherSource, catchingSound, false);

        // ----------------------------- Monitors Sound Setup ----------------------------- //
        SetAudioSource(gameManager.monitorManager.canonMonitor, out canonMonitorSource);
        SetSound(canonMonitorSource, UI_Interaction_1, false);

        SetAudioSource(gameManager.monitorManager.currentOrderMonitor, out currentOrderMonitorSource);
        SetSound(currentOrderMonitorSource, UI_Interaction_1, false);

        SetAudioSource(gameManager.monitorManager.newOrderMonitor, out newOrderMonitorSource);
        SetSound(newOrderMonitorSource, UI_Interaction_1, false);

        SetAudioSource(gameManager.monitorManager.itemFabricator, out itemFabMonitorSource);
        SetSound(itemFabMonitorSource, UI_Interaction_1, false);

        SetAudioSource(gameManager.monitorManager.mainMenuMonitor, out mainMenuSource);
        SetSound(mainMenuSource, menuButtonSound, false);
        // ------------------------------------------------------------------------------------ //

        // ----------------------------- Order Sound Setup ----------------------------- //
        SetAudioSource(gameManager.monitorManager.newOrderMonitor, out orderSource);
        SetSound(orderSource, newOrderSound, false);
        // ------------------------------------------------------------------------------------ //

        // ----------------------------- Canon Sound Setup ----------------------------- //
        SetAudioSource(gameManager.cookingManager.theCanon.canon, out canonSource);
        SetAudioSource(gameManager.cookingManager.theCanon.canon, out canonButtonSource);
        // ------------------------------------------------------------------------------------ //

        // ----------------------------- Capsule Vendor Sound Setup ----------------------------- //
        SetAudioSource(gameManager.cookingManager.theVendor.vendorBody, out capsuleVendorSource);
        SetSound(capsuleVendorSource, buttonPressSound, false);
        // ------------------------------------------------------------------------------------ //

        // ----------------------------- Player Source Sound Setup ----------------------------- //
        SetAudioSource(gameManager.playerController.transform.parent, out playerSource);
        // ------------------------------------------------------------------------------------ //

    }

    public void PlayMonitorSound(MonitorType type)
    {

        switch (type)
        {
            case MonitorType.CANON_MONITOR:
                SoundManager.PlaySound(canonMonitorSource);
                break;
            case MonitorType.CURRENT_ORDER_MONITOR:
                SoundManager.PlaySound(currentOrderMonitorSource);
                break;
            case MonitorType.NEW_ORDER_MONITOR:
                SoundManager.PlaySound(newOrderMonitorSource);
                break;
            case MonitorType.ITEM_FABRICATOR_MONITOR:
                SoundManager.PlaySound(itemFabMonitorSource);
                break;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
