using Object = UnityEngine.Object;
using System;
using System.Runtime.CompilerServices;
using AmongUs.Data;
using AmongUs.Data.Settings;
using Cpp2ILInjected;
using TMPro;
using UnityEngine;

namespace AmongUs.QuickChat
{
	public class QuickChatMenuPhrasesPagePhraseButton : AbstractQuickChatMenuPhrasesPageButton
	{
		[SerializeField]
		private QuickChatFavoriteIcon favoriteIcon;

		public override string ButtonType => "Phrase";

		protected override void Awake()
		{
			favoriteIcon.OnPressEvent += OnFavoritePressed;
		}

		protected override float GetInitialWidth()
		{
			return Collider.size.x + favoriteIcon.Size.x;
		}

		protected unsafe override void OnSetup()
		{
			Vector2 vector = new Vector2(Width, Height);
			Vector2 sizeDelta = TextRect.sizeDelta;
			sizeDelta.x = Width - favoriteIcon.Size.x;
			TextRect.sizeDelta = sizeDelta;
			float num = vector.y - 0.2f;
			TMP_TextInfo textInfo = Text.GetTextInfo(Text.text);
			float num2 = 0.2f * (float)textInfo.lineCount + num;
			float num3 = num2 - vector.y;
			vector.y = num2;
			SpriteRenderer[] array = BackgroundSprites;
			foreach (SpriteRenderer obj in array)
			{
				Vector2 size = obj.size;
				size.y += num3;
				obj.size = size;
				Vector3 localPosition = obj.transform.localPosition;
				localPosition.y -= num3 * 0.5f;
				obj.transform.localPosition = localPosition;
			}
			favoriteIcon.Setup(DataManager.Settings.Multiplayer.HasQuickChatFavorite(Phrase.PhraseKey));
			favoriteIcon.SetHeight(vector.y);
			Collider.size = vector - new Vector2(favoriteIcon.Size.x, 0f);
			Collider.offset = new Vector2((vector.x + favoriteIcon.Size.x) / 2f, (0f - vector.y) / 2f);
			if (Text != null)
			{
				TextRect.sizeDelta = Collider.size;
				Text.fontSharedMaterial = TextMaterial;
				Vector3 localPosition2 = TextRect.localPosition;
				localPosition2.x = favoriteIcon.Size.x;
				TextRect.localPosition = localPosition2;
			}
		}

		public override void SetEnabled(bool enabled)
		{
			base.SetEnabled(enabled);
			favoriteIcon.gameObject.SetActive(enabled);
		}

		public void ToggleFavorite()
		{
			if (Phrase.Type != QuickChatPhraseType.PlayerId)
			{
				if (DataManager.Settings.Multiplayer.HasQuickChatFavorite(Phrase.PhraseKey))
				{
					QuickChatMenu.Logger.Debug(string.Format("[{0}] Removing QuickChat Favorite: {1}", "AbstractQuickChatMenuPhrasesPageButton", Phrase.PhraseKey));
					DataManager.Settings.Multiplayer.RemoveQuickChatFavorite(Phrase.PhraseKey);
					favoriteIcon.SetFavorite(isFavorite: false);
				}
				else
				{
					QuickChatMenu.Logger.Debug(string.Format("[{0}] Adding QuickChat Favorite: {1}", "AbstractQuickChatMenuPhrasesPageButton", Phrase.PhraseKey));
					DataManager.Settings.Multiplayer.AddQuickChatFavorite(Phrase.PhraseKey);
					favoriteIcon.SetFavorite(isFavorite: true);
				}
				DataManager.Settings.Save();
			}
		}

		private void OnFavoritePressed()
		{
			ToggleFavorite();
		}
	}
}
