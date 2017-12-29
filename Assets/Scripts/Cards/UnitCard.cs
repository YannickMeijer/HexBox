using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitCard : Card
{

    public GameObject unit;
    protected override void Played(HexagonTile tile)
    {
        GameObject buildingObject = Instantiate(unit, tile.transform);
        buildingObject.transform.localRotation = Quaternion.Euler(90, 180, 0); // TODO: fix tile orientation, then this is not necessary.
        base.Played(tile);
    }
}
