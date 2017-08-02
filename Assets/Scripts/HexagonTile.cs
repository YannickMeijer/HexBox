using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HexagonTile : MonoBehaviour
{
	private Dictionary<HexagonDirection, HexagonTile> neighbors = new Dictionary<HexagonDirection, HexagonTile>();

	public HexagonTile this[HexagonDirection direction]
	{
		get { return neighbors[direction]; }
		set { neighbors[direction] = value; }
	}
}