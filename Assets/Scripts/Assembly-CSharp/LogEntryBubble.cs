using System;
using AmongUs.Data;
using AmongUs.Data.Settings;
using Cpp2ILInjected;
using TMPro;
using UnityEngine;

public class LogEntryBubble : PoolableBehavior
{
	[SerializeField]
	public SpriteRenderer background;

	[SerializeField]
	public SpriteRenderer maskArea;

	[SerializeField]
	public PoolablePlayer player;

	[SerializeField]
	private TextMeshPro text;

	public void DecorateLog(SecurityLogBehaviour.SecurityLogEntry entry, Sprite backgroundSprite)
	{
		NetworkedPlayerInfo playerById = GameData.Instance.GetPlayerById(entry.PlayerId);
		if (playerById == null)
		{
			Debug.LogError($"Couldn't find player {entry.PlayerId} for log");
			return;
		}
		SetCosmetics(playerById);
		SetText(entry, playerById);
		SetBackground(backgroundSprite);
	}

	public void SetMaskLayer(int maskLayer)
	{
		maskArea.material.SetInt(PlayerMaterial.MaskLayer, maskLayer);
		player.SetMaskLayer(maskLayer);
		background.material.SetInt(PlayerMaterial.MaskLayer, maskLayer);
	}

	private void SetBackground(Sprite backgroundSprite)
	{
		background.sprite = backgroundSprite;
	}

	private void SetCosmetics(NetworkedPlayerInfo playerData)
	{
		player.UpdateFromPlayerData(playerData, PlayerOutfitType.Default, PlayerMaterial.MaskType.ScrollingUI, includePet: false, null, forceAlive: true);
		player.ToggleName(active: false);
	}

	public void SetText(SecurityLogBehaviour.SecurityLogEntry entry, NetworkedPlayerInfo playerData)
	{
		string text = DestroyableSingleton<TranslationController>.Instance.GetString((StringNames)(201 + entry.Location));
		int colorId = playerData.DefaultOutfit.ColorId;
		string value = this.text.text;
		if (DataManager.Settings.Accessibility.ColorBlindMode)
		{
			string text2 = DestroyableSingleton<TranslationController>.Instance.GetString(Palette.ColorNames[colorId]).ToLower();
			this.text.text = DestroyableSingleton<TranslationController>.Instance.GetString(StringNames.SecLogEntryColorblind, playerData.PlayerName, text2, text);
		}
		else
		{
			this.text.text = DestroyableSingleton<TranslationController>.Instance.GetString(StringNames.SecLogEntry, playerData.PlayerName, text);
		}
		if (!this.text.text.Equals(value))
		{
			this.text.ForceMeshUpdate();
		}
	}
}
