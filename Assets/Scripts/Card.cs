using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Card : MonoBehaviour
{ 

	public int deckCost, manaCost, deckLimit, handPosition;
	public string flavourText, description, cardName;
	public List<string> preRequiredCards;
	//deckLimit, the amount of this type of card that is allowed in your deck. 
	//preRequiredCards: Cards that need to be in the deck in addition to this card if the deck is to be valid. Example: Rondal Twin of the Hammer, Kendal Twin of the spear.
	protected int difference;
	private Vector3 currentPos;
	public HexagonTile currentHex;
	public Hand playerHand;
	public enum Location
	{
		DECK,
		HAND,
		GRAVE,
		PLAY
	}
	public Location location;

	public float moveTime;
	public float playSpeed, rotateSpeed, angle;
    public TurnTimer timer;

	private static readonly Vector3 highlightPosition = new Vector3(0, .5f, 0);
	
	private bool highlight;

	protected virtual void Start()
	{
		moveTime = 1;
        GameObject turnTimer = GameObject.Find("TurnTimer");
        timer = turnTimer.GetComponent<TurnTimer>();

		playerHand = GameObject.Find("Hand").GetComponent<Hand>();
    }
    
	protected virtual void Update()
	{
		if (location == Location.PLAY)
		{
			transform.position = Vector3.MoveTowards(transform.position, currentHex.transform.position, playSpeed * Time.deltaTime);

			transform.rotation = Quaternion.RotateTowards(transform.rotation, currentHex.transform.rotation, rotateSpeed * Time.deltaTime);
		}
	}

	/// <summary>
	/// On mouse enter highlight the card.
	/// </summary>
	private void OnMouseEnter()
	{
		highlight = true;
	}

	/// <summary>
	/// On mouse exit stop highlighting the card.
	/// </summary>
	private void OnMouseExit()
	{
		highlight = false;
	}

	/// <summary>
	/// On mouse down select the card.
	/// </summary>
    protected virtual void OnMouseDown()
    {
        if(location == Location.HAND && !IsSelected)
        {
			playerHand.SelectCard(this);
            GlobalMouseHandler.lastSelected = this;
        }
    }

    public void Play(HexagonTile targetHex)
    {
        currentHex = targetHex;
        calcSpeed(transform.position, currentHex.transform.position, moveTime);
        CalcAngle(transform.rotation, targetHex.transform.rotation, moveTime);
        transform.SetParent(null);
        location = Location.PLAY;
    }

    //Calculates degrees per second required to rotate from currentRotation to targetRotation in goalTime.
    public void CalcAngle(Quaternion currentRotation, Quaternion targetRotation, float goalTime)
    {
        float angle = Quaternion.Angle(currentRotation, targetRotation);
        rotateSpeed = angle / goalTime;
    }

    //Calculates the speed required to move from currentPos to goalPos in goalTime.
    public void calcSpeed(Vector3 currentPos, Vector3 goalPos, float goalTime)
    {
        float tempDis = Vector3.Distance(currentPos, goalPos);
        playSpeed = tempDis / goalTime;
    }

	public void UpdatePosition(Vector3 newLoc, float frustum, int cameraDistance)
	{
		transform.localPosition = newLoc;

		// Check if the card should be highlighted.
		if (IsSelected || highlight)
			transform.localPosition += highlightPosition;
	}

	public bool IsSelected { get; set; }
}
