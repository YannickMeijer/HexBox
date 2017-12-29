using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayingFieldController : MonoBehaviour
{
    public GameObject tilePrefab;

    private GameObject tilesContainer;

    // Columns, then rows.
    private readonly Dictionary<Point, GameObject> tiles = new Dictionary<Point, GameObject>();

    private IPlayingFieldGenerator playingFieldGenerator = new SquarePlayingFieldGenerator();

    public void Start()
    {
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

        tiles[p] = newTile;
        return newTile;
    }

    public HexagonTile GetNeighbor(HexagonTile p, HexagonDirection direction)
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
                dZ = p.TileX % 2 == 0 ? 0 : 1;
                break;
            case HexagonDirection.LEFT_BOTTOM:
                dX = -1;
                dZ = p.TileX % 2 == 0 ? -1 : 0;
                break;
            case HexagonDirection.RIGHT_TOP:
                dX = 1;
                dZ = p.TileX % 2 == 0 ? 0 : 1;
                break;
            case HexagonDirection.RIGHT_BOTTOM:
                dX = 1;
                dZ = p.TileX % 2 == 0 ? -1 : 0;
                break;
        }

        return this[new Point(p.TileX + dX, p.TileZ + dZ)].GetComponent<HexagonTile>();
    }

    public GameObject this[int x, int z]
    {
        get
        {
            return this[new Point(x, z)];
        }
    }

    public GameObject this[Point p]
    {
        get
        {
            GameObject value = null;
            tiles.TryGetValue(p, out value);
            return value;
        }
    }

}