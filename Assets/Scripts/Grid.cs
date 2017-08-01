/*using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grid
{
	/*
	 * Hexagon grid layout:
	 * 
	 * 0 | 0 1 2
	 * 1 |  0 1 2
	 * 2 | 0 1 2
	 * 3 |  0 1 2
	 * ...
	 *
	private List<List<HexagonTile>> rows = new List<List<HexagonTile>>();

	public Grid(int width, int height)
	{
		// Check if the dimensions are positive.
		if (width <= 0 || height <= 0)
			throw new ArgumentException("Width and height of the grid must be greater than zero.");

		// Initialize the empty grid.
		for (int y = 0; y < height; y++)
		{
			rows.Add(new List<HexagonTile>());
			for (int x = 0; x < width; x++)
				rows[y].Add(null);
		}
	}

	public HexagonTile this[int x, int y]
	{
		get => rows[y][x];
		set
		{
			// Save the old value, set the new one.
			HexagonTile oldValue = rows[y][x];
			rows[y][x] = value;

			// Set all neighbor references.
			HexagonTile neighbor;

			// Set the x for tiles on this row.
			int leftX = x - 1;
			int rightX = x + 1;

			// Left, right.
			if (leftX >= 0 && (neighbor = rows[y][leftX]) != null)
			{
				value[HexagonDirection.LEFT] = neighbor;
				neighbor[HexagonDirection.RIGHT] = value;
			}
			if (rightX < Width && (neighbor = rows[y][rightX]) != null)
			{
				value[HexagonDirection.RIGHT] = neighbor;
				neighbor[HexagonDirection.LEFT] = value;
			}

			// Set the x for tiles above and under this row.
			leftX = y % 2 == 0 ? x - 1 : x;
			rightX = leftX + 1;

			// Top.
			if (y > 0)
			{
				if (leftX >= 0 && (neighbor = rows[y - 1][leftX]) != null)
				{
					value[HexagonDirection.TOP_LEFT] = neighbor;
					neighbor[HexagonDirection.BOTTOM_RIGHT] = value;
				}
				if (rightX < Width && (neighbor = rows[y - 1][rightX]) != null)
				{
					value[HexagonDirection.TOP_RIGHT] = neighbor;
					neighbor[HexagonDirection.BOTTOM_LEFT] = value;
				}
			}

			// Bottom.
			if (y < Height - 1)
			{
				if (leftX >= 0 && (neighbor = rows[y + 1][leftX]) != null)
				{
					value[HexagonDirection.BOTTOM_LEFT] = neighbor;
					neighbor[HexagonDirection.TOP_RIGHT] = value;
				}
				if (rightX < Width && (neighbor = rows[y + 1][rightX]) != null)
				{
					value[HexagonDirection.BOTTOM_RIGHT] = neighbor;
					neighbor[HexagonDirection.TOP_LEFT] = value;
				}
			}
		}
	}

	public int Width => rows[0].Count;

	public int Height => rows.Count;
}
*/