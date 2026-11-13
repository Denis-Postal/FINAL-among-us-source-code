using System;
using System.Collections;
using UnityEngine;

public class EggCoveringLeaf : MonoBehaviour
{
	[SerializeField]
	private PassiveButton leafButton;

	[SerializeField]
	private SpriteRenderer leafSprite;

	public event Action OnCollectedEvent;

	private void Start()
	{
		leafButton.OnClick.AddListener(() =>
		{
			StartCoroutine(CoCollectLeaf());
		});
	}

	private IEnumerator CoCollectLeaf()
	{
		leafButton.SetButtonEnableState(enabled: false);
		OnCollectedEvent?.Invoke();
		float num = ((!BoolRange.Next()) ? 1 : (-1)) * UnityEngine.Random.Range(30, 50);
		float z = transform.localEulerAngles.z;
		yield return Effects.All(Effects.ColorFade(leafSprite, Color.white, Palette.ClearWhite, 0.5f), Effects.Rotate2D(transform, z, z + num, 0.5f));
	}
}
