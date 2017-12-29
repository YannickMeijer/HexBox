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

    public MonoBehaviour selected{ get; set; }

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

    public void ContextAction(HexagonTile tile)
    {
        if(selected == null)
            return;
        if (selected.GetType() == typeof(Unit))
        {
            ((Unit)selected).FindPath(tile);
            ((Unit)selected).targetHex = tile;
        }
       if (selected.GetType() == typeof(UnitCard)|| selected.GetType() == typeof(BuildingCard)|| selected.GetType() == typeof(Card))
            hand.TileClickedCard(tile);
    }

    
}
