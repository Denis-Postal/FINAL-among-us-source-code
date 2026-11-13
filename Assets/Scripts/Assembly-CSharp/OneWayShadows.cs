using System;
using System.Collections.Generic;
using UnityEngine;

public class OneWayShadows : MonoBehaviour
{
	public Collider2D RoomCollider;

	public bool IgnoreImpostor;

	public void OnEnable()
	{
		LightSource.OneWayShadows.Add(gameObject, this);
	}

	public void OnDisable()
	{
		LightSource.OneWayShadows.Remove(gameObject);
	}

	public bool IsIgnored(LightSource lightSource)
	{
		if (!IgnoreImpostor || !PlayerControl.LocalPlayer.Data.Role.IsImpostor)
		{
			return RoomCollider.OverlapPoint(lightSource.transform.position);
		}
		return true;
	}
}
