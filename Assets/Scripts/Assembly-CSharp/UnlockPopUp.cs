using System;
using System.Collections;
using AmongUs.Data;
using UnityEngine;

public class UnlockPopUp : MonoBehaviour
{
	public IEnumerator Show()
	{
		DateTime utcNow = AmongUsDateTime.UtcNow;
		if ((utcNow.DayOfYear >= 350 || utcNow.DayOfYear <= 4) && !DataManager.Player.Purchases.GetPurchase("hats_newyears2018", "hats_newyears2018"))
		{
			gameObject.SetActive(value: true);
			DataManager.Player.Purchases.SetPurchased("hats_newyears2018");
			while (isActiveAndEnabled)
			{
				yield return null;
			}
		}
	}
}
