using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class GlobalMouseHandler: object
{
    public static Card lastSelected;
    public static Hand hand;

    public static void WasClicked(HexagonTile targetHex)
    {
        Debug.Log(lastSelected);
        Debug.Log(targetHex);
        if (lastSelected != null)
        {
            if (lastSelected.location == Card.Location.HAND)
                hand.PlayOnHexagon(targetHex);
            else
            {
                var relevantCard = lastSelected as UnitCard;
                if (relevantCard != null)
                    relevantCard.Pathfinding(targetHex);
            }
        }
    }
}
