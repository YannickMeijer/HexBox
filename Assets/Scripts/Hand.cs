using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hand : MonoBehaviour
{
    private const float CAMERA_DISTANCE = 3;

    public int startingHand = 3;
    public int handLimit = 10;

    private float frustumWidth;
    private Camera mainCamera;
    private Deck deck;

    private void Start()
    {
        mainCamera = GetComponentInParent<Camera>();
        deck = GameObject.Find("Deck").GetComponent<Deck>();

        SetHandPosition();

        // Draw the starting hand.
        for (int x = 0; x < startingHand; x++)
            DrawCard();
    }

    public void SelectCard(Card card)
    {
        Cards.ForEach(c => c.IsSelected = false);

        if (card != null)
            card.IsSelected = true;
    }

    public void TileClicked(HexagonTile tile)
    {
        Card selectedCard = Cards.Find(c => c.IsSelected);
        if (selectedCard != null)
        {
            selectedCard.Play(tile);
            UpdateCardPositions();
        }
    }

    private void DrawCard()
    {
        if (Cards.Count < handLimit)
        {
            deck.DrawCard(this);
            UpdateCardPositions();
        }
    }

    /// <summary>
    /// Set each card position based on the position in the hand.
    /// </summary>
    private void UpdateCardPositions()
    {
        List<Card> cards = Cards;
        float frustumWidthDivision = frustumWidth / (cards.Count + 1);

        for (int i = 0; i < cards.Count; i++)
        {
            Vector3 cardTransform = cards[i].transform.localPosition;
            cardTransform.x = frustumWidthDivision * (i + 1);
            cards[i].gameObject.GetComponent<SmoothMove>().Position.MoveTo(cardTransform, Card.HIGHLIGHT_MOVE_DURATION);
        }
    }

    /// <summary>
    /// Set the hand position so the origin is located at the bottom-left corner of the camera.
    /// </summary>
    private void SetHandPosition()
    {
        float frustumHeight = 2 * CAMERA_DISTANCE * Mathf.Tan(mainCamera.fieldOfView * 0.5f * Mathf.Deg2Rad);
        frustumWidth = mainCamera.aspect * frustumHeight;

        transform.localPosition = new Vector3(-0.5f * frustumWidth, -0.5f * frustumHeight, CAMERA_DISTANCE);
    }

    public List<Card> Cards
    {
        get { return new List<Card>(GetComponentsInChildren<Card>()); }
    }
}
