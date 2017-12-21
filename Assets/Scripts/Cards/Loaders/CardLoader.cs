﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardLoader : MonoBehaviour
{
    private const string CARD_RESOURCES = "cards/";
    private static readonly Dictionary<CardType, Type> CARD_TYPES = new Dictionary<CardType, Type>()
    {
        { CardType.BUILDING, typeof(BuildingCardInfo) }
    };

    public GameObject CardPrefab;

    public GameObject Load(string name)
    {
        // Load the json, get the card type.
        string json = Resources.Load<TextAsset>(CARD_RESOURCES + name).text;
        CardTypeInfo typeInfo = JsonUtility.FromJson<CardTypeInfo>(json);

        // Load the actual card.
        CardInfo cardInfo = JsonUtility.FromJson(json, CARD_TYPES[typeInfo.Type]) as CardInfo;
        GameObject card = Instantiate(CardPrefab);
        cardInfo.Apply(card);
        return card;
    }

    private enum CardType
    {
        BUILDING = 0
    }

    [Serializable]
    private class CardTypeInfo
    {
        [SerializeField]
        private CardType type = CardType.BUILDING;

        public CardType Type
        {
            get { return type; }
        }
    }
}