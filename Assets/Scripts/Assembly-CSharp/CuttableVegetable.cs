using System.Collections;
using UnityEngine;

public class CuttableVegetable : MonoBehaviour
{
	[SerializeField]
	private SpriteRenderer[] uncutPieces;

	[SerializeField]
	private SpriteRenderer[] partiallyCutPieces;

	[SerializeField]
	private Sprite[] cutSprites;

	private int cutPieces;

	public int NextPieceIndex => uncutPieces.Length - cutPieces - 1;

	public SpriteRenderer NextPiece => uncutPieces[NextPieceIndex];

	public bool IsFullyCut => CheckComplete(cutPieces);

	private void Awake()
	{
		SpriteRenderer[] array = partiallyCutPieces;
		foreach (SpriteRenderer spriteRenderer in array)
		{
			if (spriteRenderer != null)
			{
				spriteRenderer.gameObject.SetActive(value: false);
			}
		}
	}

	public IEnumerator CoCutNextPiece()
	{
		int nextPieceIndex = NextPieceIndex;
		int num = nextPieceIndex - 1;
		uncutPieces[nextPieceIndex].sprite = cutSprites[nextPieceIndex];
		if (partiallyCutPieces[nextPieceIndex] != null)
		{
			uncutPieces[nextPieceIndex].gameObject.SetActive(value: true);
			partiallyCutPieces[nextPieceIndex].gameObject.SetActive(value: false);
		}
		if (partiallyCutPieces[num] != null)
		{
			uncutPieces[num].gameObject.SetActive(value: false);
			partiallyCutPieces[num].gameObject.SetActive(value: true);
		}
		float x = 0.6f - 0.6f / (float)(uncutPieces.Length - 1) * (float)cutPieces;
		if (CheckComplete(cutPieces + 1))
		{
			uncutPieces[num].sprite = cutSprites[num];
		}
		VibrationManager.Vibrate(0.3f, 0.3f, 0.1f);
		yield return Effects.Slide2D(NextPiece.transform, NextPiece.transform.localPosition, (Vector2)NextPiece.transform.localPosition + new Vector2(x, 0f), 0.25f);
		cutPieces++;
	}

	private bool CheckComplete(int cutPieces)
	{
		return cutPieces >= uncutPieces.Length - 1;
	}
}
