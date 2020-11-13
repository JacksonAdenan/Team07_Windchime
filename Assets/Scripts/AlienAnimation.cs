﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;


public enum AlienState
{ 
    ORDERING,
    LEAVING,
    WAITING,
    WAITING_2,
    LEAVING_HAPPILY
}

[Serializable]
public class AlienAnimation
{
    public AlienState currentState = AlienState.ORDERING;
    public Transform alien;


    public float timer = 0;
    public bool destroy = false;

    public float waitTimer = 0;
    public void CreateAlien(Transform alienTransform)
    {
        alien = GameObject.Instantiate(alienTransform, alienTransform.transform.parent);
    }
    // Start is called before the first frame update
    public void DeleteAlien()
    {
        timer += Time.deltaTime;
        if (timer >= 10)
        {
            GameManager gameManager = GameManager.GetInstance();
            gameManager.orderManager.destroyingAliens.Remove(gameManager.orderManager.destroyingAliens[0]);
            GameObject.Destroy(alien.gameObject);
            alien = null;
            destroy = false;
        }
    }
    public void Animate()
    {
        switch (currentState)
        {
            case AlienState.ORDERING:
                alien.GetComponent<Animator>().SetInteger("AlienPosition", 1);
                alien.GetChild(1).GetComponent<Animator>().SetBool("Ordering", true);
                break;
            case AlienState.LEAVING:
                alien.GetComponent<Animator>().SetInteger("AlienPosition", 4);
                break;
            case AlienState.WAITING:
                alien.GetComponent<Animator>().SetInteger("AlienPosition", 2);
                waitTimer += Time.deltaTime;
                break;
            case AlienState.WAITING_2:
                alien.GetComponent<Animator>().SetInteger("AlienPosition", 1);
                waitTimer += Time.deltaTime;
                break;
            case AlienState.LEAVING_HAPPILY:
                alien.GetComponent<Animator>().SetInteger("AlienPosition", 3);
                break;
        }
    }
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
