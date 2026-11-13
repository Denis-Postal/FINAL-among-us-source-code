using System.Collections.Generic;
using Cpp2ILInjected;
using TMPro;
using UnityEngine;

[RequireComponent(typeof(TextMeshPro))]
public class PlatformTextTranslationTMP : MonoBehaviour, ITranslatedText
{
	public StringNames DefaultTargetText;

	public StringNames SwitchTargetText;

	public StringNames XboxTargetText;

	public StringNames PlaystationTargetText;

	public StringNames MobileTargetText;

	public void ResetText()
	{
		TextMeshPro component = GetComponent<TextMeshPro>();
		component.text = DestroyableSingleton<TranslationController>.Instance.GetString(DefaultTargetText);
		component.ForceMeshUpdate();
	}

	public void Start()
	{
		// TODO(verify): reimplemented (decomp stub)
		DestroyableSingleton<TranslationController>.Instance.ActiveTexts.Add(this);
		ResetText();
	}

	public void OnDestroy()
	{
		if (DestroyableSingleton<TranslationController>.InstanceExists)
		{
			try
			{
				DestroyableSingleton<TranslationController>.Instance.ActiveTexts.Remove(this);
			}
			catch
			{
			}
		}
	}
}
