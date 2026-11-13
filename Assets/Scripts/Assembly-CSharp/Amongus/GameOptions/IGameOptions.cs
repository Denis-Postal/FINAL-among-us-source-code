namespace AmongUs.GameOptions
{
	public interface IGameOptions
	{
		byte Version { get; }

		GameModes GameMode { get; }

		SpecialGameModes SpecialMode { get; }

		GameModes AprilFoolsOnMode { get; }

		GameModes AprilFoolsOffMode { get; }

		RulesPresets RulesPreset { get; }

		int MaxPlayers { get; }

		GameKeywords Keywords { get; }

		byte MapId { get; }

		int NumImpostors { get; }

		int TotalTaskCount { get; }

		bool IsDefaults { get; }

		IRoleOptionsCollection RoleOptions { get; }

		bool AreInvalid(int maxExpectedPlayers);

		void SetRecommendations(int numPlayers, bool isOnlineGame);

		void SetRecommendations(int numPlayers, bool isOnlineGame, RulesPresets rulesPreset);

		void SetByte(ByteOptionNames optionName, byte value);

		void SetFloat(FloatOptionNames optionName, float value);

		void SetBool(BoolOptionNames optionName, bool value);

		void SetInt(Int32OptionNames optionName, int value);

		void SetUInt(UInt32OptionNames optionName, uint value);

		bool TryClearAprilFoolsMode();

		byte GetByte(ByteOptionNames optionName);

		float GetFloat(FloatOptionNames optionName);

		bool GetBool(BoolOptionNames optionName);

		int GetInt(Int32OptionNames optionName);

		int[] GetIntArray(Int32ArrayOptionNames optionName);

		float[] GetFloatArray(FloatArrayOptionNames optionName);

		bool TryGetByte(ByteOptionNames optionName, out byte value);

		bool TryGetFloat(FloatOptionNames optionName, out float value);

		bool TryGetBool(BoolOptionNames optionName, out bool value);

		bool TryGetInt(Int32OptionNames optionName, out int value);

		bool TryGetIntArray(Int32ArrayOptionNames optionName, out int[] value);

		bool TryGetFloatArray(FloatArrayOptionNames optionName, out float[] value);
	}
}
