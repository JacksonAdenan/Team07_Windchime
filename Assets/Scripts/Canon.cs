using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public enum CanonState
{
    EMPTY,
    LOADED
}


[Serializable]
public class Canon
{
    GameManager gameManager;
    OrderManager orderManager;
    SoundManager soundManager;

    [Tooltip("This canon transform is for the animation.")]
    // Transform for animation. //
    public Transform canon;
    // Canon stats and current things. //
    public CanonState currentCanonState = CanonState.EMPTY;
    //public static Soup loadedCapsule;
    [HideInInspector]
    public bool isLoaded = false;

    // These transforms of capsules are just like a thing to show if the canon is loaded or not. They aren't really a part of the game. //
    public Transform canonCapsule;


    // Reference to current SoupData in the canon. //
    [HideInInspector]
    public SoupData currentSoup;


    // Reference to the button so I can use it for a sound source. //
    public Transform canonButton;


    // Start is called before the first frame update
    public void Start()
    {
        gameManager = GameManager.GetInstance();
        orderManager = gameManager.orderManager;
        soundManager = gameManager.soundManager;

        // Giving the capsules soup data components because i'm gonna use them to store information. //
        if (canonCapsule.GetComponent<SoupData>() == null)
        {
            canonCapsule.gameObject.AddComponent<SoupData>();
        }
    }

    // Update is called once per frame
    public void Update()
    {
        // Canon updates //
        UpdateCanonState();
        UpdateCanonCapsule();
    }

    void UpdateCanonCapsule()
    {
        switch (currentCanonState)
        {
            case CanonState.EMPTY:
                canonCapsule.gameObject.SetActive(false);
                canon.GetComponent<Animator>().SetBool("IsOpen", false);

                break;
            case CanonState.LOADED:
                canonCapsule.gameObject.SetActive(true);
                canon.GetComponent<Animator>().SetBool("IsOpen", true);
                break;
        }
    }

    void UpdateCanonState()
    {
        if (!isLoaded)
        {
            currentCanonState = CanonState.EMPTY;
        }
        else
        {
            currentCanonState = CanonState.LOADED;
        }
    }

    public void LoadCanon(Soup theDataToLoad)
    {

        isLoaded = true;


        // Actual loading of soup data. //
        canonCapsule.GetComponent<SoupData>().theSoup = theDataToLoad;
        Debug.Log("Canon loaded and received data successfully.");
        currentSoup = canonCapsule.GetComponent<SoupData>();
    }

    public void UnloadCanon()
    {
        canonCapsule.GetComponent<SoupData>().theSoup = null;
        isLoaded = false;
        Debug.Log("Canon unloaded and removed soup data.");
        currentSoup = null;
    }

    public void ShootCapsule()
    {
        if (orderManager.acceptedOrders.Count > 0 && isLoaded)
        {

            // Launch sound. //
            SoundManager.SetSound(soundManager.canonSource, soundManager.cannonShotSound, false);
            SoundManager.PlaySound(soundManager.canonSource);

            // Button press sound. //
            SoundManager.SetSound(soundManager.canonButtonSource, soundManager.canonButtonSound, false);
            SoundManager.PlaySound(soundManager.canonButtonSource);


            orderManager.CompleteOrder(canonCapsule.GetComponent<SoupData>().theSoup);
            canonCapsule.GetComponent<SoupData>().theSoup = null;
            isLoaded = false;
            Debug.Log("Canon submitted order and removed soup data.");

            // Display points
            MenuManager.DisplayOrderSubmittedText();
        }
        else
        {
            // Button fail sound. //
            SoundManager.SetSound(soundManager.canonButtonSource, soundManager.canonButtonFailSound, false);
            SoundManager.PlaySound(soundManager.canonButtonSource);

            Debug.Log("Tried to submit soup but you do not currently have any orders.");
        }

    }
}
