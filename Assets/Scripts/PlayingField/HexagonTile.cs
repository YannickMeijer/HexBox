using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HexagonTile : MonoBehaviour
{
    public PlayingFieldController controller;
    public float Radius = 0.5f;
    public Card card;

    [SerializeField]
    private int tileX;
    [SerializeField]
    private int tileZ;

    private float offsetX;
    private float offsetZ;
    private Hand playerHand;
    private PlayerData owner;

    private void Start()
    {
        // http://answers.unity3d.com/questions/421509/2d-hexagonal-grid-beginner.html
        // Get the offsets.
        offsetX = Radius * 1.5f;
        offsetZ = Radius * Mathf.Sqrt(3);
        UpdatePosition();

        UpdateTooltip();

        playerHand = GameObject.Find("LocalPlayer").GetComponent<Hand>();
        GetComponent<MouseHelper>().OnClick += () => controller.selectedTile = this;
        GetComponent<MouseHelper>().OnClick += controller.TriggerNotify;
    }

    private void UpdatePosition()
    {
        // Set the new x and z, keeping the y.
        gameObject.transform.localPosition = new Vector3(
            tileX * offsetX,
            gameObject.transform.localPosition.y,
            (tileX % 2 == 0 ? tileZ : tileZ + 0.5f) * offsetZ // Z is dependent on the column, odd columns are shifted half a unit up.
        );
    }

    private void UpdateTooltip()
    {
        string ownerString = owner == null ? "Uncharted area" : "Owner: " + owner.Name;
        GetComponent<Tooltip>().Text = "<b>Hex Tile</b>\n" + ownerString;
    }

    public int TileX
    {
        get { return tileX; }
        set
        {
            tileX = value;
            UpdatePosition();
        }
    }

    public int TileZ
    {
        get { return tileZ; }
        set
        {
            tileZ = value;
            UpdatePosition();
        }
    }

    public PlayerData Owner
    {
        get { return owner; }
        set
        {
            owner = value;
            UpdateTooltip();
        }
    }
}
