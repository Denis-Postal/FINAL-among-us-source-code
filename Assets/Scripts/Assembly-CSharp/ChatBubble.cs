using AmongUs.Data;
using TMPro;
using UnityEngine;
using UnityEngine.Rendering;

internal class ChatBubble : PoolableBehavior
{
	private static readonly Vector3 PlayerMessageScale = new Vector3(0.35f, 0.35f, 1f);

	private static readonly Vector3 PlayerNotificationScale = new Vector3(0.25f, 0.25f, 1f);

	private static readonly Vector3 VotedAndDeadMarkerPosition = new Vector3(-0.72f, 0.29f, -0.1f);

	private static readonly Vector3 VotedAndDeadMarkerPositionRight = new Vector3(0.72f, 0.29f, -0.1f);

	public PoolablePlayer Player;

	public SpriteRenderer Xmark;

	public SpriteRenderer votedMark;

	public TextMeshPro NameText;

	public TextMeshPro TextArea;

	public SpriteRenderer Background;

	public SpriteRenderer MaskArea;

	public PlatformIdentifier PlatformIcon;

	public TextMeshPro ColorBlindName;

	private NetworkedPlayerInfo playerInfo;

	private int maskLayer;

	private float playerZ = -0.1f;

	private SortingGroup sortingGroup;

	public void SetLeft()
	{
		transform.localPosition = new Vector3(-3f, 0f, 0f);
		Player.SetFlipX(flipped: false);
		Player.transform.localScale = PlayerMessageScale;
		Player.transform.localPosition = new Vector3(0.04f, -0.08f, playerZ);
		Xmark.transform.localPosition = VotedAndDeadMarkerPosition;
		votedMark.transform.localPosition = VotedAndDeadMarkerPosition;
		NameText.rectTransform.pivot = new Vector2(0f, 1f);
		NameText.transform.localPosition = new Vector3(0.5f, 0.358f, 0f);
		NameText.horizontalAlignment = HorizontalAlignmentOptions.Left;
		TextArea.rectTransform.pivot = new Vector2(0f, 1f);
		TextArea.transform.localPosition = new Vector3(0.5f, 0.125f, 0f);
		TextArea.horizontalAlignment = HorizontalAlignmentOptions.Left;
		ColorBlindName.rectTransform.pivot = new Vector2(0f, 1f);
		ColorBlindName.transform.localPosition = new Vector3(-0.22f, -0.071f, -0.5f);
	}

	public void SetNotification()
	{
		transform.localPosition = new Vector3(-2.75f, 0f, 0f);
		Player.SetFlipX(flipped: false);
		Player.transform.localScale = PlayerNotificationScale;
		Player.transform.localPosition = new Vector3(0f, 0.06f, playerZ);
		Xmark.transform.localPosition = VotedAndDeadMarkerPosition;
		votedMark.transform.localPosition = VotedAndDeadMarkerPosition;
		NameText.rectTransform.pivot = new Vector2(0f, 1f);
		NameText.transform.localPosition = new Vector3(0.5f, 0.358f, 0f);
		NameText.horizontalAlignment = HorizontalAlignmentOptions.Left;
		TextArea.rectTransform.pivot = new Vector2(0f, 1f);
		TextArea.transform.localPosition = new Vector3(0.5f, 0.125f, 0f);
		TextArea.horizontalAlignment = HorizontalAlignmentOptions.Left;
		ColorBlindName.rectTransform.pivot = new Vector2(0f, 1f);
		ColorBlindName.transform.localPosition = new Vector3(-0.22f, 0.12f, -0.5f);
		TextArea.text = string.Empty;
	}

	public void SetWarning(string text)
	{
		Player.gameObject.SetActive(value: false);
		NameText.gameObject.SetActive(value: false);
		ColorBlindName.gameObject.SetActive(value: false);
		SetText(text);
		TextArea.color = Color.red;
		TextArea.rectTransform.pivot = new Vector2(0f, 0f);
		TextArea.transform.localPosition = new Vector3(-0.25f, -0.15f, 0f);
		TextArea.horizontalAlignment = HorizontalAlignmentOptions.Left;
	}

	public void SetCosmetics(NetworkedPlayerInfo playerInfo)
	{
		this.playerInfo = playerInfo;
		if (gameObject.activeInHierarchy)
		{
			Player.UpdateFromEitherPlayerDataOrCache(playerInfo, PlayerOutfitType.Default, PlayerMaterial.MaskType.ScrollingUI, includePet: false);
		}
		Player.ToggleName(active: false);
		maskLayer = 51 + PoolIndex;
		SetMaskLayer();
		SetColorblindText();
	}

	public void SetRight()
	{
		transform.localPosition = new Vector3(-2.35f, 0f, 0f);
		Player.SetFlipX(flipped: true);
		Player.transform.localScale = PlayerMessageScale;
		Player.transform.localPosition = new Vector3((Player.CurrentBodyType == PlayerBodyTypes.Horse) ? 5.25f : 4.75f, -0.08f, playerZ);
		Xmark.transform.localPosition = VotedAndDeadMarkerPositionRight;
		votedMark.transform.localPosition = VotedAndDeadMarkerPositionRight;
		NameText.rectTransform.pivot = new Vector2(1f, 1f);
		NameText.transform.localPosition = new Vector3(4.35f, 0.358f, 0f);
		NameText.horizontalAlignment = HorizontalAlignmentOptions.Right;
		TextArea.rectTransform.pivot = new Vector2(1f, 1f);
		TextArea.transform.localPosition = new Vector3(4.35f, 0.125f, 0f);
		TextArea.horizontalAlignment = HorizontalAlignmentOptions.Right;
		ColorBlindName.rectTransform.pivot = new Vector2(1f, 1f);
		ColorBlindName.transform.localPosition = new Vector3(4.98f, -0.071f, -0.5f);
	}

	public void SetName(string playerName, bool isDead, bool voted, Color color)
	{
		NameText.text = playerName ?? "...";
		NameText.color = color;
		NameText.ForceMeshUpdate(ignoreActiveState: true, forceTextReparsing: true);
		if (isDead)
		{
			Xmark.enabled = true;
			Background.color = Palette.HalfWhite;
		}
		if (voted)
		{
			votedMark.enabled = true;
		}
	}

	public override void Reset()
	{
		Player.gameObject.SetActive(value: true);
		NameText.gameObject.SetActive(value: true);
		Xmark.enabled = false;
		votedMark.enabled = false;
		Background.color = Color.white;
		TextArea.color = Color.black;
		playerInfo = null;
	}

	public void AlignChildren()
	{
		Vector3 localPosition = Background.transform.localPosition;
		localPosition.y = NameText.transform.localPosition.y - Background.size.y / 2f + 0.05f;
		Background.transform.localPosition = localPosition;
		MaskArea.transform.localPosition = localPosition + new Vector3(0f, 0.02f, 0.1f);
	}

	internal void SetText(string chatText)
	{
		TextArea.color = Color.black;
		TextArea.text = chatText;
		TextArea.ForceMeshUpdate(ignoreActiveState: true, forceTextReparsing: true);
		Background.size = new Vector2(5.52f, 0.2f + NameText.GetNotDumbRenderedHeight() + TextArea.GetNotDumbRenderedHeight());
		MaskArea.size = Background.size - new Vector2(0f, 0.03f);
	}

	private void OnEnable()
	{
		if (playerInfo != null)
		{
			Player.UpdateFromEitherPlayerDataOrCache(playerInfo, PlayerOutfitType.Default, PlayerMaterial.MaskType.ScrollingUI, includePet: false);
			SetMaskLayer();
			SetColorblindText();
		}
		DataManager.Settings.Accessibility.OnColorBlindModeChanged += SetColorblindText;
	}

	private void OnDisable()
	{
		DataManager.Settings.Accessibility.OnColorBlindModeChanged -= SetColorblindText;
	}

	private void SetColorblindText()
	{
		ColorBlindName.enabled = DataManager.Settings.Accessibility.ColorBlindMode;
		ColorBlindName.text = Player.ColorBlindName;
	}

	private void SetMaskLayer()
	{
		if (sortingGroup == null)
		{
			sortingGroup = GetComponent<SortingGroup>();
			if (sortingGroup == null)
			{
				sortingGroup = gameObject.AddComponent<SortingGroup>();
			}
		}
		sortingGroup.sortingLayerID = Background.sortingLayerID;
		sortingGroup.sortingOrder = Background.sortingOrder;
		MaskArea.material.SetInt(PlayerMaterial.MaskLayer, maskLayer);
		Player.SetMaskLayer(maskLayer);
		Background.material.SetInt(PlayerMaterial.MaskLayer, maskLayer);
		int playerSortingOrder = Mathf.Max(Background.sortingOrder, MaskArea.sortingOrder) + 1;
		SortingGroup playerSortingGroup = Player.GetComponent<SortingGroup>();
		if (playerSortingGroup == null)
		{
			playerSortingGroup = Player.gameObject.AddComponent<SortingGroup>();
		}
		playerSortingGroup.sortingLayerID = Background.sortingLayerID;
		playerSortingGroup.sortingOrder = playerSortingOrder;
		SetPlayerCosmeticSorting(playerSortingOrder);
		int textSortingOrder = playerSortingOrder + 1;
		SetTextSorting(NameText, textSortingOrder);
		SetTextSorting(TextArea, textSortingOrder);
		SetTextSorting(ColorBlindName, textSortingOrder);
		NameText.fontMaterial.SetFloat("_StencilComp", (float)CompareFunction.Always);
		NameText.fontMaterial.SetFloat("_Stencil", maskLayer);
		TextArea.fontMaterial.SetFloat("_StencilComp", (float)CompareFunction.Always);
		TextArea.fontMaterial.SetFloat("_Stencil", maskLayer);
		ColorBlindName.fontMaterial.SetFloat("_StencilComp", (float)CompareFunction.Always);
		ColorBlindName.fontMaterial.SetFloat("_Stencil", maskLayer);
	}

	private void SetTextSorting(TextMeshPro text, int sortingOrder)
	{
		Renderer textRenderer = text.GetComponent<Renderer>();
		textRenderer.sortingLayerID = Background.sortingLayerID;
		textRenderer.sortingOrder = sortingOrder;
	}

	private void SetPlayerCosmeticSorting(int baseOrder)
	{
		CosmeticsLayer cosmetics = Player.Cosmetics;
		SpriteRenderer[] playerSprites = Player.GetComponentsInChildren<SpriteRenderer>(includeInactive: true);
		foreach (SpriteRenderer sprite in playerSprites)
		{
			SetPlayerSpriteSorting(sprite, baseOrder + 2);
		}

		PlayerBodySprite body = cosmetics.GetCurrentBodySprite();
		if (body != null)
		{
			SetPlayerSpriteSorting(body.BodySprite, baseOrder + 2);
			foreach (SpriteRenderer sprite in body.LongModeParts)
			{
				SetPlayerSpriteSorting(sprite, baseOrder + 2);
			}
		}

		if (cosmetics.skin != null)
		{
			SetPlayerSpriteSorting(cosmetics.skin.layer, baseOrder + 3);
		}
		if (cosmetics.hat != null)
		{
			SetPlayerSpriteSorting(cosmetics.hat.BackLayer, baseOrder + 1);
			SetPlayerSpriteSorting(cosmetics.hat.FrontLayer, baseOrder + 5);
		}
		if (cosmetics.visor != null)
		{
			bool behindHats = cosmetics.visor.visorData != null && cosmetics.visor.visorData.BehindHats;
			int visorOrder = behindHats ? baseOrder + 4 : baseOrder + 6;
			foreach (SpriteRenderer sprite in cosmetics.visor.GetComponentsInChildren<SpriteRenderer>(includeInactive: true))
			{
				SetPlayerSpriteSorting(sprite, visorOrder);
			}
		}
		if (cosmetics.CurrentPet != null)
		{
			foreach (SpriteRenderer sprite in cosmetics.CurrentPet.GetComponentsInChildren<SpriteRenderer>(includeInactive: true))
			{
				SetPlayerSpriteSorting(sprite, baseOrder + 7);
			}
		}
	}

	private void SetPlayerSpriteSorting(SpriteRenderer sprite, int sortingOrder)
	{
		if (sprite == null)
		{
			return;
		}
		sprite.sortingLayerID = Background.sortingLayerID;
		sprite.sortingOrder = sortingOrder;
	}
}
