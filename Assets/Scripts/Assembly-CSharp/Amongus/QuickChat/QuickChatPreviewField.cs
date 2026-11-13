using System;
using System.Linq;
using AmongUs.Data;
using InnerNet;
using TMPro;
using UnityEngine;

namespace AmongUs.QuickChat
{
	public class QuickChatPreviewField : AbstractChatInputField
	{
		[Space(10f)]
		[SerializeField]
		private ChatInputFieldButton clearButton;

		[SerializeField]
		private ChatInputFieldButton undoButton;

		[SerializeField]
		private TextMeshPro warningText;

		[SerializeField]
		private TextMeshPro text;

		[SerializeField]
		private TextMeshPro placeholderText;

		public event Action OnClearPressedEvent;

		public event Action OnUndoPressedEvent;

		protected override void Awake()
		{
			base.Awake();
			clearButton.OnPressed += OnClearButtonPressed;
			clearButton.OnSizeChanged += OnClearButtonSizeChanged;
			undoButton.OnPressed += OnUndoButtonPressed;
			undoButton.OnSizeChanged += OnUndoButtonSizeChanged;
			warningText.text = GetQuickChatChatWarningStr();
		}

		public override void Clear()
		{
			base.Clear();
			UpdatePreview(null);
		}

		public void UpdatePreview(QuickChatPhraseBuilder builder)
		{
			text.text = "";
			text.gameObject.SetActive(value: false);
			placeholderText.gameObject.SetActive(value: true);
			SetCanSubmit(canSubmit: false);
			clearButton.gameObject.SetActive(value: false);
			undoButton.gameObject.SetActive(value: false);
			if (builder == null || builder.IsEmpty)
			{
				return;
			}
			QuickChatPhrase[] array = builder.ToPreviewData();
			if (array == null || array.Length < 1)
			{
				clearButton.gameObject.SetActive(value: false);
				return;
			}
			text.gameObject.SetActive(value: true);
			placeholderText.gameObject.SetActive(value: false);
			clearButton.gameObject.SetActive(value: true);
			clearButton.gameObject.SetActive(value: true);
			undoButton.gameObject.SetActive(value: true);
			QuickChatPhrase rootPhrase = array[0];
			text.text = rootPhrase.ToPreviewText(null);
			if (array[0].Type != QuickChatPhraseType.ComplexPhrase)
			{
				SetCanSubmit(canSubmit: true);
				return;
			}
			text.text = rootPhrase.ToPreviewText(array.Where((QuickChatPhrase x) => x != rootPhrase).ToArray());
			if (builder.IsComplete())
			{
				SetCanSubmit(canSubmit: true);
			}
		}

		public void AnimateInvalidSend()
		{
			Debug.LogWarning("TODO: implement 'invalid send' animation for when trying to send a QC message before the builder is complete");
		}

		protected override void OnTextFieldTransformChanged(Vector3 position, Vector2 size)
		{
			base.OnTextFieldTransformChanged(position, size);
			text.rectTransform.localPosition = position;
			text.rectTransform.sizeDelta = size;
			placeholderText.rectTransform.localPosition = position;
			placeholderText.rectTransform.sizeDelta = size;
		}

		private void OnClearButtonPressed()
		{
			OnClearPressedEvent?.Invoke();
		}

		private void OnClearButtonSizeChanged()
		{
			UpdateClearButtonTransform();
		}

		private void UpdateClearButtonTransform()
		{
			Vector3 localPosition = clearButton.transform.localPosition;
			localPosition.x = undoButton.transform.localPosition.x - 0.1f - clearButton.Size.x * clearButton.transform.localScale.x;
			clearButton.transform.localPosition = localPosition;
		}

		private void OnUndoButtonPressed()
		{
			OnUndoPressedEvent?.Invoke();
		}

		private void OnUndoButtonSizeChanged()
		{
			UpdateUndoButtonTransform();
		}

		private void UpdateUndoButtonTransform()
		{
			Bounds bounds = GetBounds();
			Vector3 localPosition = undoButton.transform.localPosition;
			localPosition.x = bounds.max.x - undoButton.Size.x * undoButton.transform.localScale.x;
			undoButton.transform.localPosition = localPosition;
			UpdateClearButtonTransform();
		}

		private string GetQuickChatChatWarningStr()
		{
			string text = "";
			if (DataManager.Settings.Multiplayer.ChatMode == QuickChatModes.QuickChatOnly)
			{
				text = DestroyableSingleton<TranslationController>.Instance.GetString(StringNames.QuickChatInstructionsStart) + " ";
				if (DataManager.Player.Account.LoginStatus == EOSManager.AccountLoginStatus.LoggedIn && DestroyableSingleton<EOSManager>.Instance.IsMinorOrWaiting() && !DestroyableSingleton<EOSManager>.Instance.IsFreechatAllowed())
				{
					text += DestroyableSingleton<TranslationController>.Instance.GetString(StringNames.QuickChatInstructionsChild);
				}
			}
			return text;
		}
	}
}
