using System.Collections.Generic;
using AmongUs.GameOptions;
using TMPro;
using UnityEngine;

public class GameOptionsMapPicker : OptionBehaviour
{
	[SerializeField]
	protected List<MapIconByName> AllMapIcons;

	[SerializeField]
	protected float StartPosX;

	[SerializeField]
	protected float SpacingX;

	[SerializeField]
	protected float MapButtonY = 0.74f;

	[SerializeField]
	private SpriteRenderer MapImage;

	[SerializeField]
	private SpriteRenderer MapName;

	[SerializeField]
	protected MapSelectButton MapButtonOrigin;

	[SerializeField]
	protected Collider2D ButtonClickMask;

	[SerializeField]
	protected TextMeshPro Labeltext;

	protected List<MapSelectButton> mapButtons;

	protected MapSelectButton selectedButton;

	protected int selectedMapId;

	private int oldValue = -1;

	protected ByteOptionNames optionName;

	public List<MapSelectButton> MapButtons => mapButtons;

	public override void SetUpFromData(BaseGameSetting data, int maskLayer)
	{
		base.SetUpFromData(data, maskLayer);
		MapSelectionGameSetting mapSelectionGameSetting = data as MapSelectionGameSetting;
		if (!(mapSelectionGameSetting == null))
		{
			optionName = mapSelectionGameSetting.OptionName;
		}
	}

	public virtual void Initialize(int maskLayer)
	{
		IGameOptions currentGameOptions = GameOptionsManager.Instance.CurrentGameOptions;
		selectedMapId = currentGameOptions.MapId;
		SpriteRenderer[] componentsInChildren = GetComponentsInChildren<SpriteRenderer>(includeInactive: true);
		for (int i = 0; i < componentsInChildren.Length; i++)
		{
			componentsInChildren[i].material.SetInt(PlayerMaterial.MaskLayer, maskLayer);
		}
		Labeltext.fontMaterial.SetFloat("_StencilComp", 3f);
		Labeltext.fontMaterial.SetFloat("_Stencil", maskLayer);
		SetupMapButtons(maskLayer);
	}

	protected void SetupMapButtons(int maskLayer)
	{
		if (mapButtons != null)
		{
			for (int i = 0; i < mapButtons.Count; i++)
			{
				Object.Destroy(mapButtons[i].gameObject);
			}
		}
		mapButtons = new List<MapSelectButton>();
		for (int j = 0; j < AllMapIcons.Count; j++)
		{
			MapIconByName thisVal = AllMapIcons[j];
			MapSelectButton mapButton = Object.Instantiate(MapButtonOrigin, Vector3.zero, Quaternion.identity, transform);
			mapButton.SetImage(thisVal.MapIcon, maskLayer);
			mapButton.transform.localPosition = new Vector3(StartPosX + (float)j * SpacingX, MapButtonY, -2f);
			mapButton.Button.ClickMask = ButtonClickMask;
			mapButton.MapID = (int)thisVal.Name;
			mapButton.Button.OnClick.AddListener(() =>
			{
				if ((bool)selectedButton)
				{
					selectedButton.Button.SelectButton(isSelected: false);
				}
				selectedButton = mapButton;
				selectedButton.Button.SelectButton(isSelected: true);
				SelectMap(thisVal);
			});
			if (j > 0)
			{
				mapButton.Button.ControllerNav.selectOnLeft = mapButtons[j - 1].Button;
				mapButtons[j - 1].Button.ControllerNav.selectOnRight = mapButton.Button;
			}
			mapButtons.Add(mapButton);
			if (thisVal.Name == (MapNames)selectedMapId)
			{
				mapButton.Button.SelectButton(isSelected: true);
				SelectMap(selectedMapId);
				selectedButton = mapButton;
			}
		}
	}

	public void SetInactiveButton(bool isEnabled)
	{
		bool flag = true;
		foreach (MapSelectButton mapButton in mapButtons)
		{
			if (mapButton.Button.IsSelected())
			{
				flag = false;
			}
		}
		if (!flag && !isEnabled)
		{
			return;
		}
		foreach (MapSelectButton mapButton2 in mapButtons)
		{
			if (!mapButton2.Button.IsSelected())
			{
				mapButton2.Button.SetButtonEnableState(isEnabled);
			}
		}
	}

	private void FixedUpdate()
	{
		if (oldValue != selectedMapId)
		{
			oldValue = selectedMapId;
			SelectMap(selectedMapId);
		}
	}

	public void SelectMap(int mapId)
	{
		selectedMapId = mapId;
		if ((bool)MapImage)
		{
			MapImage.sprite = AllMapIcons.Find((MapIconByName m) => m.Name == (MapNames)mapId).MapImage;
		}
		if ((bool)MapName)
		{
			MapName.sprite = AllMapIcons.Find((MapIconByName m) => m.Name == (MapNames)mapId).NameImage;
		}
		if (mapId == 0 && AprilFoolsMode.ShouldFlipSkeld())
		{
			MapName.flipX = true;
		}
		else
		{
			MapName.flipX = false;
		}
	}

	public void SelectMap(MapIconByName mapInfo)
	{
		selectedMapId = (int)mapInfo.Name;
		if ((bool)MapImage)
		{
			MapImage.sprite = mapInfo.MapImage;
		}
		if (mapInfo.Name == MapNames.Skeld && AprilFoolsMode.ShouldFlipSkeld())
		{
			MapName.flipX = true;
		}
		else
		{
			MapName.flipX = false;
		}
		if ((bool)MapName)
		{
			MapName.sprite = mapInfo.NameImage;
		}
		UpdateValue();
		OnValueChanged?.Invoke(this);
	}

	public override float GetFloat()
	{
		return selectedMapId;
	}

	public override int GetInt()
	{
		return selectedMapId;
	}

	protected virtual void UpdateValue()
	{
		if (optionName != ByteOptionNames.Invalid)
		{
			GameOptionsManager.Instance.CurrentGameOptions.SetByte(optionName, (byte)GetInt());
		}
	}
}
