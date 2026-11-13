using AmongUs.GameOptions;
using UnityEngine;

[CreateAssetMenu(menuName = "Game Setting/PlayerSelectionGameSetting")]
public class PlayerSelectionGameSetting : BaseGameSetting
{
	public Int32OptionNames OptionName;

	public override string GetValueString(float value)
	{
		NetworkedPlayerInfo networkedPlayerInfo = GameData.Instance.AllPlayers.Find((NetworkedPlayerInfo info) => info.PlayerId == (int)value);
		if (networkedPlayerInfo != null)
		{
			return networkedPlayerInfo.PlayerName;
		}
		return DestroyableSingleton<TranslationController>.Instance.GetString(StringNames.RoundRobin);
	}
}
