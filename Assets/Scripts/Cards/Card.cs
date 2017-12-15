﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Card : MonoBehaviour
{
    private static readonly Vector3 HIGHLIGHT_POSITION = new Vector3(0, 0.5f, -0.2f);
    private const float HIGHLIGHT_MOVE_DURATION = 0.5f;
    private const float PLAY_MOVE_DURATION = 1.5f;

    // deckLimit: the amount of this type of card that is allowed in your deck.
    public int deckCost, manaCost, deckLimit, handPosition;
    public string flavourText, description, cardName;
    protected int difference;
    public CardLocation location;

    public float playSpeed, rotateSpeed, angle;

    private bool mouseOver;
    private bool wasHighlighted;

    private Hand playerHand;
    protected TurnTimer timer;
    private SmoothMove smoothMove;
    protected HexagonTile currentHex;

    protected virtual void Start()
    {
        playerHand = GameObject.Find("Hand").GetComponent<Hand>();
        timer = GameObject.Find("TurnTimer").GetComponent<TurnTimer>();
        smoothMove = GetComponent<SmoothMove>();
    }

    protected virtual void Update()
    {
        UpdateHighlight();
    }

    private void OnMouseEnter()
    {
        mouseOver = true;
    }

    private void OnMouseExit()
    {
        mouseOver = false;
    }

    protected virtual void OnMouseDown()
    {
        // Select or deselect the card.
        if (location == CardLocation.HAND)
            playerHand.SelectCard(IsSelected ? null : this);
    }

    public void Play(HexagonTile targetHex)
    {
        currentHex = targetHex;

        location = CardLocation.PLAY;
        transform.SetParent(null);

        // Move the card to the tile.
        smoothMove.Position.MoveToAbsolute(targetHex.transform.position + Vector3.up * 0.2f, PLAY_MOVE_DURATION);
        smoothMove.Rotation.RotateTo(targetHex.transform.rotation, PLAY_MOVE_DURATION);
    }

    private void UpdateHighlight()
    {
        if (location != CardLocation.HAND)
            return;

        bool shouldHighlight = mouseOver || IsSelected;

        // Check whether wasHighlighted and shouldHighlight are the same, otherwise update the state.
        if (!wasHighlighted && shouldHighlight)
            smoothMove.Position.MoveRelative(HIGHLIGHT_POSITION, HIGHLIGHT_MOVE_DURATION);
        else if (wasHighlighted && !shouldHighlight)
            smoothMove.Position.MoveRelative(-HIGHLIGHT_POSITION, HIGHLIGHT_MOVE_DURATION);

        wasHighlighted = shouldHighlight;
    }

    public bool IsSelected
    {
        get;
        set;
    }
}