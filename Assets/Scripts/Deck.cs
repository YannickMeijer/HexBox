using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Deck : MonoBehaviour
{
    private static readonly System.Random RANDOM = new System.Random();
    private static readonly Dictionary<CardAddPosition, Action<List<GameObject>, GameObject>> ADD_CARD_METHODS = new Dictionary<CardAddPosition, Action<List<GameObject>, GameObject>>()
    {
        { CardAddPosition.TOP, (list, card) => list.Add(card) },
        { CardAddPosition.BOTTOM, (list, card) => list.Insert(0, card) },
        { CardAddPosition.SHUFFLE, (list, card) => list.Insert(RANDOM.Next(list.Count), card) }
    };

    public GameObject CardPrefab;

    private List<GameObject> cards = new List<GameObject>();

    private void Awake()
    {
        for (int i = 0; i < 10; i++)
            AddCard(Instantiate(CardPrefab));
    }

    /// <summary>
    /// Draw a card from the top of the pile.
    /// If the pile is empty, null is returned.
    /// </summary>
    /// <returns>The drawn card, or null.</returns>
    public GameObject Draw()
    {
        // Check if the deck is empty.
        if (cards.Count == 0)
            return null;

        int topCard = cards.Count - 1;
        GameObject drawn = cards[topCard];
        cards.RemoveAt(topCard);

        drawn.transform.SetParent(null);
        return drawn;
    }

    /// <summary>
    /// Add a card to the deck.
    /// </summary>
    /// <param name="card">The card to add.</param>
    /// <param name="position">Where in the deck to add the card.</param>
    public void AddCard(GameObject card, CardAddPosition position = CardAddPosition.TOP)
    {
        // Set the card's parent and position.
        card.transform.SetParent(transform);
        card.transform.localPosition = Vector3.zero;
        card.transform.localRotation = Quaternion.Euler(-90, 0, 0);
        card.GetComponent<Card>().Location = CardLocation.DECK;

        ADD_CARD_METHODS[position].Invoke(cards, card);
        UpdateCardPositions();
    }

    private void UpdateCardPositions()
    {
        for (int i = 0; i < cards.Count; i++)
        {
            GameObject card = cards[i];
            Vector3 pos = card.transform.localPosition;
            pos.y = i * card.transform.lossyScale.z;
            card.transform.localPosition = pos;
        }
    }

    public enum CardAddPosition
    {
        TOP,
        BOTTOM,
        SHUFFLE
    }
}
