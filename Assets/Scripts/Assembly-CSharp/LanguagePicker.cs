using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class LanguagePicker : MonoBehaviour
{
	[SerializeField]
	private ObjectPoolBehavior ButtonPool;

	[SerializeField]
	private TextMeshPro label;

	public Action<bool> callbackTooltip;

	public Action<uint> callbackLang;

	private const float INITIAL_X_POS = 0f;

	private const float INITIAL_Y_POS = 0f;

	private const float XOFFSET = 2.1f;

	private const float YOFFSET = -0.65f;

	private const string OTHER_STRING = "Other";

	private const int NUM_COLUMNS = 3;

	private List<LanguageOption> langOptions;

	private const int MASK_LAYER = 51;

	private static readonly int STENCIL_COMP = Shader.PropertyToID("_StencilComp");

	private static readonly int STENCIL = Shader.PropertyToID("_Stencil");

	public void SetupLanguageButtons(uint sel)
	{
		ButtonPool.ReclaimAll();
		langOptions = new List<LanguageOption>();
		int num = 0;
		float num2 = 0f;
		float num3 = 0f;
		foreach (KeyValuePair<string, uint> language in ChatLanguageSet.Instance.Languages)
		{
			uint lang = language.Value;
			LanguageOption button = ButtonPool.Get<LanguageOption>();
			button.transform.localPosition = new Vector3(num3, num2, -1f);
			if (language.Key == "Other")
			{
				button.SetTextButton(DestroyableSingleton<TranslationController>.Instance.GetString(StringNames.OtherLanguage));
			}
			else
			{
				button.SetTextButton(language.Key);
			}
			button.Button.OnClick.RemoveAllListeners();
			button.Button.OnClick.AddListener(() =>
			{
				foreach (LanguageOption langOption in langOptions)
				{
					langOption.SetButtonSelected(selected: false);
				}
				button.SetButtonSelected(selected: true);
				ChooseOption(lang);
				callbackTooltip?.Invoke(obj: true);
			});
			button.Button.OnMouseOver.AddListener(() =>
			{
				callbackTooltip?.Invoke(obj: true);
			});
			button.Button.OnMouseOut.AddListener(() =>
			{
				callbackTooltip?.Invoke(obj: false);
			});
			button.Button.SelectButton(language.Value == sel);
			button.Button.SetPassiveButtonHoverStateInactive();
			langOptions.Add(button);
			num++;
			if (num % 3 == 0)
			{
				num3 = 0f;
				num2 += -0.65f;
			}
			else
			{
				num3 += 2.1f;
			}
		}
	}

	public List<LanguageOption> GetLangButtons()
	{
		return langOptions;
	}

	private void ChooseOption(uint lang)
	{
		callbackLang?.Invoke(lang);
	}

	public void SetupMaskLayer()
	{
		foreach (LanguageOption langOption in langOptions)
		{
			langOption.Button.SetMaskLayer(51);
		}
		label.fontMaterial.SetFloat(STENCIL_COMP, 3f);
		label.fontMaterial.SetFloat(STENCIL, 51f);
	}
}
