using System;
using System.Collections;
using UnityEngine;

public class CollectableOre : MonoBehaviour
{
	private const float WAIT_TIME_COLLECT = 0.3f;

	private const float WAIT_TIME_ROCK = 0.7f;

	[SerializeField]
	private SpriteRenderer spriteRenderer;

	[SerializeField]
	private BreakableRock[] rocks;

	[SerializeField]
	private Sprite[] oreSprites;

	[SerializeField]
	private Sprite[] rockSprites;

	[SerializeField]
	private AudioClip gemCollect;

	private int rockCount;

	private Action onCollect;

	private bool isRock;

	private Action onRockRelease;

	public void Initialize(Action collectOre, Action<CollectableOre, Vector3> powerAction, Controller control, Action rockReleased)
	{
		onCollect = (Action)Delegate.Combine(onCollect, collectOre);
		onRockRelease = (Action)Delegate.Combine(onRockRelease, rockReleased);
		BreakableRock[] array = rocks;
		for (int i = 0; i < array.Length; i++)
		{
			array[i].Initialize(powerAction, control);
		}
	}

	public BreakableRock[] GetRocks()
	{
		return rocks;
	}

	public bool GetIsRock()
	{
		return isRock;
	}

	public void RotateObjects()
	{
		BreakableRock[] array = rocks;
		foreach (BreakableRock breakableRock in array)
		{
			RotateObject(breakableRock.gameObject);
		}
		RotateObject(spriteRenderer.gameObject);
	}

	public void SetNumRocks(int numRocks)
	{
		rockCount = 0;
		for (int i = 0; i < numRocks; i++)
		{
			rockCount++;
			rocks[i].gameObject.SetActive(value: false);
		}
	}

	public void SetAsRock()
	{
		isRock = true;
		spriteRenderer.sprite = rockSprites.Random();
	}

	public void SetOreSprite(int index)
	{
		spriteRenderer.sprite = oreSprites[index];
	}

	public void BrokenRock()
	{
		rockCount++;
		if (rockCount >= rocks.Length)
		{
			onRockRelease?.Invoke();
			StartCoroutine(ShineCollect());
		}
	}

	public int GetNumberRocksBroken()
	{
		return rockCount;
	}

	private IEnumerator ShineCollect()
	{
		if (!isRock)
		{
			onCollect?.Invoke();
			yield return Effects.Wait(0.3f);
			yield return Effects.ScaleIn(spriteRenderer.transform, 1f, 1.2f, 0.3f);
			if (Constants.ShouldPlaySfx())
			{
				SoundManager.Instance.PlaySound(gemCollect, loop: false);
			}
			yield return Effects.ColorFade(spriteRenderer, Color.white, Palette.ClearWhite, 0.3f);
			gameObject.SetActive(value: false);
		}
		else
		{
			yield return Effects.Wait(0.7f);
			gameObject.SetActive(value: false);
		}
	}

	private void RotateObject(GameObject obj)
	{
		obj.transform.eulerAngles = new Vector3(obj.transform.eulerAngles.x, obj.transform.eulerAngles.y, UnityEngine.Random.Range(0, 360));
	}
}
