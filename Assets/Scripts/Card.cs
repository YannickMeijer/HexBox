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

    protected virtual void Start()
    {
        moveTime = 1;
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

    public void UpdatePosition(Vector3 newLoc, float frustum, int cameraDistance)
    {
        transform.localPosition = newLoc + new Vector3(0,difference * -1,difference);
    }

    void OnMouseUpAsButton()
    {
        
        if(location == Location.HAND)
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

    }
}
