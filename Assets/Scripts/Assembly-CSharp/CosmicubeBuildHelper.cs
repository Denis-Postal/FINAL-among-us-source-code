using System.Collections;
using UnityEngine;

[ExecuteInEditMode]
public class CosmicubeBuildHelper : MonoBehaviour
{
	public enum CubeSide
	{
		East,
		West,
		North,
		South
	}

	public CosmicubeCell cell;

	public CubeSide cubeSide;

	public GameObject connector;

	private bool childOwned;

	private bool animating;

	public void AnimateUnlock(float duration)
	{
		StartCoroutine(CoAnimateUnlock(duration));
	}

	private IEnumerator CoAnimateUnlock(float duration)
	{
		animating = true;
		bool flag = cubeSide == CubeSide.East || cubeSide == CubeSide.South;
		Vector3 startPosition = new Vector3(0f, flag ? 0.25f : (-0.25f), -0.01f);
		Vector3 startScale = new Vector3(1f, 0f, 1f);
		Vector3 endPosition = new Vector3(0f, 0f, -0.01f);
		Vector3 endScale = Vector3.one;
		Transform green = connector.transform.Find("Green");
		green.gameObject.SetActive(value: true);
		green.localScale = startScale;
		green.localPosition = startPosition;
		yield return Effects.Lerp(duration, (float t) =>
		{
			green.localScale = Vector3.Lerp(startScale, endScale, t);
			green.localPosition = Vector3.Lerp(startPosition, endPosition, t);
		});
		animating = false;
	}

	private void Update()
	{
		if (animating)
		{
			return;
		}
		if ((bool)cell.CosmeticData && !Application.isPlaying)
		{
			cell.qualitySprite.color = CosmicubeManager.qualityColorMap[cell.CosmeticData.quality];
		}
		switch (cubeSide)
		{
		case CubeSide.East:
			if (cell.eastChild != null)
			{
				connector.SetActive(value: true);
				if (cell.eastChild.unlocked)
				{
					childOwned = true;
				}
			}
			else
			{
				connector.SetActive(value: false);
			}
			break;
		case CubeSide.West:
			if (cell.westChild != null)
			{
				connector.SetActive(value: true);
				if (cell.westChild.unlocked)
				{
					childOwned = true;
				}
			}
			else
			{
				connector.SetActive(value: false);
			}
			break;
		case CubeSide.North:
			if (cell.northChild != null)
			{
				connector.SetActive(value: true);
				if (cell.northChild.unlocked)
				{
					childOwned = true;
				}
			}
			else
			{
				connector.SetActive(value: false);
			}
			break;
		case CubeSide.South:
			if (cell.southChild != null)
			{
				connector.SetActive(value: true);
				if (cell.southChild.unlocked)
				{
					childOwned = true;
				}
			}
			else
			{
				connector.SetActive(value: false);
			}
			break;
		}
		if (cell.unlocked)
		{
			if (!childOwned)
			{
				connector.GetComponent<SpriteRenderer>().color = Palette.CosmicubeCellUnlockedColor;
			}
			else
			{
				connector.GetComponent<SpriteRenderer>().color = Palette.White;
			}
		}
	}

	public void Show()
	{
		connector.SetActive(value: true);
	}
}
