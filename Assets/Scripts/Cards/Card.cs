﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Card : MonoBehaviour
{
    public const float HIGHLIGHT_MOVE_DURATION = 0.5f;
    public const float PLAY_MOVE_DURATION = 1.5f;
    private static readonly Vector3 HIGHLIGHT_POSITION = new Vector3(0, 0.5f, -0.2f);
    
    public string Description;

    private bool wasHighlighted;

    private Hand playerHand;
    protected TurnTimer timer;
    private SmoothMove smoothMove;
    protected HexagonTile currentHex;
    private MouseHelper mouseHelper;

    protected virtual void Start()
    {
        timer = GameObject.Find("GameController").GetComponent<TurnTimer>();
        smoothMove = GetComponent<SmoothMove>();

        mouseHelper = GetComponent<MouseHelper>();
        mouseHelper.OnClick += () =>
        {
            // Select or deselect the card.
            if (Location == CardLocation.HAND)
                playerHand.SelectCard(IsSelected ? null : this);
        };
    }

    protected virtual void Update()
    {
        UpdateHighlight();
    }

    public void Play(HexagonTile tile)
    {
        currentHex = tile;

        Location = CardLocation.PLAY;
        transform.SetParent(null);

        // Move the card to the tile.
        smoothMove.Position.MoveToAbsolute(tile.transform.position + Vector3.up * 0.2f, PLAY_MOVE_DURATION);
        smoothMove.Rotation.RotateTo(tile.transform.rotation, PLAY_MOVE_DURATION);

        smoothMove.Position.DoneOnce += card => Played(tile);
    }

    public void SetHand(Hand hand)
    {
        Location = CardLocation.HAND;
        playerHand = hand;
    }

    /// <summary>
    /// This method is invoked after the card has been moved to the playing field.
    /// </summary>
    /// <param name="tile">The tile the card has been placed on.</param>
    protected virtual void Played(HexagonTile tile)
    {
        Destroy(gameObject);
    }

    private void UpdateHighlight()
    {
        if (Location != CardLocation.HAND)
            return;

        bool shouldHighlight = mouseHelper.IsMouseOver || IsSelected;

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

    public CardLocation Location
    {
        get;
        set;
    }
}
