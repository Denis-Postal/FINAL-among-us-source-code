using Object = UnityEngine.Object;
using System;
using Cpp2ILInjected;
using TMPro;
using UnityEngine;

public class DetectiveNotesSuspectContainer : MonoBehaviour
{
	[SerializeField]
	private int SuspectIndex = 1;

	[SerializeField]
	private TMP_Text SuspectPlaceholderText;

	[SerializeField]
	private SpriteRenderer BGSprite;

	[SerializeField]
	private GameObject SuspectPrefab;

	[SerializeField]
	private DetectiveNotesSuspectInterface suspectInterface;

	[SerializeField]
	private Sprite activeBGSprite;

	[SerializeField]
	private Sprite inactiveBGSprite;

	private int MaskLayer = 30;

	public unsafe void Awake()
	{
		SuspectPlaceholderText.text = DestroyableSingleton<TranslationController>.Instance.GetString(StringNames.DetectiveNotesPromptSuspect, SuspectIndex.ToString());
	}

	public unsafe void SetSuspect(DetectiveSuspect suspectPlayerInfo, NetworkedPlayerInfo victimPlayerInfo, bool wasDead, int index)
	{
		suspectInterface.SetPlayerInfo(suspectPlayerInfo, victimPlayerInfo, index, MaskLayer);
		if (wasDead || suspectPlayerInfo.playerInfo.Disconnected)
		{
			suspectInterface.DisplayDead();
		}
		SuspectPlaceholderText.gameObject.SetActive(value: false);
		BGSprite.sprite = activeBGSprite;
	}

	public void ClearSuspect()
	{
		if (!(suspectInterface == null))
		{
			suspectInterface.Clear();
			SuspectPlaceholderText.gameObject.SetActive(value: true);
			BGSprite.sprite = inactiveBGSprite;
		}
	}

	public void Disable()
	{
		gameObject.SetActive(value: false);
	}

	public void SetMaskLayer(int layer)
	{
		MaskLayer = layer + SuspectIndex;
		SpriteRenderer[] componentsInChildren = GetComponentsInChildren<SpriteRenderer>();
		foreach (SpriteRenderer spriteRenderer in componentsInChildren)
		{
			if (spriteRenderer.material.HasProperty(PlayerMaterial.MaskLayer) && spriteRenderer.material.GetInt(PlayerMaterial.MaskLayer) != MaskLayer)
			{
				spriteRenderer.material.SetInt(PlayerMaterial.MaskLayer, MaskLayer);
			}
		}
	}
}
