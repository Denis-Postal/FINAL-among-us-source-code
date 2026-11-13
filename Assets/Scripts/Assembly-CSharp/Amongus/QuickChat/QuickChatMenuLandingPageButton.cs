using Object = UnityEngine.Object;
using System;
using Cpp2ILInjected;
using TMPro;
using UnityEngine;

namespace AmongUs.QuickChat
{
	public class QuickChatMenuLandingPageButton : MonoBehaviour
	{
		[SerializeField]
		public PassiveButton button;

		[SerializeField]
		public BoxCollider2D collider;

		[SerializeField]
		private UIScrollbarHelper scrollHelper;

		[SerializeField]
		private TMP_Text text;

		[SerializeField]
		private RectTransform textRect;

		[SerializeField]
		private TextTranslatorTMP translator;

		[SerializeField]
		private SpriteRenderer[] backgroundSprites;

		[SerializeField]
		private SpriteRenderer[] shineSprites;

		private new QuickChatTag tag;

		public QuickChatTag Tag => tag;

		public PassiveButton Button => button;

		public UIScrollbarHelper ScrollHelper => scrollHelper;

		public float Width => collider.size.x;

		public float Height => collider.size.y;

		public StringNames TranslationKey => translator.TargetText;

		public virtual void Setup(int index, QuickChatTag tag)
		{
			this.tag = tag;
			gameObject.name = $"Category Button: {tag.TagNameKey}";
			SetStringName(tag.TagNameKey);
			Button.ControllerNav.mode = ControllerNavigation.Mode.Explicit;
			if (scrollHelper != null)
			{
				scrollHelper.index = index;
			}
		}

		public void SetStringName(StringNames key)
		{
			translator.TargetText = key;
			translator.ResetText();
		}

		public unsafe void SetWidth(float width)
		{
			Vector2 size = collider.size;
			Vector2 offset = collider.offset;
			float[] array = new float[backgroundSprites.Length];
			for (int i = 0; i < backgroundSprites.Length; i++)
			{
				SpriteRenderer spriteRenderer = backgroundSprites[i];
				array[i] = Mathf.Abs(spriteRenderer.size.x * 0.5f - offset.x);
			}
			size.x = width;
			offset.x = size.x * 0.5f;
			if (shineSprites != null && shineSprites.Length != 0)
			{
				SpriteRenderer[] array2 = shineSprites;
				foreach (SpriteRenderer obj in array2)
				{
					Vector3 localPosition = obj.transform.localPosition;
					localPosition.x = width * 0.5f;
					obj.transform.localPosition = localPosition;
				}
			}
			if (backgroundSprites != null && backgroundSprites.Length != 0)
			{
				for (int k = 0; k < backgroundSprites.Length; k++)
				{
					SpriteRenderer obj2 = backgroundSprites[k];
					Vector3 localPosition2 = obj2.transform.localPosition;
					float num = array[k];
					localPosition2.x = offset.x;
					obj2.transform.localPosition = localPosition2;
					Vector2 size2 = obj2.size;
					size2.x = size.x + num * 2f;
					obj2.size = size2;
				}
			}
			if (text != null)
			{
				Vector2 sizeDelta = textRect.sizeDelta;
				sizeDelta.x = width;
				textRect.sizeDelta = sizeDelta;
			}
			collider.size = size;
			collider.offset = offset;
		}
	}
}
