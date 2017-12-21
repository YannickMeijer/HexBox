using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class BuildingCardInfo : CardInfo
{
    private const string BUILDING_RESOURCES = "Buildings/";

    [SerializeField]
    private string building;

    public override void Apply(GameObject gameObject)
    {
        BuildingCard card = gameObject.AddComponent<BuildingCard>();
        card.Building = Resources.Load<GameObject>(BUILDING_RESOURCES + building);

        base.Apply(gameObject);
    }
}
