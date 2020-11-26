using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemCollision : MonoBehaviour
{
    GameManager gameManager;
    SoundManager soundManager;

    AudioSource thisAudioSource;
    // Start is called before the first frame update
    void Start()
    {
        gameManager = GameManager.GetInstance();
        soundManager = gameManager.soundManager;

        // Adding audio source to the object
        gameObject.AddComponent<AudioSource>();
        thisAudioSource = gameObject.GetComponent<AudioSource>();
        thisAudioSource.spatialBlend = 1;
        thisAudioSource.volume = soundManager.audioVolume;

        AttachAppropriateSound();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void AttachAppropriateSound()
    {
        switch (gameObject.tag)
        {
            case ("Ingredient"):
                SoundManager.SetSound(thisAudioSource, soundManager.ingredientCollisionSound, false);
                break;
            case ("Item"):
                SoundManager.SetSound(thisAudioSource, soundManager.objectCollisionSound, false);
                break;
            case "Water":
                SoundManager.SetSound(thisAudioSource, soundManager.waterCollisionSound, false);
                break;
            case "Capsule":
                SoundManager.SetSound(thisAudioSource, soundManager.objectCollisionSound, false);
                break;
        }
    }

	private void OnCollisionEnter(Collision collision)
	{
        // We don't want the collision sound to play if the player is holding the object.
        if (gameObject.transform != MouseLook.heldItem)
        {
            SoundManager.PlaySound(thisAudioSource);
        }
        
	}

}
