using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hand : MonoBehaviour {

    public List<Card> cardsInHand;
    public int cameraDistance, startingHand;
    private Camera mainCamera;
    public int clickedCard, cardCount;
    public float frustumHeight, frustumWidth;
    public Deck deck;


	void Start() {
        mainCamera = GetComponentInParent<Camera>();
        clickedCard = -1;
        startingHand = 3;
        for (int x = 0; x < cardsInHand.Count; x++)
        {
            cardsInHand[x].transform.SetParent(this.transform);
            cardsInHand[x].transform.rotation = this.transform.rotation;
            cardsInHand[x].playerHand = this;
            cardsInHand[x].location = Card.Location.HAND;
            cardCount++;
        }
    }
	

	void Update () {
        calcFrustum();
        float frustumWidthDivision = frustumWidth / (cardCount + 1);
        for (int x = 0; x < cardCount; x++)
        {
            cardsInHand[x].UpdatePosition(new Vector3( frustumWidthDivision * (x + 1), 0, 0), frustumHeight , cameraDistance);
            cardsInHand[x].handPosition = x;
        }
    }

    public void UpdatePlayCard(int newCard)
    {
        if (clickedCard != -1)
        {
            cardsInHand[clickedCard].clicked = false;
        }
        clickedCard = newCard;
    }

    public void PlayOnHexagon(HexagonTile targetHex)
    {
        Debug.Log("Call Succseful");
            
        if (clickedCard != -1)
        {
            cardsInHand[clickedCard].Play(targetHex);
            
            for (int x = clickedCard + 1; x < cardCount;x++)
            {
                cardsInHand[x - 1] = cardsInHand[x];
                cardsInHand[x - 1].handPosition = x - 1;
            }
            cardCount -= 1;
            clickedCard = -1;
        }
    }

    void DrawCard()
    {
        Card temp = deck.Draw();
    }

    void calcFrustum()
    {
        frustumHeight = 2.0f * cameraDistance * Mathf.Tan(mainCamera.fieldOfView * 0.5f * Mathf.Deg2Rad);
        frustumWidth = mainCamera.aspect * frustumHeight;
        transform.localPosition = new Vector3(-frustumWidth / 2, -frustumHeight / 2, cameraDistance);
        //Places the origin of Hand left and down from the camera,at the edge of what's visible
    }
}
