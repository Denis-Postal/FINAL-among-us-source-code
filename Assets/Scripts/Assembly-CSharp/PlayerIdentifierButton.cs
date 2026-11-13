using InnerNet;
using TMPro;
using UnityEngine;

public class PlayerIdentifierButton : PoolableBehavior
{
	[SerializeField]
	public TextMeshPro NameText;

	[SerializeField]
	private PlatformIdentifier PlatformIdentifier;

	[SerializeField]
	public PoolablePlayer PlayerPreview;

	[SerializeField]
	private SpriteRenderer MaskArea;

	[SerializeField]
	private SpriteRenderer buttonSprite;

	[SerializeField]
	private PassiveButton button;

	[SerializeField]
	private SpriteRenderer highlight;

	[SerializeField]
	private SpriteRenderer platformIcon;

	[SerializeField]
	private TextMeshPro platformText;

	private byte targetPlayerId;

	private static readonly int STENCIL_COMP = Shader.PropertyToID("_StencilComp");

	private static readonly int STENCIL = Shader.PropertyToID("_Stencil");

	public PassiveButton Button => button;

	public void SetTextStencil(int MASK_LAYER)
	{
		NameText.fontMaterial.SetFloat(STENCIL_COMP, 3f);
		NameText.fontMaterial.SetFloat(STENCIL, MASK_LAYER);
		PlayerPreview.SetMaskLayer(MASK_LAYER);
		MaskArea.material.SetInt(PlayerMaterial.MaskLayer, MASK_LAYER);
		buttonSprite.material.SetInt(PlayerMaterial.MaskLayer, MASK_LAYER);
		highlight.material.SetInt(PlayerMaterial.MaskLayer, MASK_LAYER);
		if ((bool)platformText)
		{
			platformText.fontMaterial.SetFloat(STENCIL_COMP, 3f);
			platformText.fontMaterial.SetFloat(STENCIL, MASK_LAYER);
		}
		if ((bool)platformIcon)
		{
			platformIcon.material.SetInt(PlayerMaterial.MaskLayer, MASK_LAYER);
		}
	}

	public void Populate(NetworkedPlayerInfo player)
	{
		NameText.text = player.PlayerName;
		SetTargetPlayerId(player.PlayerId);
		PlayerPreview.UpdateFromPlayerData(player, PlayerOutfitType.Default, PlayerMaterial.MaskType.ComplexUI, includePet: false, null, forceAlive: true);
		PlayerPreview.ToggleName(active: false);
		button.enabled = false;
	}

	private void SetTargetPlayerId(byte targetId)
	{
		targetPlayerId = targetId;
		if ((bool)PlayerPreview)
		{
			SetMaskLayer(targetId);
		}
		if ((bool)PlatformIdentifier)
		{
			PlayerControl playerControl = PlayerControl.AllPlayerControls.Find((PlayerControl pc) => pc.PlayerId == targetId);
			if ((bool)playerControl)
			{
				ClientData client = AmongUsClient.Instance.GetClient(playerControl.OwnerId);
				PlatformIdentifier.SetInfo(client);
			}
			else
			{
				PlatformIdentifier.SetInfo(null);
			}
		}
	}

	private void SetMaskLayer(int maskLayer)
	{
		PlayerPreview.SetMaskLayer(maskLayer + 2);
		SpriteRenderer[] componentsInChildren = GetComponentsInChildren<SpriteRenderer>();
		for (int i = 0; i < componentsInChildren.Length; i++)
		{
			componentsInChildren[i].material.SetInt(PlayerMaterial.MaskLayer, maskLayer + 2);
		}
	}

	public void DisplayProfileInfo()
	{
		PlayerControl playerControl = PlayerControl.AllPlayerControls.Find((PlayerControl pc) => pc.PlayerId == targetPlayerId);
		if ((bool)playerControl)
		{
			AmongUsClient.Instance.GetClient(playerControl.OwnerId);
		}
	}

	public override void Reset()
	{
		base.Reset();
		if ((bool)PlatformIdentifier)
		{
			PlatformIdentifier.SetInfo(null);
		}
	}
}
