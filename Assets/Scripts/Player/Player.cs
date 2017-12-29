using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public GameObject NormalDeck;

    private Hand hand;

    private List<Unit> myUnits;
    private List<Building> myHealth;
    private List<Building> myMana;
    private List<Building> remainingBuildings;

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
