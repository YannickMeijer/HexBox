using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Card : MonoBehaviour
{
    public const float HIGHLIGHT_MOVE_DURATION = 0.5f;
    private static readonly Vector3 HIGHLIGHT_POSITION = new Vector3(0, 0.5f, -0.2f);
    private const float PLAY_MOVE_DURATION = 1.5f;

    protected CardLocation location = CardLocation.HAND;

    private bool wasHighlighted;

    private Hand playerHand;
    protected TurnTimer timer;
    private SmoothMove smoothMove;
    protected HexagonTile currentHex;
    private MouseHelper mouseHelper;

    protected virtual void Start()
    {
        playerHand = GameObject.Find("MainCamera/Hand").GetComponent<Hand>();
        timer = GameObject.Find("TurnTimer").GetComponent<TurnTimer>();
        smoothMove = GetComponent<SmoothMove>();
        mouseHelper = GetComponent<MouseHelper>();
    }

    protected virtual void Update()
    {
        UpdateHighlight();
    }

    protected virtual void OnMouseDown()
    {
        // Select or deselect the card.
        if (location == CardLocation.HAND)
            playerHand.SelectCard(IsSelected ? null : this);
    }

    public void Play(HexagonTile tile)
    {
        currentHex = tile;

        location = CardLocation.PLAY;
        transform.SetParent(null);

        // Move the card to the tile.
        smoothMove.Position.MoveToAbsolute(tile.transform.position + Vector3.up * 0.2f, PLAY_MOVE_DURATION);
        smoothMove.Rotation.RotateTo(tile.transform.rotation, PLAY_MOVE_DURATION);

        smoothMove.Position.Done += () => Played(tile);
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
        if (location != CardLocation.HAND)
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
}
