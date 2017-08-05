using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

public class Point
{
	public Point(int x, int y)
	{
		X = x;
		Y = y;
	}

	public override string ToString()
	{
		return X.ToString() + ',' + Y;
	}

	public override bool Equals(object obj)
	{
		if (obj == null || !(obj is Point))
			return false;

		Point other = obj as Point;
		return other.X == X && other.Y == Y;
	}

	public override int GetHashCode()
	{
		return ToString().GetHashCode();
	}

	public int X { get; set; }
	public int Y { get; set; }
}
