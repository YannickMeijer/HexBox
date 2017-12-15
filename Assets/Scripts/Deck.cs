using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Deck : MonoBehaviour
{
    public List<GameObject> cards;

    public Card DrawCard(Hand hand)
    {
        Debug.Log("Drawing a card.");

        if (cards.Count == 0)
            return null;

        Card drawn = Instantiate(cards[0], hand.transform).GetComponent<Card>();
        cards.RemoveAt(0);
        
        drawn.handPosition = hand.Cards.Count;

        return drawn;
    }
}
