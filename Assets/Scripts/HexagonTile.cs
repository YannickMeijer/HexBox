using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HexagonTile : MonoBehaviour
{
	private Dictionary<HexagonDirection, HexagonTile> neighbors = new Dictionary<HexagonDirection, HexagonTile>();

	private void Start()
	{

	}

	private void Update()
	{

	}

	public HexagonTile this[HexagonDirection direction]
	{
		get => neighbors[direction];
		set { neighbors[direction] = value; }
	}
}