namespace AmongUs.InnerNet.GameDataMessages
{
	public enum GameDataTypes : byte
	{
		Invalid = 0,
		DataFlag = 1,
		RpcFlag = 2,
		SpawnFlag = 4,
		DespawnFlag = 5,
		SceneChangeFlag = 6,
		ReadyFlag = 7,
		ChangeSettingsFlag_Deprecated = 8,
		XboxDeclareXuid = 207
	}
}
