using TMPro;
using UnityEngine;

namespace AmongUs.QuickChat
{
	public abstract class AbstractQuickChatMenuPhrasesPageButton : MonoBehaviour
	{
		public delegate void ButtonClickedHandler(AbstractQuickChatMenuPhrasesPageButton button);

		private const int MASK_LAYER_INDEX_START = 145;

		[SerializeField]
		private PassiveButton button;

		[SerializeField]
		private BoxCollider2D collider;

		[SerializeField]
		private UIScrollbarHelper scrollHelper;

		[Space(10f)]
		[SerializeField]
		private TextMeshPro text;

		[SerializeField]
		private RectTransform textRect;

		[SerializeField]
		[Space(10f)]
		private SpriteRenderer arrow;

		[SerializeField]
		private SpriteRenderer[] backgroundSprites;

		private QuickChatPhrase phrase;

		private int index;

		private int buttonCount;

		private float width;

		private Material textMaterial;

		public QuickChatPhrase Phrase => phrase;

		public PassiveButton Button => button;

		public UIScrollbarHelper ScrollHelper => scrollHelper;

		public float Width => width;

		public float Height => collider.size.y;

		public int MaskLayer => buttonCount - index + 145;

		protected TextMeshPro Text => text;

		protected Material TextMaterial => textMaterial;

		protected RectTransform TextRect => textRect;

		protected BoxCollider2D Collider => collider;

		protected SpriteRenderer Arrow => arrow;

		protected SpriteRenderer[] BackgroundSprites => backgroundSprites;

		public abstract string ButtonType { get; }

		public event ButtonClickedHandler OnSelectedEvent;

		protected virtual void Awake()
		{
		}

		protected abstract void OnSetup();

		protected abstract float GetInitialWidth();

		public void Setup(int index, int buttonCount, float width, QuickChatPhrase phrase, Material textMaterial)
		{
			this.width = width;
			this.phrase = phrase;
			this.index = index;
			this.buttonCount = buttonCount;
			this.textMaterial = textMaterial;
			text.text = phrase.ToButtonText();
			scrollHelper.index = index;
			gameObject.name = ButtonType + " Button: " + phrase.ToPreviewText(null);
			Button.AddOnClickListeners(OnButtonPressed);
			Button.ControllerNav.mode = ControllerNavigation.Mode.Explicit;
			OnSelectedEvent = null;
			SetWidth(width);
			Vector3 localPosition = arrow.transform.localPosition;
			localPosition.y = (0f - Height) / 2f;
			arrow.transform.localPosition = localPosition;
		}

		public void SetWidth(float width)
		{
			Vector2 vector = new Vector2(GetInitialWidth(), Height);
			Vector2 vector2 = new Vector2(vector.x * 0.5f, (0f - vector.y) * 0.5f);
			float[] array = new float[backgroundSprites.Length];
			for (int i = 0; i < backgroundSprites.Length; i++)
			{
				SpriteRenderer spriteRenderer = backgroundSprites[i];
				array[i] = Mathf.Abs(spriteRenderer.size.x * 0.5f - vector2.x);
			}
			vector.x = width;
			vector2.x = vector.x * 0.5f;
			if (backgroundSprites != null && backgroundSprites.Length != 0)
			{
				for (int j = 0; j < backgroundSprites.Length; j++)
				{
					SpriteRenderer obj = backgroundSprites[j];
					Vector3 localPosition = obj.transform.localPosition;
					float num = array[j];
					localPosition.x = vector2.x;
					obj.transform.localPosition = localPosition;
					Vector2 size = obj.size;
					size.x = vector.x + num * 2f;
					obj.size = size;
				}
			}
			OnSetup();
		}

		public virtual void SetEnabled(bool enabled)
		{
			Button.SetButtonEnableState(enabled);
		}

		private void OnButtonPressed()
		{
			QuickChatMenu.Logger.Debug("[AbstractQuickChatMenuPhrasesPageButton] Button press DETECTED for phrase: " + gameObject.name);
			OnSelectedEvent?.Invoke(this);
		}
	}
}
