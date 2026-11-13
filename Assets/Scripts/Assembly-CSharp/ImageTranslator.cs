using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class ImageTranslator : MonoBehaviour, ITranslatedText
{
	public ImageNames TargetImage;

	public void ResetText()
	{
		Sprite image = DestroyableSingleton<TranslationController>.Instance.GetImage(TargetImage);
		if ((bool)image)
		{
			GetComponent<SpriteRenderer>().sprite = image;
		}
	}

	public void Start()
	{ /* spliced-from-2019 */
		DestroyableSingleton<TranslationController>.Instance.ActiveTexts.Add(this);
		ResetText();
	}

	public void OnDestroy()
	{
		if (DestroyableSingleton<TranslationController>.InstanceExists)
		{
			DestroyableSingleton<TranslationController>.Instance.ActiveTexts.Remove(this);
		}
	}
}
