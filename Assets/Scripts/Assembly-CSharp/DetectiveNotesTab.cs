using System.Collections.Generic;
using AmongUs.Data;
using TMPro;
using UnityEngine;

public class DetectiveNotesTab : MonoBehaviour
{
	private int tabIndex;

	[SerializeField]
	private BoxCollider2D collider;

	[SerializeField]
	private PoolablePlayer playerIcon;

	[SerializeField]
	private List<SpriteRenderer> backgrounds;

	[SerializeField]
	private SpriteRenderer playerColorBackground;

	[SerializeField]
	private PassiveButton passiveButton;

	[SerializeField]
	private SpriteRenderer tabBG;

	[SerializeField]
	private SpriteRenderer dimBG;

	[SerializeField]
	private Sprite largeBG;

	[SerializeField]
	private Sprite smallBG;

	[SerializeField]
	private TMP_Text colorblindText;

	private float tabSpacing = 1.6f;

	private float smallColliderSizeX = 0.87f;

	private float smallPlayerPosX;

	private float smallPlayerScale = 0.5f;

	private Vector2 smallBackgroundSize = new Vector2(2.18f, 1.43f);

	private Vector2 smallColorSize = new Vector2(1.29f, 1.2f);

	private int maskLayer = 30;

	public void Setup(DetectiveNotesMinigame notesMinigame, int _tabIndex, NetworkedPlayerInfo player, int amountOfTabs)
	{
		tabIndex = _tabIndex;
		playerColorBackground.color = player.Color;
		passiveButton.OnClick.AddListener(() =>
		{
			notesMinigame.SetPage(tabIndex);
		});
		passiveButton.OnClick.AddListener(() =>
		{
			SetHighlighted(notesMinigame, highlighted: true);
		});
		if (amountOfTabs > 6)
		{
			collider.size = new Vector2(smallColliderSizeX, collider.size.y);
			tabSpacing = 0.9f;
			playerColorBackground.size = smallColorSize;
			playerIcon.transform.localPosition = new Vector3(smallPlayerPosX, playerIcon.transform.localPosition.y, playerIcon.transform.localPosition.z);
			playerIcon.transform.localScale = Vector3.one * smallPlayerScale;
			foreach (SpriteRenderer background in backgrounds)
			{
				background.size = smallBackgroundSize;
			}
		}
		transform.localPosition += new Vector3(tabSpacing * (float)_tabIndex, 0f);
		playerColorBackground.material.SetInt(PlayerMaterial.MaskLayer, maskLayer + _tabIndex);
		colorblindText.text = (DataManager.Settings.Accessibility.ColorBlindMode ? player.GetPlayerColorString() : string.Empty);
		playerIcon.SetBodyColor(player.Outfits[PlayerOutfitType.Default].ColorId);
		playerIcon.SetMaskLayer(maskLayer + _tabIndex);
		playerIcon.Cosmetics.SetMaskType(PlayerMaterial.MaskType.ComplexUI);
	}

	public void SetHighlighted(DetectiveNotesMinigame minigame, bool highlighted)
	{
		if (highlighted)
		{
			minigame.ClearTabHighlights();
			dimBG.enabled = false;
			transform.localPosition = new Vector3(transform.localPosition.x, transform.localPosition.y, -1f);
		}
		else
		{
			dimBG.enabled = true;
			transform.localPosition = new Vector3(transform.localPosition.x, transform.localPosition.y, 1f);
		}
	}
}
