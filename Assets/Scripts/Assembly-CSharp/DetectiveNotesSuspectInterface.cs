using System;
using AmongUs.Data;
using AmongUs.Data.Settings;
using Cpp2ILInjected;
using TMPro;
using UnityEngine;

public class DetectiveNotesSuspectInterface : MonoBehaviour
{
	[SerializeField]
	public GameObject container;

	[SerializeField]
	private PoolablePlayer player;

	[SerializeField]
	public GameObject deadImage;

	[SerializeField]
	private TMP_Text playerName;

	[SerializeField]
	private TMP_Text playerColor;

	[SerializeField]
	private TMP_Text locationName;

	[SerializeField]
	private TMP_Text numberedText;

	public unsafe void SetPlayerInfo(DetectiveSuspect suspectPlayerInfo, NetworkedPlayerInfo victimPlayerInfo, int index, int maskLayer)
	{
		container.SetActive(value: true);
		player.UpdateFromPlayerData(suspectPlayerInfo.playerInfo, PlayerOutfitType.Default, PlayerMaterial.MaskType.ComplexUI, includePet: false, null, forceAlive: true);
		playerName.text = suspectPlayerInfo.playerInfo.PlayerName;
		if (DataManager.Settings.Accessibility.ColorBlindMode)
		{
			playerColor.text = suspectPlayerInfo.playerInfo.GetPlayerColorString();
		}
		else
		{
			playerColor.text = string.Empty;
		}
		if (suspectPlayerInfo.shapeshifterPlayerInfo != null)
		{
			locationName.text = (PlayerControl.LocalPlayer.Data.Role as DetectiveRole).GetPlayerLocation(suspectPlayerInfo.shapeshifterPlayerInfo.PlayerId, victimPlayerInfo.PlayerId);
		}
		else
		{
			locationName.text = (PlayerControl.LocalPlayer.Data.Role as DetectiveRole).GetPlayerLocation(suspectPlayerInfo.playerInfo.PlayerId, victimPlayerInfo.PlayerId);
		}
		numberedText.text = DestroyableSingleton<TranslationController>.Instance.GetString(StringNames.DetectiveNotesSuspectNumber, index);
		player.SetMaskLayer(maskLayer);
	}

	public void DisplayDead()
	{
		deadImage.SetActive(value: true);
	}

	public void Clear()
	{
		deadImage.SetActive(value: false);
		container.SetActive(value: false);
	}
}
