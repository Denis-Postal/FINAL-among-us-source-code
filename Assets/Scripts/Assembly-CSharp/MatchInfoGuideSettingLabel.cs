using Cpp2ILInjected;
using TMPro;
using UnityEngine;

public class MatchInfoGuideSettingLabel : MonoBehaviour
{
	[SerializeField]
	private TextMeshPro settingLabel;

	[SerializeField]
	private TextMeshPro settingValue;

	[SerializeField]
	private SpriteRenderer[] sprites;

	private readonly string STENCIL_NAME= "_Stencil";

	public void SetInfo(string label, string value)
	{
		settingLabel.text = label;
		settingValue.text = value;
		settingLabel.fontMaterial.SetFloat(STENCIL_NAME, 50f);
		settingValue.fontMaterial.SetFloat(STENCIL_NAME, 50f);
		SpriteRenderer[] array = sprites;
		for (int i = 0; i < array.Length; i++)
		{
			array[i].material.SetInt(PlayerMaterial.MaskLayer, 50);
		}
	}

	public MatchInfoGuideSettingLabel()
	{ }
}
