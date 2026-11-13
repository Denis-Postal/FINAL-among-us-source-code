using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class AspectScaledAsset : MonoBehaviour
{
	private class ScaledAsset
	{
		public float OriginalWidth;

		public ScaledAsset(float originalWidth)
		{
			OriginalWidth = originalWidth;
		}
	}

	private class ScaledSprite : ScaledAsset
	{
		public SpriteRenderer Sprite;

		public ScaledSprite(float originalWidth, SpriteRenderer sprite) : base(originalWidth)
		{
			Sprite = sprite;
		}
	}

	private class ScaledCollider : ScaledAsset
	{
		public BoxCollider2D Collider;

		public ScaledCollider(float originalWidth, BoxCollider2D collider) : base(originalWidth)
		{
			Collider = collider;
		}
	}

	private class ScaledText : ScaledAsset
	{
		public TextMeshPro Text;

		public ScaledText(float originalWidth, TextMeshPro text) : base(originalWidth)
		{
			Text = text;
		}
	}

	private class ScaledMask : ScaledAsset
	{
		public SpriteMask Mask;

		public ScaledMask(float originalWidth, SpriteMask mask) : base(originalWidth)
		{
			Mask = mask;
		}
	}

	private sealed class _003C_003Ec__DisplayClass13_0
	{
		public float aspectDiff;

		internal void _003CScaleObject_003Eb__0(ScaledSprite s)
		{
			Vector2 size = s.Sprite.size;
			Vector2 size2 = default;
			s.Sprite.size = size2;
		}

		internal void _003CScaleObject_003Eb__1(ScaledCollider s)
		{
			Vector2 size = s.Collider.size;
			Vector2 size2 = default;
			s.Collider.size = size2;
		}

		internal void _003CScaleObject_003Eb__2(ScaledText s)
		{
			RectTransform rectTransform = s.Text.rectTransform;
			RectTransform rectTransform2 = s.Text.rectTransform;
			Vector2 sizeDelta = rectTransform2.sizeDelta;
			Vector2 sizeDelta2 = default;
			rectTransform.sizeDelta = sizeDelta2;
		}

		internal unsafe void _003CScaleObject_003Eb__3(ScaledMask s)
		{
			// TODO(verify): no-op, original logic unrecoverable from decomp
		}
	}

	[SerializeField]
	private List<SpriteRenderer> spritesToScale;

	[SerializeField]
	private AspectPosition aspectPosition;

	[SerializeField]
	private int positionDirection = 1;

	[SerializeField]
	private List<BoxCollider2D> collidersToScale;

	[SerializeField]
	private List<TextMeshPro> textToScale;

	[SerializeField]
	private List<SpriteMask> masksToScale;

	private float originalDistanceFromEdge;

	private List<ScaledSprite> allSprites;

	private List<ScaledCollider> allColliders;

	private List<ScaledText> allText;

	private List<ScaledMask> allMasks;

	private bool initialized;

	private void Awake()
	{
		allSprites = new List<ScaledSprite>();
		allColliders = new List<ScaledCollider>();
		allText = new List<ScaledText>();
		allMasks = new List<ScaledMask>();
		spritesToScale.ForEach((SpriteRenderer s) =>
		{
			allSprites.Add(new ScaledSprite(s.size.x, s));
		});
		collidersToScale.ForEach((BoxCollider2D c) =>
		{
			allColliders.Add(new ScaledCollider(c.size.x, c));
		});
		textToScale.ForEach((TextMeshPro t) =>
		{
			allText.Add(new ScaledText(t.rectTransform.sizeDelta.x, t));
		});
		masksToScale.ForEach((SpriteMask m) =>
		{
			allMasks.Add(new ScaledMask(m.transform.localScale.x, m));
		});
		if ((bool)aspectPosition)
		{
			originalDistanceFromEdge = aspectPosition.DistanceFromEdge.x;
		}
		initialized = true;
	}

	public unsafe void ScaleObject(float aspectDiff)
	{
		if (initialized)
		{
			allSprites.ForEach((ScaledSprite s) =>
			{
				s.Sprite.size = new Vector2(s.OriginalWidth * aspectDiff, s.Sprite.size.y);
			});
			allColliders.ForEach((ScaledCollider s) =>
			{
				s.Collider.size = new Vector2(s.OriginalWidth * aspectDiff, s.Collider.size.y);
			});
			allText.ForEach((ScaledText s) =>
			{
				s.Text.rectTransform.sizeDelta = new Vector2(s.OriginalWidth * aspectDiff, s.Text.rectTransform.sizeDelta.y);
			});
			allMasks.ForEach((ScaledMask s) =>
			{
				s.Mask.transform.localScale = new Vector2(s.OriginalWidth * aspectDiff, s.Mask.transform.localScale.y);
			});
			if ((bool)aspectPosition)
			{
				aspectPosition.DistanceFromEdge.x = originalDistanceFromEdge + (float)positionDirection * ((allSprites[0].OriginalWidth - allSprites[0].Sprite.size.x) / 2f);
			}
		}
	}

	private void _003CAwake_003Eb__12_0(SpriteRenderer s)
	{
		// TODO(verify): no-op, original logic unrecoverable from decomp
	}

	private void _003CAwake_003Eb__12_1(BoxCollider2D c)
	{
		// TODO(verify): no-op, original logic unrecoverable from decomp
	}

	private void _003CAwake_003Eb__12_2(TextMeshPro t)
	{
		// TODO(verify): no-op, original logic unrecoverable from decomp
	}

	private void _003CAwake_003Eb__12_3(SpriteMask m)
	{
		// TODO(verify): no-op, original logic unrecoverable from decomp
	}
}
