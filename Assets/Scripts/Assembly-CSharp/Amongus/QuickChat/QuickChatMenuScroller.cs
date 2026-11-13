using Object = UnityEngine.Object;
using System;
using Cpp2ILInjected;
using UnityEngine;

namespace AmongUs.QuickChat
{
	public class QuickChatMenuScroller : MonoBehaviour
	{
		public const float SCROLLBAR_WIDTH = 0.2f;

		[SerializeField]
		private Scroller scroller;

		[SerializeField]
		public Scrollbar scrollBar;

		[SerializeField]
		private SpriteRenderer track;

		[SerializeField]
		private BoxCollider2D collider;

		[SerializeField]
		private ScrollToSelection scrollToSelection;

		[SerializeField]
		private SpriteRenderer topGradient;

		[SerializeField]
		private Transform topGradientContainer;

		[SerializeField]
		private SpriteRenderer bottomGradient;

		[SerializeField]
		private Transform bottomGradientContainer;

		private float width;

		private float height;

		private Color topGradientColor;

		private Color bottomGradientColor;

		public bool IsVisible => scrollBar.HandleDrag;

		private void Awake()
		{
			Scroller scroller = this.scroller;
			scroller.OnScrollYEvent = (Scroller.ScrollHandler)Delegate.Combine(scroller.OnScrollYEvent, new Scroller.ScrollHandler(OnScroll));
		}

		public void SetSize(float width, float height)
		{
			this.width = width;
			this.height = height;
			Vector2 vector = track.transform.localPosition;
			vector.x = width;
			track.transform.localPosition = vector;
			Vector3 localPosition = scrollBar.transform.localPosition;
			localPosition.x = width;
			scrollBar.transform.localPosition = localPosition;
			Vector3 localPosition2 = collider.transform.localPosition;
			localPosition2.x = width / 2f;
			localPosition2.y = height / 2f * -1f;
			collider.transform.localPosition = localPosition2;
			Vector3 localScale = collider.transform.localScale;
			localScale.x = width + 0.2f;
			localScale.y = height;
			collider.transform.localScale = localScale;
			InitializeGradient();
			float y = height - 0.2f;
			Vector2 size = track.size;
			size.y = y;
			track.size = size;
			float num = height - 0.25f;
			scroller.ScrollbarYBounds = new FloatRange(Mathf.Min(-0.25f, 0f - num), Mathf.Max(-0.25f, 0f - num));
			scroller.ScrollToTop();
		}

		public void SetContentHeight(float contentHeight)
		{
			float min = scroller.GetYBounds().min;
			scroller.SetBounds(new FloatRange(min, Mathf.Max(min, contentHeight + min - collider.transform.localScale.y)), null);
			scroller.ScrollToTop();
			InitializeGradient();
		}

		private void InitializeGradient()
		{
			if (topGradient != null && topGradientColor.a < 0.1f)
			{
				topGradientColor = topGradient.color;
			}
			if (bottomGradient != null && bottomGradientColor.a < 0.1f)
			{
				bottomGradientColor = bottomGradient.color;
			}
			if (topGradientContainer != null)
			{
				topGradientContainer.gameObject.SetActive(IsVisible);
				Vector3 localPosition = topGradientContainer.localPosition;
				localPosition.y = 0f;
				topGradientContainer.localPosition = localPosition;
				Vector3 localScale = topGradientContainer.localScale;
				localScale.x = width + Mathf.Abs(localPosition.x) * 2f;
				topGradientContainer.localScale = localScale;
			}
			if (bottomGradientContainer != null)
			{
				bottomGradientContainer.gameObject.SetActive(IsVisible);
				Vector3 localPosition2 = bottomGradientContainer.localPosition;
				localPosition2.y = 0f - height;
				bottomGradientContainer.localPosition = localPosition2;
				Vector3 localScale2 = bottomGradientContainer.localScale;
				localScale2.x = width + Mathf.Abs(localPosition2.x) * 2f;
				bottomGradientContainer.localScale = localScale2;
			}
			UpdateGradientAlpha(0f);
		}

		private unsafe void UpdateGradientAlpha(float value)
		{
			if (topGradient != null)
			{
				Color color = topGradientColor;
				color.a *= GetPercentage(0f, 0.4f, value);
				topGradient.color = color;
			}
			if (bottomGradient != null)
			{
				Color color2 = bottomGradientColor;
				color2.a *= 1f - GetPercentage(0.6f, 1f, value);
				bottomGradient.color = color2;
			}
		}

		private static float GetPercentage(float min, float max, float input)
		{
			return Mathf.Clamp01((input - min) / (max - min));
		}

		private void OnScroll(float value)
		{
			UpdateGradientAlpha(value);
		}
	}
}
