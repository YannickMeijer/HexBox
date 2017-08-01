using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitCard : Card
{
    public int health, defense, attack, sight, contactDamage;

    void Start()
    {
        location = Location.HAND;
    }

    // Update is called once per frame
    void Update()
    {
        if (location == Location.HAND)
        {
        }

    }


}
