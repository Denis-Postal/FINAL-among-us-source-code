using System;
using System.Collections;
using System.Collections.Generic;
using AmongUs.Data;
using AmongUs.Data.Settings;
using InnerNet;
using UnityEngine;

public class FilterTagManager : DestroyableSingleton<FilterTagManager>
{
	private static TimeSpan FilterTagRefreshCooldown= TimeSpan.FromHours(12.0);

	private Logger logger = new Logger(Logger.Category.Network, "FilterTagManager");

	public void RefreshTags()
	{
		GameFilterOptions validGameFilterOptions = DataManager.Settings.Multiplayer.ValidGameFilterOptions;
		TimeSpan timeSpan = DateTime.UtcNow - validGameFilterOptions.LastRefreshTime;
		if (timeSpan <= FilterTagRefreshCooldown)
		{
			logger.Info($"Using FilterTags from {timeSpan} ago");
		}
		else
		{
			StartCoroutine(DestroyableSingleton<HttpMatchmakerManager>.Instance.CoRefreshTags(HandleRefreshedTags));
		}
	}

	private void HandleRefreshedTags(HashSet<string> tags)
	{
		logger.Info($"Server returned {tags.Count} extra tags");
		GameFilterOptions gameFilterOptions = new GameFilterOptions();
		gameFilterOptions.ResetToDefaults(DateTime.UtcNow);
		gameFilterOptions.FilterTags.UnionWith(tags);
		DataManager.Settings.Multiplayer.ValidGameFilterOptions = gameFilterOptions;
	}

	static FilterTagManager()
	{ }
}
