using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingCard : Card
{
    public GameObject Building;

    protected override void Played(HexagonTile tile)
    {
        GameObject buildingObject = Instantiate(Building, tile.transform);
        buildingObject.transform.localRotation = Quaternion.Euler(90, 180, 0); // TODO: fix tile orientation, then this is not necessary.
        base.Played(tile);
    }
}
