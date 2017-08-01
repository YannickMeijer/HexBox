using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Card : MonoBehaviour
{
    public int deckCost, manaCost, deckLimit;
    public string flavourText, description, cardName;
    public List<string> preRequiredCards;
    //deckLimit, the amount of this type of card that is allowed in your deck. 
    //preRequiredCards: Cards that need to be in the deck in addition to this card if the deck is to be valid. Example: Rondal Twin of the Hammer, Kendal Twin of the spear.
    protected int difference;
    public enum Location
    {
        DECK,
        HAND,
        GRAVE,
        PLAY
    }
    public Location location;

    public void UpdatePosition(Vector3 newLoc, float frustum, int cameraDistance)
    {
        transform.localPosition = newLoc + new Vector3(0,0,difference);
    }


    void OnMouseEnter()
    {

        if (location == Location.HAND)
            difference = -1;
    }

    void OnMouseExit()
    {
        if (location == Location.HAND)
            difference = 0;
    }
}
