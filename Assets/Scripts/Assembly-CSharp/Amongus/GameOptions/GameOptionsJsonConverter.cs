using System;
using Cpp2ILInjected;
using Hazel;
using Newtonsoft.Json;

namespace AmongUs.GameOptions
{
	public class GameOptionsJsonConverter : JsonConverter<IGameOptions>
	{
		private readonly GameOptionsFactory optionsFactory;

		public override bool CanWrite => true;

		public override bool CanRead => true;

		public GameOptionsJsonConverter(GameOptionsFactory factory)
		{
			if (factory == null)
			{
				throw new ArgumentNullException("factory");
			}
			optionsFactory = factory;
		}

		public override void WriteJson(JsonWriter writer, IGameOptions value, JsonSerializer serializer)
		{
			byte[] inArray = optionsFactory.ToBytes(value, forceAprilFoolsMode: false);
			writer.WriteValue(Convert.ToBase64String(inArray));
		}

		public override IGameOptions ReadJson(JsonReader reader, Type objectType, IGameOptions existingValue, bool hasExistingValue, JsonSerializer serializer)
		{
			string text = (string)reader.Value;
			if (text != null)
			{
				return optionsFactory.FromBytes(Convert.FromBase64String(text));
			}
			throw new JsonException("IGameOptions string value cannot be null");
		}
	}
}
