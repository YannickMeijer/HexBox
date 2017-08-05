using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Deck : MonoBehaviour {

    public List<Card> deckList;
    public int drawn;


	void Start () {
	}
	
    public Card Draw()
    {
        Card output =  deckList[0 + drawn];
        drawn += 1;
        return output;
    }
}
