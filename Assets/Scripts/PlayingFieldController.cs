using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayingFieldController : MonoBehaviour
{
	public GameObject tilePrefab;

	private GameObject tilesContainer;

	// Columns, then rows.
	private readonly Dictionary<int, Dictionary<int, GameObject>> columns = new Dictionary<int, Dictionary<int, GameObject>>();

	public void Start()
	{
		tilesContainer = gameObject.transform.Find("Tiles").gameObject;

		// Generate some tiles.
		for (int x = -5; x < 5; x++)
			for (int z = -5; z < 5; z++)
				CreateTile(new Point(x, z));
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

		// Save the tile.
		if (!columns.ContainsKey(p.X))
			columns[p.X] = new Dictionary<int, GameObject>();
		columns[p.X][p.Y] = newTile;

		return newTile;
	}

	public GameObject GetNeighbor(Point p, HexagonDirection direction)
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

		return this[p.X + dX, p.Y + dZ];
	}

	public GameObject this[int x, int z]
	{
		get
		{
			Dictionary<int, GameObject> column;
			GameObject value = null;
			// Try to get the row, if it exists try to get the tile.
			if (columns.TryGetValue(x, out column))
				column.TryGetValue(z, out value);
			return value;
		}
	}

	public GameObject this[Point p]
	{
		get { return this[p.X, p.Y]; }
	}
}