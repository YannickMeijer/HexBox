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
		// Get the unit length and offsets.
		float unitLength = Radius / (Mathf.Sqrt(3) / 2);
		offsetX = unitLength * Mathf.Sqrt(3);
		offsetZ = unitLength * 1.5f;

		UpdatePosition();
	}

	private void UpdatePosition()
	{
		// Set the new x and z, keeping the y.
		gameObject.transform.localPosition = new Vector3(
			(TileZ % 2 == 0 ? TileX : TileX + 0.5f) * offsetX, // x is dependent on the column, odd columns are shifted half a unit up.
			gameObject.transform.localPosition.y,
			TileZ * offsetZ
			);
	}
}
