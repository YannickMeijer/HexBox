using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hand : MonoBehaviour
{
    private const float CAMERA_DISTANCE = 3;

    public int startingHand = 3;
    public int handLimit = 10;

    private float frustumWidth;
    private List<Card> cards = new List<Card>();
    private Camera mainCamera;
    private Deck deck;

    private void Start()
    {
        GlobalMouseHandler.hand = this;

        mainCamera = GetComponentInParent<Camera>();
        deck = GameObject.Find("Deck").GetComponent<Deck>();

        SetHandPosition();

        // Draw the starting hand.
        for (int x = 0; x < startingHand; x++)
            DrawCard();
    }


    private void Update()
    {
        UpdateCardPositions();
    }

    public void SelectCard(Card card)
    {
        cards.ForEach(c => c.IsSelected = false);
        GlobalMouseHandler.lastSelected = card;

        if (card != null)
            card.IsSelected = true;
    }

    public void PlayOnHexagon(HexagonTile targetHex)
    {
        if (GlobalMouseHandler.lastSelected != null)
        {
            GlobalMouseHandler.lastSelected.Play(targetHex);
            int relevantPos = GlobalMouseHandler.lastSelected.handPosition;
            cards.RemoveAt(relevantPos);

            foreach (Card handCard in cards)
                if (relevantPos < handCard.handPosition)
                    handCard.handPosition -= 1;
        }
    }

    private void DrawCard()
    {
        if (cards.Count < handLimit)
        {
            Card drawn = deck.DrawCard(this);
            if (drawn != null)
                cards.Add(drawn);
        }
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
            cards[i].transform.localPosition = cardTransform;
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
        get { return cards; }
    }
}
