using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class GameOptions : NetworkData
{
    public WorldShape WorldShape = WorldShape.SQUARE;
    public int WorldSize = 10;
    public int PlayerCount = 2;
}
