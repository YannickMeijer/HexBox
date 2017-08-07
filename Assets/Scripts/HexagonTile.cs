﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HexagonTile : MonoBehaviour
{
	public float Radius = 0.5f;

	[SerializeField]
	private int tileX;
	[SerializeField]
	private int tileZ;

	private float offsetX;
	private float offsetZ;

	private GameObject playerHand;
	private Hand hand;


	private void Start()
	{
		// http://answers.unity3d.com/questions/421509/2d-hexagonal-grid-beginner.html
		// Get the offsets.
		offsetX = Radius * 1.5f;
		offsetZ = Radius * Mathf.Sqrt(3);

		UpdatePosition();

		playerHand = GameObject.FindGameObjectWithTag("Hand");
		hand = playerHand.GetComponent<Hand>();
	}

	private void UpdatePosition()
	{
		// Set the new x and z, keeping the y.
		gameObject.transform.localPosition = new Vector3(
			tileX * offsetX,
			gameObject.transform.localPosition.y,
			(tileX % 2 == 0 ? tileZ : tileZ + 0.5f) * offsetZ // Z is dependent on the column, odd columns are shifted half a unit up.
		);
	}

	public void OnMouseUpAsButton()
	{
		hand.PlayOnHexagon(this);
	}

	public int TileX
	{
		get { return tileX; }
		set
		{
			tileX = value;
			UpdatePosition();
		}
	}

	public int TileZ
	{
		get { return tileZ; }
		set
		{
			tileZ = value;
			UpdatePosition();
		}
	}
}
