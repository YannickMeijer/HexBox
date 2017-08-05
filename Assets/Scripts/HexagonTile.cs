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
		// http://answers.unity3d.com/questions/421509/2d-hexagonal-grid-beginner.html
		// Get the offsets.
		offsetX = Radius * 1.5f;
		offsetZ = Radius * Mathf.Sqrt(3);

		UpdatePosition();
	}

	private void UpdatePosition()
	{
		// Set the new x and z, keeping the y.
		gameObject.transform.localPosition = new Vector3(
			TileX * offsetX,
			gameObject.transform.localPosition.y,
			(TileX % 2 == 0 ? TileZ : TileZ + 0.5f) * offsetZ // Z is dependent on the column, odd columns are shifted half a unit up.
		);
	}
}
