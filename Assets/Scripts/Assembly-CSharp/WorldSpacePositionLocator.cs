using System;
using UnityEngine;

public class WorldSpacePositionLocator : MonoBehaviour
{
	public Vector2 WorldSpacePosition;

	private void Update()
	{
		WorldSpacePosition = transform.position;
	}
}
