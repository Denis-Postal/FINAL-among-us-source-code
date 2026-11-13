using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace AmongUs.QuickChat
{
	public class QuickChatMenu : MonoBehaviour
	{
		public const float WIDTH_DEFAULT = 3.5f;

		public const float WIDTH_MIN = 2.5f;

		public const float PADDING_X = 0.2f;

		public static readonly Logger Logger = new Logger(Logger.Category.UI, "QuickChat");

		[SerializeField]
		private QuickChatData data;

		[SerializeField]
		private Transform container;

		[SerializeField]
		private SpriteRenderer frame;

		[Space(10f)]
		[SerializeField]
		private Transform topContentBlocker;

		[SerializeField]
		private Transform bottomContentBlocker;

		[Space(10f)]
		[SerializeField]
		private PassiveButton closeButton;

		[SerializeField]
		private GameObject closeButtonGlyph;

		[Header("Animations")]
		[SerializeField]
		private AnimationCurve animationIn;

		[SerializeField]
		private AnimationCurve animationOut;

		[Header("Pages")]
		[SerializeField]
		private Transform pagesContainer;

		[SerializeField]
		private QuickChatMenuLandingPage landingPage;

		[SerializeField]
		private QuickChatMenuPhrasesPage phrasesPage;

		[Header("Header")]
		[SerializeField]
		private QuickChatMenuHeader header;

		[SerializeField]
		private QuickChatMenuVoteTimer timer;

		private QuickChatPhraseBuilder builder = new QuickChatPhraseBuilder();

		private List<QuickChatBreadcrumb> breadcrumbs = new List<QuickChatBreadcrumb>();

		private IEnumerator animationCoroutine;

		public bool isOpen;

		private float height;

		private float width;

		private float targetXPos;

		public QuickChatData Data => data;

		public bool IsOpen => isOpen;

		public bool IsAnimating => animationCoroutine != null;

		public bool IsEmpty => builder.IsEmpty;

		public bool CanSend => builder.IsComplete();

		public Vector3 WorldPosition => container.position;

		private bool IsTimerVisible => MeetingHud.Instance != null;

		private float TargetXClosed => 0f;

		private float TargetXOpen => 0f - width;

		public event Action OnOpenEvent;

		public event Action OnCloseEvent;

		public event QuickChatPhraseBuilder.ChangeHandler OnBuilderChangedEvent;

		private void Awake()
		{
			builder.OnChangedEvent += OnBuilderChanged;
			landingPage.OnButtonPressed += OnLandingPageTagButtonPressed;
			header.OnBackPressedEvent += OnHeaderBackButtonPressed;
			phrasesPage.OnPhraseSelectedEvent += OnPhraseSelected;
			closeButton.AddOnClickListeners(OnCloseButtonPressed);
		}

		private void Update()
		{
			UpdateContainerTransform();
		}

		public void Clear()
		{
			builder.Reset();
			if (IsOpen)
			{
				ShowLandingPage();
			}
		}

		public void Undo()
		{
			builder.Undo();
			breadcrumbs.RemoveAt(breadcrumbs.Count - 1);
			if (builder.IsEmpty)
			{
				if (breadcrumbs.Count < 1)
				{
					ShowLandingPage();
				}
				else
				{
					ShowPhrasePage(breadcrumbs.Last().Tags);
				}
			}
			else
			{
				ShowPhrasePage(builder.GetNextExpectedInput());
			}
		}

		public void UpdateVoteTimerText(StringNames key, int value)
		{
			timer.SetText(key, value);
		}

		private void InitializeChildren()
		{
			float num = width - 0.4f;
			height = DestroyableSingleton<HudManager>.Instance.UICamera.orthographicSize * 2f;
			float num2 = 0f;
			Vector3 localPosition = frame.transform.localPosition;
			frame.transform.localPosition = new Vector3(localPosition.x, 0.15f, localPosition.z);
			frame.size = new Vector2(frame.size.x, height + 0.3f);
			timer.gameObject.SetActive(value: false);
			if (IsTimerVisible)
			{
				timer.gameObject.SetActive(value: true);
				timer.SetWidth(num);
				timer.SetText(StringNames.QCResVote, 0);
				num2 -= timer.Height;
			}
			Vector3 localPosition2 = header.transform.localPosition;
			localPosition2.y = num2;
			header.transform.localPosition = localPosition2;
			header.Initialize(num);
			UpdatePagePosition();
			landingPage.Initialize(this, num);
			phrasesPage.Initialize(this, num);
			ShowLandingPage();
		}

		public void OnCloseButtonPressed()
		{
			Close();
		}

		public QuickChatPhraseBuilderResult BuildMessageData()
		{
			return builder.Build();
		}

		public void BuildMessageLog(StringBuilder log)
		{
			builder.ToLogString(log);
		}

		public bool CanAddPhraseToBuilder(QuickChatPhrase p)
		{
			return builder.CanAddAsNextPhrase(p);
		}

		private void OnBuilderChanged(QuickChatPhraseBuilder builder)
		{
			OnBuilderChangedEvent?.Invoke(builder);
		}

		public void Open(float width)
		{
			if (!IsOpen)
			{
				this.width = width;
				InitializeChildren();
				gameObject.SetActive(value: true);
				isOpen = true;
				StopAnimationCoroutine();
				animationCoroutine = CoAnimate(TargetXClosed, TargetXOpen, animationIn);
				StartCoroutine(animationCoroutine);
				ControllerManager.Instance.OpenOverlayMenu(name, closeButton, landingPage.DefaultSelection, null);
				ShowLandingPage();
				OnOpenEvent?.Invoke();
			}
		}

		public void Close()
		{
			if (IsOpen)
			{
				isOpen = false;
				ControllerManager.Instance.UnsetBackButton(closeButton);
				StopAnimationCoroutine();
				animationCoroutine = CoAnimate(TargetXOpen, TargetXClosed, animationOut, OnCloseComplete);
				StartCoroutine(animationCoroutine);
				builder.Reset();
				OnCloseEvent?.Invoke();
			}
		}

		public void CloseImmediately()
		{
			isOpen = false;
			ControllerManager.Instance.UnsetBackButton(closeButton);
			StopAnimationCoroutine();
			SetAnimationParameters(TargetXClosed);
			UpdateContainerTransform();
			builder.Reset();
			OnCloseComplete();
			OnCloseEvent?.Invoke();
		}

		private void OnCloseComplete()
		{
			ControllerManager.Instance.CloseOverlayMenu(name);
			gameObject.SetActive(value: false);
		}

		private void StopAnimationCoroutine()
		{
			if (animationCoroutine != null)
			{
				StopCoroutine(animationCoroutine);
				animationCoroutine = null;
			}
		}

		private IEnumerator CoAnimate(float from, float to, AnimationCurve animation, Action onCompleteCallback = null)
		{
			targetXPos = container.localPosition.x;
			float duration = animation.keys.Last().time;
			float t = 0f;
			while (t < duration)
			{
				t += Time.deltaTime;
				SetAnimationParameters(Mathf.LerpUnclamped(from, to, animation.Evaluate(t)));
				yield return null;
			}
			onCompleteCallback?.Invoke();
			animationCoroutine = null;
		}

		private void SetAnimationParameters(float value)
		{
			targetXPos = value;
		}

		private void UpdateContainerTransform()
		{
			Vector3 localPosition = container.localPosition;
			localPosition.x = targetXPos;
			container.localPosition = localPosition;
		}

		private float GetPageHeight()
		{
			float num = height;
			if (IsTimerVisible)
			{
				num -= timer.Height;
			}
			return num - header.Height;
		}

		private void UpdatePagePosition()
		{
			Vector3 localPosition = pagesContainer.localPosition;
			localPosition.y = header.transform.localPosition.y - header.Height;
			pagesContainer.localPosition = localPosition;
		}

		private void UpdatePageBlockers()
		{
			Vector3 localPosition = topContentBlocker.localPosition;
			Vector3 localScale = topContentBlocker.localScale;
			Vector3 localPosition2 = bottomContentBlocker.localPosition;
			Vector3 localScale2 = bottomContentBlocker.localScale;
			localPosition.y = pagesContainer.localPosition.y;
			localScale.x = width;
			localScale.y = Mathf.Abs(localPosition.y);
			localPosition2.y = localPosition.y - GetPageHeight();
			localScale2.x = width;
			localScale2.y = 5f;
			topContentBlocker.localPosition = localPosition;
			topContentBlocker.localScale = localScale;
			bottomContentBlocker.localPosition = localPosition2;
			bottomContentBlocker.localScale = localScale2;
		}

		private void ShowLandingPage()
		{
			breadcrumbs.Clear();
			closeButtonGlyph.SetActive(value: true);
			header.ShowLandingPage();
			UpdatePagePosition();
			phrasesPage.Hide();
			landingPage.Show(GetPageHeight());
			UpdatePageBlockers();
			if (isOpen)
			{
				ControllerManager.Instance.SetBackButton(closeButton);
			}
		}

		private void OnLandingPageTagButtonPressed(QuickChatMenuLandingPageButton button)
		{
			Logger.Debug("[QuickChatMenu] Landing page QuickChatTag button pressed: " + button.gameObject.name);
			breadcrumbs.Add(new QuickChatBreadcrumb(QuickChatBreadcrumbType.Tag, button.Tag));
			ShowPhrasePage(button.Tag);
		}

		private void ShowPhrasePage(params QuickChatTag[] tags)
		{
			header.ShowPhrasesPage(breadcrumbs);
			closeButtonGlyph.SetActive(value: false);
			UpdatePagePosition();
			landingPage.Hide();
			phrasesPage.ShowTagPage(GetPageHeight(), tags);
			UpdatePageBlockers();
		}

		private void OnPhraseSelected(QuickChatPhrase phrase)
		{
			if (breadcrumbs == null || breadcrumbs.Count <= 0 || breadcrumbs.Last().Type != QuickChatBreadcrumbType.End)
			{
				if (builder.IsEmpty)
				{
					Logger.Info(string.Format("[{0}] {1} build <color=#de381b>STARTED</color>! Root Phrase: {2}", "QuickChatMenu", "QuickChatPhraseBuilder", phrase.PhraseKey));
					builder.Start(phrase);
				}
				else
				{
					Logger.Info(string.Format("[{0}] {1} build <color=#f5a70c>ADDED</color>: {2}", "QuickChatMenu", "QuickChatPhraseBuilder", phrase.PhraseKey));
					builder.AddNextPhrase(phrase);
				}
				if (builder.IsComplete())
				{
					Logger.Info("[QuickChatMenu] QuickChatPhraseBuilder build <color=#9bf514>COMPLETE</color>! Ready to send.");
					breadcrumbs.Add(new QuickChatBreadcrumb(QuickChatBreadcrumbType.End, (QuickChatTag[])null));
					header.ShowPhrasesPage(breadcrumbs);
					phrasesPage.SetPhrasesEnableState(enabled: false);
				}
				else
				{
					phrasesPage.SetPhrasesEnableState(enabled: true);
					QuickChatTag[] nextExpectedInput = builder.GetNextExpectedInput();
					breadcrumbs.Add(new QuickChatBreadcrumb(QuickChatBreadcrumbType.Inputs, nextExpectedInput));
					ShowPhrasePage(nextExpectedInput);
				}
			}
		}

		private void OnHeaderBackButtonPressed()
		{
			Undo();
		}

		private void OnPhrasesTopMenuButtonPressed()
		{
			builder.Reset();
			ShowLandingPage();
		}
	}
}
