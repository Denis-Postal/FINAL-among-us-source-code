using System;
using System.Collections.Generic;
using AmongUs.Matchmaking;
using Cpp2ILInjected;
using UnityEngine;

public static class CurrentModRegistration
{
	public static string ModRegistrationGuidString = string.Empty;

	public static bool TryGetModRegistrationGuid(out Guid guid)
	{
		guid = default;
		if (string.IsNullOrEmpty(ModRegistrationGuidString))
		{
			return false;
		}
		if (!Guid.TryParse(ModRegistrationGuidString, out guid))
		{
			Debug.LogWarning("Mod registration guid " + ModRegistrationGuidString + " failed to parse.");
			return false;
		}
		return true;
	}

	public static void UpdateFilterSetWithModRegistrationSettings(GameFilterSet filterSet)
	{
		int count = filterSet.Filters.Count;
		if (TryGetModRegistrationGuid(out var guid))
		{
			bool flag = false;
			for (int i = 0; i < count; i++)
			{
				GameFilter gameFilter = filterSet.Filters[i];
				if (gameFilter.Key == "mod")
				{
					flag = true;
					if (!(gameFilter.SubFilter is ModFilter modFilter))
					{
						Debug.LogWarning($"Cannot update existing mod filter to {guid}");
					}
					else if (!(modFilter.AcceptedValues == guid))
					{
						Guid acceptedValues = modFilter.AcceptedValues;
						Debug.Log($"Updating mod filter from {acceptedValues} to {guid}");
						modFilter.AcceptedValues = guid;
					}
					break;
				}
			}
			if (!flag)
			{
				Debug.Log($"Adding mod filter for {guid}");
				filterSet.Filters.Add(new GameFilter("mod", new ModFilter
				{
					AcceptedValues = guid
				}));
			}
			return;
		}
		for (int num = count - 1; num >= 0; num--)
		{
			if (filterSet.Filters[num].Key == "mod")
			{
				Debug.Log("Clearing mod filter");
				filterSet.Filters.RemoveAt(num);
			}
		}
	}
}
