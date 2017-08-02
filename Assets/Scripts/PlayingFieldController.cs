using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayingFieldController : MonoBehaviour
{
	public GameObject tilePrefab;

	private GameObject tiles;

	public void Start()
	{
		tiles = gameObject.transform.Find("Tiles").gameObject;
		GenerateTiles();
	}

	public HexagonTile GetTile(int x, int y)
	{
		Transform child = tiles.transform.Find(x + "," + y);
		return child == null ? null : child.gameObject.GetComponent<HexagonTile>();
	}

	private void GenerateTiles()
	{
		for (int x = -5; x < 5; x++)
			for (int y = -5; y < 5; y++)
			{
				GameObject newTile = Instantiate(tilePrefab, tiles.transform);
				newTile.name = x.ToString() + ',' + y;
				HexagonTile tile = newTile.GetComponent<HexagonTile>();
				tile.TileX = x;
				tile.TileZ = y;
			}
	}
}