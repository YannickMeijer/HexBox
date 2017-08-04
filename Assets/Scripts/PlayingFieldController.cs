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
			for (int y = -5; y < 5; y++)
				CreateTile(new Point(x, y));
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

	public GameObject this[Point p]
	{
		get
		{
			Dictionary<int, GameObject> column;
			GameObject value = null;
			// Try to get the row, if it exists try to get the tile.
			if (columns.TryGetValue(p.X, out column))
				column.TryGetValue(p.Y, out value);
			return value;
		}
	}
}