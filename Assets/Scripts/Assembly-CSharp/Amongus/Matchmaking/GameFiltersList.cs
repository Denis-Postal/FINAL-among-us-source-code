using System;
using System.Collections.Generic;
using AmongUs.GameOptions;
using Cpp2ILInjected;

namespace AmongUs.Matchmaking
{
	[Serializable]
	public class GameFiltersList
	{
		public List<GameFilterSet> FilterSets= new List<GameFilterSet>
		{
			new GameFilterSet()
		};

		public GameFiltersList()
		{ }
	}
}
