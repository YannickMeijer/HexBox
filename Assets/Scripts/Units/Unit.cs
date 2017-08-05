using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Unit : MonoBehaviour
{
	public int Defense = 0;
	public int MaxHealth = 0;

	private int health;

	private void Start()
	{
		health = MaxHealth;
	}

	protected virtual void Die()
	{
		Destroy(gameObject);
	}

	public int Health
	{
		get { return health; }
		set
		{
			health = Mathf.Clamp(value, 0, MaxHealth);
			if (health <= 0)
				Die();
		}
	}
}