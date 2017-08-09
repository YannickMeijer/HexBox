using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class TurnTimer : MonoBehaviour
{

    public bool gradualMovement = true, gracefulStart = true, unitProgress = false;
    //If gradualMovement is true, everytime turnDuration runs out the units will move their movespeed. If gracefulStart is true, the game will not start normal turn progression until all are ready or the timer runs out. 
    public int turnDuration, gracefulDuration;
    //gracefulDuration determines how long the gracefulStart lasts.
    public List<bool> playersReady;
    bool gracefulActive = true;
    //unitProgress is accessed by units to see if they should move at this point. Units should check if they haven't moved in the last frame.
    int orderDuration;
    //orderDuration is used to ensure that all units can see if unitProgress has been changed.
    float internalTimer;
    //The value that is used internally to count down, and which should be used for time depictions.

    void Start()
    {
        if (gracefulStart)
            internalTimer = gracefulDuration;
    }


    void Update()
    {
        while (gracefulStart && gracefulActive) //Lasts until the gracefulStart ends, internaltimer is at least below 0, so units will move immediately.
        {
            internalTimer -= Time.deltaTime;
            if (playersReady.All(players => players == true) || internalTimer > 0)
            {
                gracefulActive = false;
                internalTimer = 0;
            }
        }

        if (orderDuration > 1)
        {
            unitProgress = false;
            internalTimer = turnDuration;
            orderDuration = 0;
        }

        internalTimer -= Time.deltaTime;

        if (internalTimer < 0)
        {
            orderDuration += 1;
            unitProgress = true;
        }
    }
}