using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public GameObject NormalDeck;

    private Hand hand;

    private void Start()
    {
        hand = GetComponent<Hand>();

        // Draw the starting hand.
        for (int x = 0; x < hand.StartingHand; x++)
            DrawCard();
    }

    private void DrawCard()
    {
        hand.DrawCard(NormalDeck.GetComponent<Deck>());
    }
}
