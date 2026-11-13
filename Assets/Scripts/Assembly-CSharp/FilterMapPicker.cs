using System;
using System.Collections.Generic;
using AmongUs.GameOptions;
using AmongUs.Matchmaking;
using TMPro;
using UnityEngine;

public class FilterMapPicker : GameOptionsMapPicker
{
	[SerializeField]
	private StringNames[] mapStrings;

	[SerializeField]
	private TextMeshPro mapNames;

	private const string COMMA_SPACE_TEXT = ", ";

	private List<int> mapIDs;

	public override void Initialize(int maskLayer)
	{
		mapIDs = new List<int>();
		IGameOptions gameSearchOptions = GameOptionsManager.Instance.GameSearchOptions;
		selectedMapId = gameSearchOptions.MapId;
		SpriteRenderer[] componentsInChildren = GetComponentsInChildren<SpriteRenderer>(includeInactive: true);
		for (int i = 0; i < componentsInChildren.Length; i++)
		{
			componentsInChildren[i].material.SetInt(PlayerMaterial.MaskLayer, maskLayer);
		}
		Labeltext.fontMaterial.SetFloat("_StencilComp", 3f);
		Labeltext.fontMaterial.SetFloat("_Stencil", maskLayer);
		SetupMapButtons(maskLayer);
	}

	protected override void UpdateValue()
	{
		if (optionName != ByteOptionNames.Invalid)
		{
			GameOptionsManager.Instance.GameSearchOptions.SetByte(optionName, (byte)GetInt());
		}
	}

	public bool IsFilterEnabled()
	{
		foreach (MapSelectButton mapButton in mapButtons)
		{
			if (mapButton.Button.IsSelected())
			{
				return true;
			}
		}
		return false;
	}

	public void SetupSelectedIcons(MapGameFilter gameFilter)
	{
		if (gameFilter != null)
		{
			byte acceptedValues = gameFilter.AcceptedValues;
			foreach (MapSelectButton mapButton in mapButtons)
			{
				if ((acceptedValues & (1 << mapButton.MapID)) != 0)
				{
					mapIDs.Add(mapButton.MapID);
					mapButton.Button.SelectButton(isSelected: true);
				}
				else
				{
					mapButton.Button.SelectButton(isSelected: false);
				}
			}
		}
		else
		{
			foreach (MapSelectButton mapButton2 in mapButtons)
			{
				mapButton2.Button.SelectButton(isSelected: false);
			}
		}
		SetupMapNameString();
	}

	private void AddMapName(int mapID)
	{
		if (!mapIDs.Contains(mapID))
		{
			mapIDs.Add(mapID);
		}
		SetupMapNameString();
	}

	private void RemoveMapName(int mapID)
	{
		if (mapIDs.Contains(mapID))
		{
			mapIDs.Remove(mapID);
		}
		SetupMapNameString();
	}

	private void ClearMapNames()
	{
		mapIDs.Clear();
		mapNames.text = string.Empty;
	}

	private void SetupMapNameString()
	{
		mapIDs.Sort();
		mapNames.text = string.Empty;
		if (mapIDs.Count > 0)
		{
			for (int i = 0; i < mapIDs.Count; i++)
			{
				if ((i == 0 && mapIDs.Count == 1) || i + 1 >= mapIDs.Count)
				{
					mapNames.text += DestroyableSingleton<TranslationController>.Instance.GetString(mapStrings[mapIDs[i]]);
					break;
				}
				TextMeshPro textMeshPro = mapNames;
				textMeshPro.text = textMeshPro.text + DestroyableSingleton<TranslationController>.Instance.GetString(mapStrings[mapIDs[i]]) + ", ";
			}
		}
		else
		{
			mapNames.text += DestroyableSingleton<TranslationController>.Instance.GetString(StringNames.FilterNoMaps);
		}
	}

	public void SetupClickAction(Action checkFilterLimit = null)
	{
		foreach (MapSelectButton mapButton in mapButtons)
		{
			mapButton.Button.OnClick.RemoveAllListeners();
			mapButton.Button.OnClick.AddListener(() =>
			{
				mapButton.Button.SelectButton(!mapButton.Button.IsSelected());
				mapButton.Button.SetPassiveButtonHoverStateActive();
				if (mapButton.Button.IsSelected())
				{
					AddMapName(mapButton.MapID);
				}
				else
				{
					RemoveMapName(mapButton.MapID);
				}
				byte b = 0;
				foreach (MapSelectButton mapButton2 in mapButtons)
				{
					if (mapButton2.Button.IsSelected())
					{
						b = (byte)(b + (1 << mapButton2.MapID));
					}
				}
				if (b == 0)
				{
					DestroyableSingleton<FindAGameManager>.Instance.RemoveMapFilter();
					RemoveMapName(mapButton.MapID);
				}
				else
				{
					DestroyableSingleton<FindAGameManager>.Instance.SetMapFilter(b);
				}
				checkFilterLimit?.Invoke();
			});
		}
	}

	public List<MapSelectButton> GetMapButtons()
	{
		return mapButtons;
	}

	public void SetupMouseOver(Action<int> action)
	{
		foreach (MapSelectButton mapButton in mapButtons)
		{
			mapButton.Button.OnClick.AddListener(() =>
			{
				action(1);
			});
			mapButton.Button.OnMouseOver.AddListener(() =>
			{
				action(1);
			});
			mapButton.Button.OnMouseOut.AddListener(() =>
			{
				action(0);
			});
		}
	}
}
