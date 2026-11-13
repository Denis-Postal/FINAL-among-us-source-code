using Cpp2ILInjected;
using TMPro;
using UnityEngine;

namespace AmongUs.QuickChat
{
	public class QuickChatMenuVoteTimer : MonoBehaviour
	{
		private const float HEIGHT = 0.4f;

		[SerializeField]
		private TextMeshPro text;

		[SerializeField]
		public RectTransform textRect;

		[SerializeField]
		public SpriteRenderer[] backgrounds;

		public float Height => 0.4f;

		public void SetText(StringNames key, int value)
		{
			text.text = string.Format(DestroyableSingleton<TranslationController>.Instance.GetString(key), $"<b>{value}</b>");
		}

		public unsafe void SetWidth(float width)
		{
			Vector2 sizeDelta = textRect.sizeDelta;
			textRect.sizeDelta = new Vector2(width, sizeDelta.y);
			SpriteRenderer[] array = backgrounds;
			foreach (SpriteRenderer obj in array)
			{
				obj.size = new Vector2(width, obj.size.y);
				Vector3 localPosition = obj.transform.localPosition;
				localPosition.x = width * 0.5f;
				obj.transform.localPosition = localPosition;
			}
		}
	}
}
