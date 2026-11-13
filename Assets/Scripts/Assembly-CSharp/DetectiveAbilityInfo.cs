using Object = UnityEngine.Object;
using System;
using System.Runtime.CompilerServices;
using AmongUs.Data;
using AmongUs.Data.Settings;
using AmongUs.GameOptions;
using Cpp2ILInjected;
using TMPro;
using UnityEngine;

public class DetectiveAbilityInfo : MonoBehaviour
{
	[SerializeField]
	private PoolablePlayer PlayerIcon;

	[SerializeField]
	private TextMeshPro ColorBlindName;

	[SerializeField]
	private SpriteRenderer MaskingArea;

	[SerializeField]
	private SpriteRenderer[] checkMarkBoxes;

	[SerializeField]
	private SpriteRenderer[] checkMarks;

	public int SuspectCount;

	private NetworkedPlayerInfo PlayerInfo { get; set; }

	public NetworkedPlayerInfo CurrentPlayerInfo => PlayerInfo;

	private void OnEnable()
	{
		DataManager.Settings.Accessibility.OnColorBlindModeChanged += SetColorBlindTag;
	}

	private void OnDisable()
	{
		DataManager.Settings.Accessibility.OnColorBlindModeChanged -= SetColorBlindTag;
	}

	public void SetPlayerInfo(NetworkedPlayerInfo playerInfo, int suspectCount, bool isMeeting = false)
	{
		PlayerInfo = playerInfo;
		PlayerIcon.SetFlipX(flipped: false);
		PlayerIcon.UpdateFromPlayerData(playerInfo, PlayerOutfitType.Default, PlayerMaterial.MaskType.ComplexUI, includePet: false, null, forceAlive: true);
		PlayerIcon.ToggleName(active: false);
		SetColorBlindTag();
		MaskingArea.material.SetInt(PlayerMaterial.MaskLayer, 2);
		PlayerIcon.SetMaskLayer(2);
		SuspectCount = suspectCount;
		if (isMeeting)
		{
			DisableCheckmarks();
			return;
		}
		for (int i = 0; i < checkMarks.Length; i++)
		{
			checkMarks[i].transform.gameObject.SetActive(value: false);
		}
		for (int j = 0; j < suspectCount && j < checkMarks.Length; j++)
		{
			checkMarks[j].transform.gameObject.SetActive(value: true);
		}
		float roleFloat = GameManager.Instance.LogicOptions.GetRoleFloat(FloatOptionNames.DetectiveSuspectLimit);
		for (int k = 1; (float)k < 5f - roleFloat; k++)
		{
			checkMarkBoxes[^k].gameObject.SetActive(value: false);
		}
	}

	public void DisableCheckmarks()
	{
		SpriteRenderer[] array = checkMarkBoxes;
		for (int i = 0; i < array.Length; i++)
		{
			array[i].gameObject.SetActive(value: false);
		}
	}

	public void SetDisabled()
	{
		SpriteRenderer[] componentsInChildren = GetComponentsInChildren<SpriteRenderer>();
		foreach (SpriteRenderer spriteRenderer in componentsInChildren)
		{
			if (spriteRenderer.color.a != 0f)
			{
				spriteRenderer.color = Palette.DisabledClear;
			}
		}
		ColorBlindName.color = Palette.DisabledClear;
	}

	public void SetEnabled()
	{
		SpriteRenderer[] componentsInChildren = GetComponentsInChildren<SpriteRenderer>();
		foreach (SpriteRenderer spriteRenderer in componentsInChildren)
		{
			if (spriteRenderer.color.a != 0f)
			{
				spriteRenderer.color = Color.white;
			}
		}
		componentsInChildren = checkMarks;
		for (int i = 0; i < componentsInChildren.Length; i++)
		{
			componentsInChildren[i].color = Color.white;
		}
		ColorBlindName.color = Palette.White;
		MaskingArea.color = Color.clear;
	}

	private void SetColorBlindTag()
	{
		ColorBlindName.text = PlayerIcon.ColorBlindName;
		ColorBlindName.enabled = DataManager.Settings.Accessibility.ColorBlindMode;
	}
}
