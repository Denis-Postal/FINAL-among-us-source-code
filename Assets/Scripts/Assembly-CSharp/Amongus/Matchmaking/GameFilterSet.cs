using System;
using System.Collections.Generic;
using AmongUs.GameOptions;
using Cpp2ILInjected;

namespace AmongUs.Matchmaking
{
	[Serializable]
	public class GameFilterSet
	{
		public GameModes GameMode;

		public List<GameFilter> Filters;

		public GameFilterSet(GameModes mode)
		{
			GameMode = mode;
			Filters = new List<GameFilter>();
		}

		public GameFilterSet()
		{
			GameMode = GameModes.Normal;
			Filters = new List<GameFilter>();
		}
	}
}
