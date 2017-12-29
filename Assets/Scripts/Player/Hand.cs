﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hand : MonoBehaviour
{
    private const float CAMERA_DISTANCE = 3;

    public int StartingHand = 3;
    public int Limit = 10;

    private PlayingFieldController controller;

    private NetworkPlayer network;

    private float frustumWidth;
    private List<GameObject> cards = new List<GameObject>();

    private void Start()
    {
        controller = GameObject.FindGameObjectWithTag("PlayingFieldController").GetComponent<PlayingFieldController>();
        network = GameObject.Find("Network").GetComponent<NetworkPlayer>();
        SetHandPosition();
        controller.Notify += () => TileClickedCard(controller.selectedTile);
        controller.Notify += () => TileClickedUnit(controller.selectedTile);
    }

    /// <summary>
    /// Select a card.
    /// </summary>
    /// <param name="card">The card to select.</param>
    public void SelectCard(Card card)
    {
        cards.ForEach(c => c.GetComponent<Card>().IsSelected = false);
        if (card != null)
            card.IsSelected = true;
    }

    /// <summary>
    /// Indicate a tile has been clicked, playing the selected card (if any).
    /// </summary>
    /// <param name="tile">The clicked tile.</param>
    public void TileClickedCard(HexagonTile tile)
    {
        GameObject selectedCard = cards.Find(card => card.GetComponent<Card>().IsSelected);

        if (selectedCard != null)
        {
            // Play the card.
            cards.Remove(selectedCard);
            selectedCard.GetComponent<Card>().Play(tile);
            UpdateCardPositions(Card.HIGHLIGHT_MOVE_DURATION);

            network.SendText("Playing card: " + selectedCard.name);
        }
    }

    public void TileClickedUnit(HexagonTile tile)
    {
        /*if (selectedUnit != null)
        {
            network.Send("Playing card: " + selectedCard.name);
        }*/
    }

    /// <summary>
    /// Draw a card.
    /// If the hand is full, no card will be drawn.
    /// </summary>
    /// <param name="deck">The deck to draw the card from.</param>
    public void DrawCard(Deck deck)
    {
        if (cards.Count >= Limit)
            return;

        // Try to draw a card.
        GameObject drawn = deck.Draw();
        if (drawn == null)
            return;

        // Add the card to the hand.
        cards.Add(drawn);
        drawn.transform.SetParent(transform);
        drawn.GetComponent<Card>().SetHand(this);

        // Move the card to the hand.
        SmoothMove smoothMove = drawn.GetComponent<SmoothMove>();
        smoothMove.Position.MoveTo(Vector3.zero, Card.PLAY_MOVE_DURATION);
        smoothMove.Rotation.RotateTo(Quaternion.Euler(Vector3.zero), Card.PLAY_MOVE_DURATION);

        smoothMove.Position.DoneOnce += card => card.GetComponent<Card>().Location = CardLocation.HAND;

        UpdateCardPositions(Card.PLAY_MOVE_DURATION);
    }

    /// <summary>
    /// Set each card position based on the position in the hand.
    /// </summary>
    private void UpdateCardPositions(float speed)
    {
        float frustumWidthDivision = frustumWidth / (cards.Count + 1);

        for (int i = 0; i < cards.Count; i++)
        {
            // Calculate the required relative movement, using the SmoothMove target.
            SmoothMove smoothMove = cards[i].GetComponent<SmoothMove>();
            float newX = frustumWidthDivision * (i + 1);
            Vector3 delta = new Vector3(newX - smoothMove.Position.Target.x, 0, 0);
            smoothMove.Position.MoveRelative(delta, speed);

            // Set the card location to floating to prevent highlight, reset to hand when movement is done.
            cards[i].GetComponent<Card>().Location = CardLocation.FLOATING;
            smoothMove.Position.DoneOnce += card => card.GetComponent<Card>().Location = CardLocation.HAND;
        }
    }

    /// <summary>
    /// Set the hand position so the origin is located at the bottom-left corner of the camera.
    /// </summary>
    private void SetHandPosition()
    {
        Camera mainCamera = GetComponentInParent<Camera>();
        float frustumHeight = 2 * CAMERA_DISTANCE * Mathf.Tan(mainCamera.fieldOfView * 0.5f * Mathf.Deg2Rad);
        frustumWidth = mainCamera.aspect * frustumHeight;

        transform.localPosition = new Vector3(-0.5f * frustumWidth, -0.5f * frustumHeight, CAMERA_DISTANCE);
    }
}
