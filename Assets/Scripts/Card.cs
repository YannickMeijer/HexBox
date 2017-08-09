using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Card : MonoBehaviour
<<<<<<< HEAD
{ 
=======
{
>>>>>>> d8e84a4d36b5104ceefbe568cdfbcc5113b006ab
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
<<<<<<< HEAD
    public TurnTimer timer;
=======
>>>>>>> d8e84a4d36b5104ceefbe568cdfbcc5113b006ab

	protected virtual void Start()
	{
		moveTime = 1;
<<<<<<< HEAD
        GameObject turnTimer = GameObject.FindGameObjectWithTag("TurnTimer");
        timer = turnTimer.GetComponent<TurnTimer>();
    }
=======
	}
>>>>>>> d8e84a4d36b5104ceefbe568cdfbcc5113b006ab

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

<<<<<<< HEAD

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
=======
	public void UpdatePosition(Vector3 newLoc, float frustum, int cameraDistance)
	{
		transform.localPosition = newLoc + new Vector3(0, difference * -1, difference);
	}

	void OnMouseUpAsButton()
	{

		if (location == Location.HAND)
		{
			Debug.Log("Success");
			clicked = true;
			playerHand.UpdatePlayCard(handPosition);
		}
	}

	public void Play(HexagonTile targetHex)
	{
		currentHex = targetHex;
		currentPos = transform.position;
		calcDistance();
		calcAngle();
		location = Location.PLAY;
	}

	public void calcDistance()
	{
		float tempDis = Vector3.Distance(currentPos, currentHex.transform.position);
		playSpeed = tempDis / moveTime;
	}

	public void calcAngle()
	{
		angle = Quaternion.Angle(transform.rotation, currentHex.transform.rotation);
		rotateSpeed = angle / moveTime;

>>>>>>> d8e84a4d36b5104ceefbe568cdfbcc5113b006ab
	}
}
