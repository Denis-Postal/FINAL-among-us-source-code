using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace AmongUs.QuickChat
{
	public class QuickChatMenuHeader : MonoBehaviour
	{
		private const float PADDING = 0.2f;

		private const float TEXT_LINE_HEIGHT = 0.5f;

		private const float DIVIDER_OFFSET = -0.11f;

		private const StringNames DEFAULT_HEADER = StringNames.QCBuilderHeader;

		[SerializeField]
		private Transform content;

		[Space(10f)]
		[SerializeField]
		private TextMeshPro text;

		[SerializeField]
		private Transform textMask;

		[SerializeField]
		private Transform textGradient;

		[Space(10f)]
		[SerializeField]
		private PassiveButton backButton;

		[SerializeField]
		private BoxCollider2D backButtonCollider;

		[Space(10f)]
		[SerializeField]
		private Transform divider;

		[Space(10f)]
		[SerializeField]
		private GameObject favoriteBox;

		[SerializeField]
		private GameObject favoriteBoxContainer;

		[SerializeField]
		private GameObject favoriteBoxGlyph;

		[SerializeField]
		private SpriteRenderer favoriteBoxBackground;

		private float width;

		private float height;

		private float defaultFontSize;

		public float Height => height;

		public event Action OnBackPressedEvent;

		private void Awake()
		{
			defaultFontSize = text.fontSize;
			backButton.AddOnClickListeners(OnBackPressed);
		}

		private void Update()
		{
			if (favoriteBoxGlyph.activeSelf != favoriteBoxContainer.activeSelf)
			{
				favoriteBoxContainer.SetActive(favoriteBoxGlyph.activeSelf);
			}
		}

		public void Initialize(float width)
		{
			this.width = width;
			Vector3 localPosition = content.localPosition;
			localPosition.y = -0.2f;
			content.localPosition = localPosition;
			text.text = DestroyableSingleton<TranslationController>.Instance.GetString(StringNames.QCBuilderHeader);
		}

		public void ShowLandingPage()
		{
			text.enableAutoSizing = true;
			text.text = DestroyableSingleton<TranslationController>.Instance.GetString(StringNames.QCBuilderHeader);
			Vector2 sizeDelta = text.rectTransform.sizeDelta;
			sizeDelta.y = 0.5f;
			text.rectTransform.sizeDelta = sizeDelta;
			ControllerManager.Instance.UnsetBackButton(backButton);
			backButton.gameObject.SetActive(value: false);
			UpdateSize();
			textGradient.gameObject.SetActive(value: false);
			SetFavoriteGlyphBox(enabled: false);
		}

		public void ShowPhrasesPage(List<QuickChatBreadcrumb> breadcrumbs)
		{
			string text = "";
			if (breadcrumbs.Count > 1)
			{
				text += "<color=#FFFFFF55>";
			}
			for (int i = 0; i < breadcrumbs.Count; i++)
			{
				QuickChatBreadcrumb quickChatBreadcrumb = breadcrumbs[i];
				if (i > 0 && quickChatBreadcrumb.Type != QuickChatBreadcrumbType.End)
				{
					text += "/ ";
				}
				if (breadcrumbs.Count > 1 && i == breadcrumbs.Count - 1)
				{
					text += "</color>";
				}
				switch (quickChatBreadcrumb.Type)
				{
				case QuickChatBreadcrumbType.Tag:
					text += quickChatBreadcrumb.Tags[0].LocalizedTagName;
					break;
				case QuickChatBreadcrumbType.Inputs:
					text += quickChatBreadcrumb.Tags[0].LocalizedTagName;
					break;
				}
			}
			this.text.enableAutoSizing = false;
			this.text.fontSize = defaultFontSize;
			this.text.text = ClampTextToBounds(text);
			backButton.gameObject.SetActive(value: true);
			ControllerManager.Instance.SetBackButton(backButton);
			UpdateSize();
			textGradient.gameObject.SetActive(value: true);
			SetFavoriteGlyphBox(enabled: true);
		}

		private void UpdateSize()
		{
			float x = backButtonCollider.size.x;
			Vector3 localPosition = text.rectTransform.localPosition;
			localPosition.x = 0f;
			Vector2 sizeDelta = text.rectTransform.sizeDelta;
			sizeDelta.x = width;
			if (backButton.gameObject.activeInHierarchy)
			{
				localPosition.x = x + 0.1f;
				sizeDelta.x = width - (x + 0.1f);
			}
			text.rectTransform.localPosition = localPosition;
			text.rectTransform.sizeDelta = sizeDelta;
			Vector3 localScale = textMask.localScale;
			localScale.x = sizeDelta.x;
			textMask.localScale = localScale;
			height = 0.4f + text.rectTransform.sizeDelta.y;
		}

		private void SetFavoriteGlyphBox(bool enabled)
		{
			favoriteBox.SetActive(enabled);
			Vector3 localPosition = divider.localPosition;
			localPosition.y = 0f - Height - -0.11f;
			localPosition.x = -0.2f;
			if (enabled)
			{
				localPosition.y -= favoriteBoxBackground.size.y * 0.5f;
				height += favoriteBoxBackground.size.y;
			}
			divider.localPosition = localPosition;
		}

		private string ClampTextToBounds(string input)
		{
			string result = input;
			TMP_TextInfo textInfo = text.GetTextInfo(input);
			int num = -1;
			int num2 = 0;
			for (int i = 0; i < textInfo.characterInfo.Length && !(textInfo.characterInfo[i].bottomLeft.x >= 0f); i++)
			{
				if (num == -1)
				{
					num = textInfo.characterInfo[i].index;
				}
				num2 = i;
			}
			if (num2 > 0 && num > -1)
			{
				result = input.Remove(num, num2);
			}
			return result;
		}

		private void OnBackPressed()
		{
			OnBackPressedEvent?.Invoke();
		}
	}
}
