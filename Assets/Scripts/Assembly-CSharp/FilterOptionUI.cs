using System;
using System.Collections.Generic;
using AmongUs.Matchmaking;
using TMPro;
using UnityEngine;

public class FilterOptionUI : MonoBehaviour
{
	public enum ButtonSize
	{
		Short,
		Med,
		Long
	}

	public FilterPopUp.FilterInfoUI FilterInfo;

	public Action<int, int, string> toolTipCallback;

	public int HighlightIndex;

	[SerializeField]
	private List<PassiveButton> currentButtons;

	[SerializeField]
	private PassiveButton[] buttonPrefabs;

	[SerializeField]
	private TextMeshPro label;

	[SerializeField]
	private SpriteRenderer blackSquare;

	private const int MASK_LAYER = 51;

	private const float SHORT_DISTANCE = 0.55f;

	private const float X_START_SHORT = -0.65f;

	private const float MED_DISTANCE = 1.05f;

	private const float X_START_MED = 0f;

	private const float LONG_DISTANCE = 2f;

	private const float X_START_LONG = 0.11f;

	private Action<bool, int, FilterPopUp.FilterInfoUI> onClickAction;

	private GameFilter gameFilter;

	private static readonly int STENCIL_COMP = Shader.PropertyToID("_StencilComp");

	private static readonly int STENCIL = Shader.PropertyToID("_Stencil");

	public void SetupButtons(Action<bool, int, FilterPopUp.FilterInfoUI> clickAction, Collider2D clickMask)
	{
		onClickAction = clickAction;
		if (currentButtons != null && currentButtons.Count > 0)
		{
			foreach (PassiveButton currentButton in currentButtons)
			{
				UnityEngine.Object.Destroy(currentButton.gameObject);
			}
		}
		currentButtons = new List<PassiveButton>();
		PassiveButton original;
		float num;
		float num2;
		switch (FilterInfo.ButtonSize)
		{
		case ButtonSize.Short:
			original = buttonPrefabs[0];
			num = 0.55f;
			num2 = -0.65f;
			break;
		case ButtonSize.Med:
			original = buttonPrefabs[1];
			num = 1.05f;
			num2 = 0f;
			break;
		default:
			original = buttonPrefabs[2];
			num = 2f;
			num2 = 0.11f;
			break;
		}
		label.text = DestroyableSingleton<TranslationController>.Instance.GetString(FilterInfo.stringName);
		gameFilter = null;
		foreach (GameFilter filter in DestroyableSingleton<FindAGameManager>.Instance.GetFilterSet().Filters)
		{
			if (filter.Key == FilterInfo.myFilterName.ToString())
			{
				gameFilter = filter;
				break;
			}
		}
		for (int i = 0; i < FilterInfo.ButtonCount; i++)
		{
			PassiveButton filterB = UnityEngine.Object.Instantiate(original, Vector3.zero, Quaternion.identity, transform);
			filterB.transform.localPosition = new Vector3(num2, 0f, -2f);
			currentButtons.Add(filterB);
			num2 += num;
			filterB.ChangeButtonText(FilterInfo.buttonTexts[i]);
			int index = i;
			filterB.ClickMask = clickMask;
			filterB.OnClick.AddListener(() =>
			{
				HandleSelection(index);
				onClickAction?.Invoke(filterB.IsSelected(), index, FilterInfo);
				toolTipCallback?.Invoke(FilterInfo.tooltipIndex, HighlightIndex, FilterInfo.ExtraInfoString);
			});
			filterB.OnMouseOut.AddListener(() =>
			{
				toolTipCallback?.Invoke(0, -1, string.Empty);
			});
			filterB.OnMouseOver.AddListener(() =>
			{
				toolTipCallback?.Invoke(FilterInfo.tooltipIndex, HighlightIndex, FilterInfo.ExtraInfoString);
			});
			if (gameFilter == null)
			{
				continue;
			}
			switch (FilterInfo.GetFilterType())
			{
			case "bool":
				if (gameFilter.SubFilter is BoolGameFilter boolGameFilter && boolGameFilter.AcceptedValues.Contains(index == 0))
				{
					filterB.SelectButton(isSelected: true);
					filterB.SetPassiveButtonHoverStateInactive();
				}
				else
				{
					filterB.SelectButton(isSelected: false);
				}
				break;
			case "cat":
				if (gameFilter.SubFilter is CategorizedGameFilter categorizedGameFilter && categorizedGameFilter.AcceptedValues.Contains(index))
				{
					filterB.SelectButton(isSelected: true);
					filterB.SetPassiveButtonHoverStateInactive();
				}
				else
				{
					filterB.SelectButton(isSelected: false);
				}
				break;
			case "int":
				if (gameFilter.SubFilter is IntGameFilter intGameFilter2 && intGameFilter2.AcceptedValues.Contains(index + 1))
				{
					filterB.SelectButton(isSelected: true);
					filterB.SetPassiveButtonHoverStateInactive();
				}
				else
				{
					filterB.SelectButton(isSelected: false);
				}
				break;
			case "int0":
				if (gameFilter.SubFilter is IntGameFilter intGameFilter && intGameFilter.AcceptedValues.Contains(index))
				{
					filterB.SelectButton(isSelected: true);
					filterB.SetPassiveButtonHoverStateInactive();
				}
				else
				{
					filterB.SelectButton(isSelected: false);
				}
				break;
			}
		}
		SetupButtonMaskLayer();
		SetupNav();
	}

	public SpriteRenderer GetBlackSquare()
	{
		return blackSquare;
	}

	public void ResetFilterSelection()
	{
		for (int i = 0; i < currentButtons.Count; i++)
		{
			currentButtons[i].SelectButton(isSelected: false);
		}
	}

	public List<PassiveButton> GetButtons()
	{
		return currentButtons;
	}

	public void SetupButtonMaskLayer()
	{
		foreach (PassiveButton currentButton in currentButtons)
		{
			currentButton.SetMaskLayer(51);
		}
		label.fontMaterial.SetFloat(STENCIL_COMP, 3f);
		label.fontMaterial.SetFloat(STENCIL, 51f);
	}

	private void SetupNav()
	{
		for (int i = 0; i < currentButtons.Count; i++)
		{
			if (i + 1 < currentButtons.Count)
			{
				currentButtons[i].ControllerNav.selectOnRight = currentButtons[i + 1];
			}
			if (i - 1 >= 0)
			{
				currentButtons[i].ControllerNav.selectOnLeft = currentButtons[i - 1];
			}
		}
	}

	private void HandleSelection(int index)
	{
		currentButtons[index].SelectButton(!currentButtons[index].IsSelected());
		currentButtons[index].SetPassiveButtonHoverStateActive();
	}

	public void SetInactiveButton(bool isEnabled)
	{
		bool flag = true;
		foreach (PassiveButton currentButton in currentButtons)
		{
			if (currentButton.IsSelected())
			{
				flag = false;
			}
		}
		if (!flag && !isEnabled)
		{
			return;
		}
		foreach (PassiveButton currentButton2 in currentButtons)
		{
			if (!currentButton2.IsSelected())
			{
				currentButton2.SetButtonEnableState(isEnabled);
			}
		}
	}

	public bool isFilterActive()
	{
		foreach (PassiveButton currentButton in currentButtons)
		{
			if (currentButton.IsSelected())
			{
				return true;
			}
		}
		return false;
	}
}
