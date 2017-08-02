using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HexagonTile : MonoBehaviour
{
	public float Radius = 0.5f;
	public int TileX;
	public int TileZ;

	private float offsetX;
	private float offsetZ;

	private void Start()
	{
		float unitLength = Radius / (Mathf.Sqrt(3) / 2);
		offsetX = unitLength * Mathf.Sqrt(3);
		offsetZ = unitLength * 1.5f;

		SetTilePosition();
	}

	private void SetTilePosition()
	{
		float newZ = TileZ * offsetZ;

		float newX;
		if (TileZ % 2 == 0)
			newX = TileX * offsetX;
		else
			newX = (TileX + 0.5f) * offsetX;

		gameObject.transform.localPosition = new Vector3(newX, 0, newZ);
	}
}