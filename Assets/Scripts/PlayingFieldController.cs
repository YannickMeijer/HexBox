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
		for (int x = 0; x < 10; x++)
			for (int y = 0; y < 10; y++)
			{
				GameObject newTile = Instantiate(tilePrefab, tiles.transform);
				newTile.name = x.ToString() + ',' + y;
				newTile.GetComponent<HexagonTile>().SetTilePosition(x, y);
			}
	}
}