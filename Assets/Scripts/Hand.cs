using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hand : MonoBehaviour
{
    public List<Card> cardsInHand;
    public int cameraDistance, startingHand;
    private Camera mainCamera;
    public int handLimit;
    public float frustumHeight, frustumWidth;
    public Deck deck;


    void Start()
    {
        mainCamera = GetComponentInParent<Camera>();
        startingHand = 3;
        handLimit = 10;
        GlobalMouseHandler.hand = this;

        for (int x = 0; x < startingHand; x++)
            DrawCard();
    }


    void Update()
    {
        calcFrustum();
        float frustumWidthDivision = frustumWidth / (cardsInHand.Count + 1);

        foreach (Card playCard in cardsInHand)
            playCard.UpdatePosition(new Vector3(frustumWidthDivision * (playCard.handPosition + 1), 0, 0), frustumHeight, cameraDistance);
    }


    public void PlayOnHexagon(HexagonTile targetHex)
    {
        if (GlobalMouseHandler.lastSelected != null)
        {
            GlobalMouseHandler.lastSelected.Play(targetHex);
            int relevantPos = GlobalMouseHandler.lastSelected.handPosition;
            cardsInHand.RemoveAt(relevantPos);

            foreach (Card handCard in cardsInHand)
                if (relevantPos < handCard.handPosition)
                    handCard.handPosition -= 1;
        }
    }

    void DrawCard()
    {
        if (cardsInHand.Count < handLimit)
        {
            Card temp = deck.DrawCard();
            temp.transform.SetParent(this.transform);
            temp.transform.rotation = this.transform.rotation;
            temp.location = Card.Location.HAND;
            temp.handPosition = cardsInHand.Count;
            cardsInHand.Add(temp);
        }

    }

    void calcFrustum()
    {
        frustumHeight = 2.0f * cameraDistance * Mathf.Tan(mainCamera.fieldOfView * 0.5f * Mathf.Deg2Rad);
        frustumWidth = mainCamera.aspect * frustumHeight;

        transform.localPosition = new Vector3(-frustumWidth / 2, -frustumHeight / 2, cameraDistance);
        //Places the origin of Hand left and down from the camera, at the edge of what's visible.
    }
}