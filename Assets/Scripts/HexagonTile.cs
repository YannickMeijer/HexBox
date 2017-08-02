using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HexagonTile : MonoBehaviour
{
	private void Start()
	{
	}

	public void SetTilePosition(int tileX, int tileY)
	{
		transform.localPosition = new Vector3(tileX, 0, tileY);
	}
}