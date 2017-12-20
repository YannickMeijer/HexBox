using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hand : MonoBehaviour
{
    private const float CAMERA_DISTANCE = 3;

    public GameObject DeckGameObject;
    public int startingHand = 3;
    public int handLimit = 10;

    private float frustumWidth;
    private Camera mainCamera;
    private Deck deck;

    private List<GameObject> cards = new List<GameObject>();

    private void Start()
    {
        mainCamera = GetComponentInParent<Camera>();
        deck = DeckGameObject.GetComponent<Deck>();

        SetHandPosition();

        // Draw the starting hand.
        for (int x = 0; x < startingHand; x++)
            DrawCard();
    }

    public void SelectCard(Card card)
    {
        cards.ForEach(c => c.GetComponent<Card>().IsSelected = false);

        if (card != null)
            card.IsSelected = true;
    }

    public void TileClicked(HexagonTile tile)
    {
        GameObject selectedCard = cards.Find(card => card.GetComponent<Card>().IsSelected);

        if (selectedCard != null)
        {
            cards.Remove(selectedCard);
            selectedCard.GetComponent<Card>().Play(tile);
            UpdateCardPositions();
        }
    }

    private void DrawCard()
    {
        if (cards.Count >= handLimit)
            return;

        // Try to draw a card.
        GameObject drawn = deck.Draw();
        if (drawn == null)
            return;

        // Add the card to the hand.
        drawn.GetComponent<Card>().Location = CardLocation.HAND;
        cards.Add(drawn);

        drawn.transform.SetParent(transform);
        drawn.transform.localPosition = Vector3.zero;
        drawn.transform.localRotation = Quaternion.Euler(0, 0, 0);

        UpdateCardPositions();
    }

    /// <summary>
    /// Set each card position based on the position in the hand.
    /// </summary>
    private void UpdateCardPositions()
    {
        float frustumWidthDivision = frustumWidth / (cards.Count + 1);

        for (int i = 0; i < cards.Count; i++)
        {
            Vector3 cardTransform = cards[i].transform.localPosition;
            cardTransform.x = frustumWidthDivision * (i + 1);
            cards[i].GetComponent<SmoothMove>().Position.MoveTo(cardTransform, Card.HIGHLIGHT_MOVE_DURATION);
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
}
