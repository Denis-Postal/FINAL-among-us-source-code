using Object = UnityEngine.Object;
using System;
using Cpp2ILInjected;
using TMPro;
using UnityEngine;

public class ViewSettingsInfoPanel : MonoBehaviour
{
	[SerializeField]
	protected SpriteRenderer labelBackground;

	[SerializeField]
	protected SpriteRenderer background;

	[SerializeField]
	protected SpriteRenderer disabledBackground;

	[SerializeField]
	protected SpriteRenderer checkMark;

	[SerializeField]
	protected SpriteRenderer checkMarkOff;

	[SerializeField]
	protected TextMeshPro titleText;

	[SerializeField]
	protected TextMeshPro settingText;

	public void SetInfo(StringNames title, string valueString, int maskLayer)
	{
		titleText.text = DestroyableSingleton<TranslationController>.Instance.GetString(title);
		settingText.text = valueString;
		disabledBackground.gameObject.SetActive(value: false);
		background.gameObject.SetActive(value: true);
		SetMaskLayer(maskLayer);
	}

	public void SetInfoCheckbox(StringNames title, int maskLayer, bool isOn)
	{
		titleText.text = DestroyableSingleton<TranslationController>.Instance.GetString(title);
		settingText.text = string.Empty;
		checkMarkOff.gameObject.SetActive(!isOn);
		background.gameObject.SetActive(value: true);
		checkMark.gameObject.SetActive(isOn);
		SetMaskLayer(maskLayer);
	}

	protected virtual void SetMaskLayer(int maskLayer)
	{
		background.material.SetInt(PlayerMaterial.MaskLayer, maskLayer);
		disabledBackground.material.SetInt(PlayerMaterial.MaskLayer, maskLayer);
		checkMarkOff.material.SetInt(PlayerMaterial.MaskLayer, maskLayer);
		labelBackground.material.SetInt(PlayerMaterial.MaskLayer, maskLayer);
		checkMark.material.SetInt(PlayerMaterial.MaskLayer, maskLayer);
		settingText.fontMaterial.SetFloat("_StencilComp", 3f);
		settingText.fontMaterial.SetFloat("_Stencil", maskLayer);
		titleText.fontMaterial.SetFloat("_StencilComp", 3f);
		titleText.fontMaterial.SetFloat("_Stencil", maskLayer);
	}
}
