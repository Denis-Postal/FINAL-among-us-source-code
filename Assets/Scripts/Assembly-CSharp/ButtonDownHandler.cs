using System.Collections;
using UnityEngine;

public class ButtonDownHandler : MonoBehaviour
{
	private Coroutine downState;

	public SpriteRenderer Target;

	public Sprite UpSprite;

	public Sprite DownSprite;

	public void Start()
	{
		GetComponent<PassiveButton>().OnClick.AddListener(StartDown);
	}

	public void OnDisable()
	{
		if (downState != null)
		{
			StopCoroutine(downState);
			downState = null;
			Target.sprite = UpSprite;
		}
	}

	private void StartDown()
	{
		if (downState == null)
		{
			downState = StartCoroutine(CoRunDown());
		}
	}

	private IEnumerator CoRunDown()
	{
		Target.sprite = DownSprite;
		while (DestroyableSingleton<PassiveButtonManager>.Instance.controller.AnyTouch)
		{
			yield return null;
		}
		Target.sprite = UpSprite;
		downState = null;
	}
}
