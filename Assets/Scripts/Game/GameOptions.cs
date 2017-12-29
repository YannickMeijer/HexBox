using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class GameOptions : NetworkData
{
    public int PlayerCount = 2;
    public int PlayingFieldSize = 10;
}
