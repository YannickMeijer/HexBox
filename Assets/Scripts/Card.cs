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
	public bool clicked;
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

	protected virtual void Start()
	{
		moveTime = 1;
        GameObject turnTimer = GameObject.FindGameObjectWithTag("TurnTimer");
        timer = turnTimer.GetComponent<TurnTimer>();
    }
    
	protected virtual void Update()
	{
		if (location == Location.PLAY)
		{
			transform.position = Vector3.MoveTowards(transform.position, currentHex.transform.position, playSpeed * Time.deltaTime);

			transform.rotation = Quaternion.RotateTowards(transform.rotation, currentHex.transform.rotation, rotateSpeed * Time.deltaTime);
		}

		if (clicked)
			difference = -1;
		else
			difference = 0;

	}
    protected virtual void OnMouseUpAsButton()
    {
        if(location == Location.HAND && !clicked)
        {
            clicked = true;
            GlobalMouseHandler.lastSelected = this;
        }
        else
        {
            clicked = false;
            GlobalMouseHandler.lastSelected = null;
        }
    }

    public void Play(HexagonTile targetHex)
    {
        currentHex = targetHex;
        calcSpeed(transform.position, currentHex.transform.position, moveTime);
        calcAngle(transform.rotation, targetHex.transform.rotation, moveTime);
        transform.SetParent(null);
        location = Location.PLAY;
    }

    //Calculates degrees per second required to rotate from currentRotation to targetRotation in goalTime.
    public void calcAngle(Quaternion currentRotation, Quaternion targetRotation, float goalTime)
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
		transform.localPosition = newLoc + new Vector3(0, difference * -1, difference);
	}
}
