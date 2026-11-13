using System.Collections;
using TMPro;
using UnityEngine;

public class WeatherControl : MonoBehaviour
{
	public Sprite backgroundLight;

	public Sprite backgroundDark;

	public Sprite lightOff;

	public Sprite lightOn;

	public SpriteRenderer Background;

	public SpriteRenderer Switch;

	public SpriteRenderer Light;

	public TextMeshPro Label;

	internal void SetInactive()
	{
		Light.sprite = lightOff;
		Label.color = new Color32(146, 135, 163, byte.MaxValue);
		StartCoroutine(Run());
		Switch.flipX = true;
	}

	public void SetActive()
	{
		StopAllCoroutines();
		Label.color = new Color32(81, 53, 115, byte.MaxValue);
		Background.sprite = backgroundLight;
		Light.sprite = lightOn;
		Switch.flipX = false;
	}

	private IEnumerator Run()
	{
		while (true)
		{
			Background.sprite = backgroundDark;
			yield return Effects.Wait(0.5f);
			Background.sprite = backgroundLight;
			yield return Effects.Wait(0.5f);
		}
	}
}
