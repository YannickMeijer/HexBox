using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class PlayingField
{
    //Column then Rows
    public static readonly Dictionary<Point, GameObject> tiles = new Dictionary<Point, GameObject>();

    public static GameObject RetrieveTile(Point p)
    {
        GameObject result;
        if (tiles.TryGetValue(p, out result))
        {
            return result;
        }
        return null;
    }
}
