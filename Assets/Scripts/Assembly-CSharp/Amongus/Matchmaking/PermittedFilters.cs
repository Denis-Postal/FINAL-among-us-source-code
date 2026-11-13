using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace AmongUs.Matchmaking
{
	[Serializable]
	public class PermittedFilters
	{
		[JsonProperty("filters")]
		public List<Filters> Filters;
	}
}
