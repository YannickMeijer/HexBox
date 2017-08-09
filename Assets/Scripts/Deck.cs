using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Deck : MonoBehaviour
{

    public List<Card> deckList;
    public int drawn;

	void Start () {
	}

    public Card DrawCard()
    {
        Card output = deckList[deckList.Count-1];
        drawn += 1;
        deckList.RemoveAt(deckList.Count - 1);
        return output;
    }
}
