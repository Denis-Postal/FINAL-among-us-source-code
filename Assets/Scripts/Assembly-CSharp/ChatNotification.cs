using Object = UnityEngine.Object;
using System;
using TMPro;
using UnityEngine;

public class ChatNotification : MonoBehaviour
{
	[SerializeField]
	private PoolablePlayer player;

	[SerializeField]
	private TextMeshPro chatText;

	[SerializeField]
	private TextMeshPro playerNameText;

	[SerializeField]
	private TextMeshPro playerColorText;

	[SerializeField]
	private SpriteRenderer background;

	[SerializeField]
	private SpriteRenderer maskArea;

	private float timeOnScreen;

	private const int maskLayer = 55;

	private void Awake()
	{
		SetMaskLayer();
	}

	public unsafe void SetUp(PlayerControl sender, string text)
	{
		if (!ShipStatus.Instance && !PlayerCustomizationMenu.Instance)
		{
			timeOnScreen = 5f;
			gameObject.SetActive(value: true);
			SetCosmetics(sender.Data);
			string text2 = ColorUtility.ToHtmlStringRGB(Palette.TextColors[player.ColorId]);
			Color color = Palette.TextOutlineColors[player.ColorId];
			playerColorText.text = player.ColorBlindName;
			playerNameText.text = "<color=#" + text2 + ">" + (string.IsNullOrEmpty(sender.Data.PlayerName) ? "..." : sender.Data.PlayerName);
			playerNameText.outlineColor = color;
			chatText.color = Color.black;
			chatText.text = text;
		}
	}

	public void SetCosmetics(NetworkedPlayerInfo playerInfo)
	{
		if (gameObject.activeInHierarchy)
		{
			player.UpdateFromEitherPlayerDataOrCache(playerInfo, PlayerOutfitType.Default, PlayerMaterial.MaskType.ScrollingUI, includePet: false);
		}
		player.ToggleName(active: false);
	}

	private void SetMaskLayer()
	{
		maskArea.material.SetInt(PlayerMaterial.MaskLayer, 55);
		player.SetMaskLayer(55);
		background.material.SetInt(PlayerMaterial.MaskLayer, 55);
	}

	private void Update()
	{
		timeOnScreen -= Time.deltaTime;
		if (timeOnScreen <= 0f || (bool)PlayerCustomizationMenu.Instance)
		{
			Close();
		}
	}

	public void Close()
	{
		gameObject.SetActive(value: false);
	}
}
