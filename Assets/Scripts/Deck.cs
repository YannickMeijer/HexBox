using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Deck : MonoBehaviour
{
    public GameObject CardPrefab;

    public void DrawCard(Hand hand)
    {
        GameObject card = Instantiate(CardPrefab, hand.transform);

        // TODO: separate this to a nice card component generator or something.
        card.AddComponent<BuildingCard>();
        BuildingCard component = card.GetComponent<BuildingCard>();
        component.Building = Resources.Load<GameObject>("Buildings/Cube");
    }
}
