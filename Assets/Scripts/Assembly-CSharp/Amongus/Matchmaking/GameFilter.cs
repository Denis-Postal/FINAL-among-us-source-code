using System;
using System.Runtime.Serialization;
using Newtonsoft.Json;

namespace AmongUs.Matchmaking
{
	[Serializable]
	public class GameFilter
	{
		public const string FILTER_TYPE_BOOL = "bool";

		public const string FILTER_TYPE_INT = "int";

		public const string FILTER_TYPE_CATEGORIZED = "cat";

		public const string FILTER_TYPE_MAP = "map";

		public const string FILTER_TYPE_PLATFORM = "platform";

		public const string FILTER_TYPE_CHAT = "chat";

		public const string FILTER_TYPE_LANG = "languages";

		public const string FILTER_TYPE_MOD = "mod";

		public string OptionType;

		public string Key;

		public string SubFilterString;

		[JsonIgnore]
		public ISubFilter SubFilter { get; private set; }

		public GameFilter(string key, ISubFilter subFilter)
		{
			OptionType = subFilter.FilterType;
			Key = key;
			SubFilter = subFilter;
		}

		public void ModifySubFilter(ISubFilter subFilter)
		{
			SubFilter = subFilter;
		}

		[JsonConstructor]
		private GameFilter(string optionType, string key, string subFilterString)
		{
			OptionType = optionType;
			Key = key;
			SubFilterString = subFilterString;
		}

		[OnSerializing]
		internal void OnSerializing(StreamingContext context)
		{
			SubFilterString = JsonConvert.SerializeObject(SubFilter);
		}

		[OnDeserialized]
		internal void OnDeserialized(StreamingContext context)
		{
			SubFilter = ResolveSubFilter(OptionType, SubFilterString);
		}

		private ISubFilter ResolveSubFilter(string type, string filterString)
		{
			return type switch
			{
				"bool" => (ISubFilter)JsonConvert.DeserializeObject<BoolGameFilter>(filterString), 
				"int" => JsonConvert.DeserializeObject<IntGameFilter>(filterString), 
				"cat" => JsonConvert.DeserializeObject<CategorizedGameFilter>(filterString), 
				"map" => JsonConvert.DeserializeObject<MapGameFilter>(filterString), 
				"platform" => JsonConvert.DeserializeObject<PlatformGameFilter>(filterString), 
				"chat" => JsonConvert.DeserializeObject<ChatModeGameFilter>(filterString), 
				"languages" => JsonConvert.DeserializeObject<LanguageFilter>(filterString), 
				_ => null, 
			};
		}
	}
}
