using System;
using System.Collections.Generic;
using Cpp2ILInjected;
using Hazel;

public class EndGameResult
{
	public static List<CachedPlayerData> CachedWinners= new List<CachedPlayerData>();

	public static CachedPlayerData CachedLocalPlayer;

	public static GameOverReason CachedGameOverReason= GameOverReason.CrewmatesByTask;

	public static bool CachedShowAd;

	public static ProgressionManager.XpGrantResult CachedXpGrantResult;

	public static ProgressionManager.CurrencyGrantResult CachedBeansGrantResult;

	public static ProgressionManager.CurrencyGrantResult CachedPodsGrantResult;

	public readonly GameOverReason GameOverReason;

	public readonly bool ShowAd;

	public readonly ProgressionManager.XpGrantResult XpGrantResult;

	public readonly ProgressionManager.CurrencyGrantResult PodsGrantResult;

	public readonly ProgressionManager.CurrencyGrantResult BeansGrantResult;

	private EndGameResult(GameOverReason gameOverReason, bool showAd, ProgressionManager.XpGrantResult xpGrantResult = null, ProgressionManager.CurrencyGrantResult podsGrantResult = null, ProgressionManager.CurrencyGrantResult beansGrantResult = null)
	{
		GameOverReason = gameOverReason;
		ShowAd = showAd;
		XpGrantResult = xpGrantResult;
		PodsGrantResult = podsGrantResult;
		BeansGrantResult = beansGrantResult;
		ResetCachedValues();
		CachedGameOverReason = GameOverReason;
		CachedShowAd = ShowAd;
		CachedXpGrantResult = XpGrantResult;
		CachedPodsGrantResult = PodsGrantResult;
		CachedBeansGrantResult = BeansGrantResult;
	}

	public static void ResetCachedValues()
	{
		CachedWinners.Clear();
		CachedLocalPlayer = null;
		CachedGameOverReason = GameOverReason.CrewmatesByTask;
		CachedShowAd = false;
		CachedXpGrantResult = null;
		CachedPodsGrantResult = null;
		CachedBeansGrantResult = null;
	}

	// TODO(verify): only the base payload (reason byte + showAd bool) is parsed. The optional progression sub-message
	// (tag 1: xp / pod / bean grant results) the official server appends is not decoded (field mapping not recoverable
	// from the decompile); a server such as Impostor does not send it.
	public static EndGameResult Create(MessageReader reader)
	{
		GameOverReason gameOverReason = (GameOverReason)reader.ReadByte();
		bool showAd = reader.ReadBoolean();
		if (reader.Position < reader.Length)
		{
			MessageReader messageReader = reader.ReadMessage();
			if (messageReader.Tag == 1)
			{
				uint oldXpAmount = (uint)messageReader.ReadInt32();
				uint grantedXp = (uint)messageReader.ReadInt32();
				uint num = (uint)messageReader.ReadInt32();
				messageReader.ReadInt32();
				uint oldLevel = (uint)messageReader.ReadInt32();
				uint newLevel = (uint)messageReader.ReadInt32();
				uint maxLevel = (uint)messageReader.ReadInt32();
				string text = messageReader.ReadString();
				messageReader.ReadInt32();
				uint grantedPodsWithMultiplierApplied = (uint)messageReader.ReadInt32();
				messageReader.ReadInt32();
				uint grantedPodsWithMultiplierApplied2 = (uint)messageReader.ReadInt32();
				bool flag = messageReader.ReadBoolean();
				float multiplier = 1f;
				float multiplier2 = 1f;
				uint xpRequiredToLevelUpNextLevel = num;
				if (flag)
				{
					multiplier = messageReader.ReadSingle();
					multiplier2 = messageReader.ReadSingle();
					xpRequiredToLevelUpNextLevel = (uint)messageReader.ReadInt32();
				}
				ProgressionManager.XpGrantResult xpGrantResult = new ProgressionManager.XpGrantResult(grantedXp, oldXpAmount, num, xpRequiredToLevelUpNextLevel, flag, oldLevel, newLevel, maxLevel);
				ProgressionManager.CurrencyGrantResult podsGrantResult = new ProgressionManager.CurrencyGrantResult(text, (uint)DestroyableSingleton<InventoryManager>.Instance.GetPodCount(text), grantedPodsWithMultiplierApplied, multiplier);
				ProgressionManager.CurrencyGrantResult beansGrantResult = new ProgressionManager.CurrencyGrantResult(Constants.Beans, (uint)DestroyableSingleton<InventoryManager>.Instance.UnusedBeans, grantedPodsWithMultiplierApplied2, multiplier2);
				return new EndGameResult(gameOverReason, showAd, xpGrantResult, podsGrantResult, beansGrantResult);
			}
		}
		return new EndGameResult(gameOverReason, showAd);
	}

	static EndGameResult()
	{ }
}
