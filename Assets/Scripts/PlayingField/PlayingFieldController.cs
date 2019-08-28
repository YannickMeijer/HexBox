using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayingFieldController : MonoBehaviour
{
    public GameObject tilePrefab;

    private GameObject tilesContainer;

    private HexagonDirection[] directions;

    private IPlayingFieldGenerator playingFieldGenerator = new SquarePlayingFieldGenerator();

    public void Start()
    {
        directions = (HexagonDirection[])Enum.GetValues(typeof(HexagonDirection));
        tilesContainer = gameObject.transform.Find("Tiles").gameObject;
        playingFieldGenerator.GenerateField(this, new GameOptions());
    }

    public GameObject CreateTile(Point p)
    {
        // Create the gameobject.
        GameObject newTile = Instantiate(tilePrefab, tilesContainer.transform);
        newTile.name = p.ToString();

        // Set the tile position for the script.
        HexagonTile tile = newTile.GetComponent<HexagonTile>();
        tile.TileX = p.X;
        tile.TileZ = p.Y;

        PlayingField.tiles[p] = newTile;
        return newTile;
    }

    public Point GetNeighbourPoint(Point p, HexagonDirection direction)
    {
        int dX = 0;
        int dZ = 0;

        switch (direction)
        {
            case HexagonDirection.TOP:
                dZ = 1;
                break;
            case HexagonDirection.BOTTOM:
                dZ = -1;
                break;
            case HexagonDirection.LEFT_TOP:
                dX = -1;
                dZ = p.X % 2 == 0 ? 0 : 1;
                break;
            case HexagonDirection.LEFT_BOTTOM:
                dX = -1;
                dZ = p.X % 2 == 0 ? -1 : 0;
                break;
            case HexagonDirection.RIGHT_TOP:
                dX = 1;
                dZ = p.X % 2 == 0 ? 0 : 1;
                break;
            case HexagonDirection.RIGHT_BOTTOM:
                dX = 1;
                dZ = p.X % 2 == 0 ? -1 : 0;
                break;
        }
        return new Point(dX, dZ);
    }

    public HexagonTile GetNeighbor(HexagonTile p, HexagonDirection direction)
    {
        var neighbourPoint = GetNeighbourPoint(new Point(p.TileX, p.TileZ), direction);
        GameObject temp = PlayingField.tiles[neighbourPoint];
        if (temp == null)
            return null;
        return temp.GetComponent<HexagonTile>();
    }

    public List<HexagonTile> GetAllNeighbours(HexagonTile p)
    {
        List<HexagonTile> output = new List<HexagonTile>();
        foreach (HexagonDirection direction in directions)
            output.Add(GetNeighbor(p, direction));

        return output;
    }
}