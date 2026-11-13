using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AmongUs.Data;
using AmongUs.QuickChat;
using InnerNet;
using TMPro;
using UnityEngine;

public class ChatController : MonoBehaviour
{
	private delegate string GetFormattedNameFunc(string name);

	private const float SCREEN_SIZE_PERCENT = 0.8f;

	private const float MAX_CHAT_SEND_RATE = 3f;

	private const float X_DISTANCE_FROM_QUICKCHAT = -0.39f;

	private static readonly Logger Logger = new Logger(Logger.Category.UI, "ChatController");

	[Header("Buttons")]
	[SerializeField]
	private PassiveButton chatButton;

	[SerializeField]
	private AspectPosition chatButtonAspectPosition;

	[Space(10f)]
	[SerializeField]
	private BanMenu banButton;

	[SerializeField]
	private GameObject openKeyboardButton;

	[SerializeField]
	private PassiveButton quickChatButton;

	[SerializeField]
	[Header("Chat Screen")]
	private GameObject chatScreen;

	[SerializeField]
	private ObjectPoolBehavior chatBubblePool;

	[SerializeField]
	private Scroller scroller;

	[SerializeField]
	private SpriteRenderer backgroundImage;

	[SerializeField]
	private SpriteRenderer chatNotifyDot;

	[SerializeField]
	private TextMeshPro sendRateMessageText;

	[SerializeField]
	private AspectPosition aspectPosition;

	[SerializeField]
	[Header("Free Chat")]
	private FreeChatInputField freeChatField;

	[SerializeField]
	[Header("Quick Chat")]
	private QuickChatMenu quickChatMenu;

	[SerializeField]
	private QuickChatPreviewField quickChatField;

	[Header("SFX")]
	[SerializeField]
	private AudioClip messageSound;

	[SerializeField]
	private AudioClip warningSound;

	[SerializeField]
	[Header("Animations")]
	private AnimationCurve chatAnimationIn;

	[SerializeField]
	private AnimationCurve chatAnimationOut;

	[Header("Console Controller Navigation")]
	[SerializeField]
	private UiElement backButton;

	[SerializeField]
	private UiElement defaultButtonSelected;

	[SerializeField]
	private List<UiElement> controllerSelectable;

	[SerializeField]
	private ChatNotification chatNotification;

	private ChatControllerState state;

	private Vector3 targetChatPosition;

	private float timeSinceLastMessage = 3f;

	private Coroutine notificationRoutine;

	private SpecialInputHandler specialInputHandler;

	private float targetScale;

	public ChatControllerState State
	{
		get
		{
			if (!chatScreen.activeInHierarchy)
			{
				return ChatControllerState.Closed;
			}
			return state;
		}
	}

	public bool IsOpenOrOpening
	{
		get
		{
			if (State != ChatControllerState.Opening)
			{
				return State == ChatControllerState.Open;
			}
			return true;
		}
	}

	public bool IsClosedOrClosing
	{
		get
		{
			if (State != ChatControllerState.Closing)
			{
				return State == ChatControllerState.Closed;
			}
			return true;
		}
	}

	public bool IsAnimating
	{
		get
		{
			if (State != ChatControllerState.Opening)
			{
				return State == ChatControllerState.Closing;
			}
			return true;
		}
	}

	private void Awake()
	{
		specialInputHandler = GetComponentInChildren<SpecialInputHandler>(includeInactive: true);
		freeChatField.OnSubmitEvent += OnFreeChatSubmit;
		freeChatField.OnChangedEvent += OnFreeChatChanged;
		quickChatButton.AddOnClickListeners(OnQuickChatButtonClicked);
		quickChatMenu.OnOpenEvent += OnQuickChatOpened;
		quickChatMenu.OnCloseEvent += OnQuickChatClosed;
		quickChatMenu.OnBuilderChangedEvent += OnQuickChatBuilderChanged;
		quickChatField.OnSubmitEvent += OnQuickChatSubmit;
		quickChatField.OnClearPressedEvent += OnQuickChatClearPressed;
		quickChatField.OnUndoPressedEvent += OnQuickChatUndoPressed;
		targetChatPosition = chatScreen.transform.localPosition;
	}

	private void OnEnable()
	{
		if (specialInputHandler != null)
		{
			specialInputHandler.disableVirtualCursor = true;
		}
		DataManager.Settings.Multiplayer.OnChatModeChanged += OnChatModeSettingsChanged;
		ResolutionManager.ResolutionChanged += OnResolutionChanged;
		quickChatMenu.Close();
		UpdateChatMode();
	}

	private void OnDisable()
	{
		DataManager.Settings.Multiplayer.OnChatModeChanged -= OnChatModeSettingsChanged;
		ResolutionManager.ResolutionChanged -= OnResolutionChanged;
	}

	private void Update()
	{
		timeSinceLastMessage += Time.deltaTime;
		if (sendRateMessageText.isActiveAndEnabled)
		{
			float num = 3f - timeSinceLastMessage;
			if (num < 0f)
			{
				sendRateMessageText.gameObject.SetActive(value: false);
				return;
			}
			sendRateMessageText.text = DestroyableSingleton<TranslationController>.Instance.GetString(StringNames.ChatRateLimit, Mathf.CeilToInt(num));
		}
	}

	private void LateUpdate()
	{
		Vector3 vector = transform.InverseTransformPoint(quickChatMenu.WorldPosition);
		Vector3 localPosition = chatScreen.transform.localPosition;
		localPosition.x = Mathf.Min(targetChatPosition.x, vector.x - -0.39f * chatScreen.transform.localScale.x);
		chatScreen.transform.localPosition = localPosition;
	}

	public void SendChat()
	{
		float num = 3f - timeSinceLastMessage;
		if (num > 0f)
		{
			sendRateMessageText.gameObject.SetActive(value: true);
			sendRateMessageText.text = DestroyableSingleton<TranslationController>.Instance.GetString(StringNames.ChatRateLimit, Mathf.CeilToInt(num));
			return;
		}
		if (quickChatMenu.CanSend)
		{
			SendQuickChat();
		}
		else
		{
			if (quickChatMenu.IsOpen || string.IsNullOrWhiteSpace(freeChatField.Text) || DataManager.Settings.Multiplayer.ChatMode != QuickChatModes.FreeChatOrQuickChat)
			{
				return;
			}
			SendFreeChat();
		}
		timeSinceLastMessage = 0f;
		freeChatField.Clear();
		quickChatMenu.Clear();
		quickChatField.Clear();
		UpdateChatMode();
	}

	public void OnMeetingStart()
	{
		quickChatMenu.Close();
	}

	private void UpdateChatMode()
	{
		bool flag = DataManager.Settings.Multiplayer.ChatMode != QuickChatModes.QuickChatOnly && quickChatMenu.IsEmpty && !quickChatMenu.IsOpen;
		if (freeChatField.Visible != flag)
		{
			freeChatField.SetVisible(flag);
		}
		if (quickChatField.Visible != !flag)
		{
			quickChatField.SetVisible(!flag);
		}
		banButton.Hide();
		CheckKeyboardButton();
	}

	private void CheckKeyboardButton()
	{
		bool flag = DataManager.Settings.Multiplayer.ChatMode != QuickChatModes.QuickChatOnly;
		if (openKeyboardButton.activeSelf != flag)
		{
			openKeyboardButton.SetActive(flag);
		}
	}

	private void OnChatModeSettingsChanged()
	{
		UpdateChatMode();
	}

	private void OnResolutionChanged(float aspectRatio, int width, int height, bool fullscreen)
	{
		chatButtonAspectPosition.AdjustPosition();
		targetChatPosition = chatButton.transform.localPosition;
		Close();
	}

	public void SetVisible(bool visible)
	{
		Logger.Debug("SetVisible() :: Chat is " + (visible ? "visible" : "hidden"));
		ForceClosed();
		gameObject.SetActive(visible);
	}

	public void Close()
	{
		StopAllCoroutines();
		StartCoroutine(CoClose());
	}

	public void ForceClosed()
	{
		StopAllCoroutines();
		quickChatMenu.CloseImmediately();
		freeChatField.Unfocus();
		chatScreen.SetActive(value: false);
		banButton.SetVisible(show: false);
		banButton.Hide();
		state = ChatControllerState.Closed;
		chatButton.SelectButton(isSelected: false);
		if (DestroyableSingleton<HudManager>.InstanceExists)
		{
			DestroyableSingleton<HudManager>.Instance.ToggleMapButtonGlyph(isActive: true);
		}
		if (DestroyableSingleton<GameStartManager>.InstanceExists)
		{
			DestroyableSingleton<GameStartManager>.Instance.ToggleButtonGlyphs(enabled: true);
		}
		ConsoleJoystick.SetMode_Menu();
		ControllerManager.Instance.CloseOverlayMenu(name);
	}

	public void Toggle()
	{
		CustomNetworkTransform customNetworkTransform = (PlayerControl.LocalPlayer ? PlayerControl.LocalPlayer.NetTransform : null);
		if (!customNetworkTransform || DestroyableSingleton<FriendsListManager>.Instance.Ui.gameObject.activeSelf || MatchInfoGuide.Instance.IsActive || ((bool)PlayerCustomizationMenu.Instance && PlayerCustomizationMenu.Instance.cosmicubeMenu.activeSelf))
		{
			return;
		}
		StopAllCoroutines();
		if (IsOpenOrOpening)
		{
			StartCoroutine(CoClose());
			if (DestroyableSingleton<FriendsListManager>.InstanceExists)
			{
				DestroyableSingleton<FriendsListManager>.Instance.SetFriendButtonColor(isGrayedOut: false);
			}
			return;
		}
		chatScreen.SetActive(value: true);
		customNetworkTransform.Halt();
		StartCoroutine(CoOpen());
		if (DestroyableSingleton<FriendsListManager>.InstanceExists)
		{
			DestroyableSingleton<FriendsListManager>.Instance.SetFriendButtonColor(isGrayedOut: true);
		}
		if (chatNotification.gameObject.activeSelf)
		{
			chatNotification.Close();
		}
	}

	public void HideBanButton()
	{
		banButton.Hide();
	}

	public void UpdateVoteTimerText(StringNames key, int value)
	{
		quickChatMenu.UpdateVoteTimerText(key, value);
	}

	private IEnumerator CoOpen()
	{
		CheckKeyboardButton();
		state = ChatControllerState.Opening;
		float duration = chatAnimationIn.keys.Last().time;
		chatButtonAspectPosition.AdjustPosition();
		chatButton.SelectButton(isSelected: true);
		quickChatMenu.Clear();
		quickChatField.Clear();
		quickChatMenu.Close();
		banButton.Hide();
		banButton.SetVisible(show: true);
		if (DestroyableSingleton<HudManager>.InstanceExists)
		{
			DestroyableSingleton<HudManager>.Instance.ToggleMapButtonGlyph(isActive: false);
		}
		if (DestroyableSingleton<GameStartManager>.InstanceExists)
		{
			DestroyableSingleton<GameStartManager>.Instance.ToggleButtonGlyphs(enabled: false);
		}
		ConsoleJoystick.SetMode_QuickChat();
		ControllerManager.Instance.OpenOverlayMenu(name, backButton, defaultButtonSelected, controllerSelectable);
		if (Controller.currentTouchType != Controller.TouchType.Joystick)
		{
			freeChatField.Focus();
		}
		Vector3 scale = Vector3.one;
		targetScale = AspectSize.CalculateSize(chatButton.transform.localPosition, backgroundImage.sprite, 0.8f);
		targetChatPosition = chatButton.transform.localPosition;
		float timer = 0f;
		while (timer < duration)
		{
			timer += Time.deltaTime;
			float t = chatAnimationIn.Evaluate(timer);
			scale.y = (scale.x = Mathf.LerpUnclamped(0.01f, targetScale, t));
			chatScreen.transform.localScale = scale;
			yield return null;
		}
		chatNotifyDot.enabled = false;
		state = ChatControllerState.Open;
	}

	private IEnumerator CoClose()
	{
		state = ChatControllerState.Closing;
		float duration = chatAnimationOut.keys.Last().time;
		chatButton.SelectButton(isSelected: false);
		quickChatMenu.Close();
		banButton.Hide();
		if (DestroyableSingleton<HudManager>.InstanceExists)
		{
			DestroyableSingleton<HudManager>.Instance.ToggleMapButtonGlyph(isActive: true);
		}
		if (DestroyableSingleton<GameStartManager>.InstanceExists)
		{
			DestroyableSingleton<GameStartManager>.Instance.ToggleButtonGlyphs(enabled: true);
		}
		openKeyboardButton.SetActive(value: false);
		freeChatField.Unfocus();
		ConsoleJoystick.SetMode_None();
		Vector3 scale = Vector3.one;
		float targetScale = AspectSize.CalculateSize(chatButton.transform.localPosition, backgroundImage.sprite, 0.8f);
		targetChatPosition = chatButton.transform.localPosition;
		for (float timer = 0f; timer < duration; timer += Time.deltaTime)
		{
			float t = chatAnimationOut.Evaluate(timer);
			scale.y = (scale.x = Mathf.LerpUnclamped(targetScale, 0.01f, t));
			chatScreen.transform.localScale = scale;
			yield return null;
		}
		banButton.SetVisible(show: false);
		chatScreen.SetActive(value: false);
		ConsoleJoystick.SetMode_Menu();
		ControllerManager.Instance.CloseOverlayMenu(name);
		state = ChatControllerState.Closed;
		freeChatField.ForceKeyboardClose();
	}

	public void AddChatNote(NetworkedPlayerInfo srcPlayer, ChatNoteTypes noteType)
	{
		if (srcPlayer == null)
		{
			return;
		}
		ChatBubble pooledBubble = GetPooledBubble();
		pooledBubble.SetCosmetics(srcPlayer);
		pooledBubble.transform.SetParent(scroller.Inner);
		pooledBubble.transform.localScale = Vector3.one;
		pooledBubble.SetNotification();
		if (noteType == ChatNoteTypes.DidVote)
		{
			int rem = MeetingHud.Instance.GetVotesRemaining();
			SetChatBubbleName(pooledBubble, srcPlayer, isDead: false, didVote: true, Color.green, (string returnedName) => DestroyableSingleton<TranslationController>.Instance.GetString(StringNames.MeetingHasVoted, returnedName, rem));
		}
		pooledBubble.SetText(string.Empty);
		pooledBubble.AlignChildren();
		AlignAllBubbles();
		if (!IsOpenOrOpening && notificationRoutine == null)
		{
			notificationRoutine = StartCoroutine(BounceDot());
		}
		if (srcPlayer.Object != PlayerControl.LocalPlayer)
		{
			SoundManager.Instance.PlaySound(messageSound, loop: false).pitch = 0.5f + (float)(int)srcPlayer.PlayerId / 15f;
		}
	}

	public void AddChat(PlayerControl sourcePlayer, string chatText, bool censor = true)
	{
		if (!sourcePlayer || !PlayerControl.LocalPlayer)
		{
			return;
		}
		NetworkedPlayerInfo data = PlayerControl.LocalPlayer.Data;
		NetworkedPlayerInfo data2 = sourcePlayer.Data;
		if (data2 == null || data == null || (data2.IsDead && !data.IsDead))
		{
			return;
		}
		ChatBubble pooledBubble = GetPooledBubble();
		try
		{
			pooledBubble.transform.SetParent(scroller.Inner);
			pooledBubble.transform.localScale = Vector3.one;
			bool flag = sourcePlayer == PlayerControl.LocalPlayer;
			if (flag)
			{
				pooledBubble.SetRight();
			}
			else
			{
				pooledBubble.SetLeft();
			}
			bool didVote = (bool)MeetingHud.Instance && MeetingHud.Instance.DidVote(sourcePlayer.PlayerId);
			pooledBubble.SetCosmetics(data2);
			SetChatBubbleName(pooledBubble, data2, data2.IsDead, didVote, PlayerNameColor.Get(data2));
			if (censor && DataManager.Settings.Multiplayer.CensorChat)
			{
				chatText = BlockedWords.CensorWords(chatText);
			}
			pooledBubble.SetText(chatText);
			pooledBubble.AlignChildren();
			AlignAllBubbles();
			if (!IsOpenOrOpening && notificationRoutine == null)
			{
				notificationRoutine = StartCoroutine(BounceDot());
			}
			if (!flag && !IsOpenOrOpening)
			{
				SoundManager.Instance.PlaySound(messageSound, loop: false).pitch = 0.5f + (float)(int)sourcePlayer.PlayerId / 15f;
				chatNotification.SetUp(sourcePlayer, chatText);
			}
		}
		catch (Exception message)
		{
			Logger.Error(message);
			chatBubblePool.Reclaim(pooledBubble);
		}
	}

	public void AddChatWarning(string warningText)
	{
		ChatBubble pooledBubble = GetPooledBubble();
		try
		{
			pooledBubble.transform.SetParent(scroller.Inner);
			pooledBubble.transform.localScale = Vector3.one;
			pooledBubble.SetRight();
			pooledBubble.SetWarning(warningText);
			pooledBubble.AlignChildren();
			AlignAllBubbles();
			if (!IsOpenOrOpening && notificationRoutine == null)
			{
				notificationRoutine = StartCoroutine(BounceDot());
			}
			SoundManager.Instance.PlaySound(warningSound, loop: false);
		}
		catch (Exception message)
		{
			Logger.Error(message);
			chatBubblePool.Reclaim(pooledBubble);
		}
	}

	private void SetChatBubbleName(ChatBubble bubble, NetworkedPlayerInfo playerInfo, bool isDead, bool didVote, Color nameColor, GetFormattedNameFunc getFormattedNameFunc = null)
	{
		if (!string.IsNullOrEmpty(playerInfo.PlayerName))
		{
			string playerName = ((getFormattedNameFunc == null) ? playerInfo.PlayerName : getFormattedNameFunc(playerInfo.PlayerName));
			bubble.SetName(playerName, isDead, didVote, nameColor);
			return;
		}
		bubble.SetName("...", isDead, didVote, nameColor);
		StartCoroutine(WaitForNameToExist(playerInfo, (NetworkedPlayerInfo player) =>
		{
			string playerName2 = ((getFormattedNameFunc == null) ? playerInfo.PlayerName : getFormattedNameFunc(playerInfo.PlayerName));
			bubble.SetName(playerName2, isDead, didVote, nameColor);
			bubble.AlignChildren();
			AlignAllBubbles();
		}));
	}

	private IEnumerator WaitForNameToExist(NetworkedPlayerInfo player, Action<NetworkedPlayerInfo> onNameExistsCallback)
	{
		yield return null;
		while (string.IsNullOrEmpty(player.PlayerName))
		{
			yield return null;
		}
		onNameExistsCallback?.Invoke(player);
	}

	private void AlignAllBubbles()
	{
		float num = 0f;
		List<PoolableBehavior> activeChildren = chatBubblePool.activeChildren;
		for (int num2 = activeChildren.Count - 1; num2 >= 0; num2--)
		{
			ChatBubble chatBubble = activeChildren[num2] as ChatBubble;
			num += chatBubble.Background.size.y;
			Vector3 localPosition = chatBubble.transform.localPosition;
			localPosition.y = -1.85f + num;
			chatBubble.transform.localPosition = localPosition;
			num += 0.15f;
		}
		float num3 = -0.3f;
		scroller.SetYBoundsMin(Mathf.Min(0f, 0f - num + scroller.Hitbox.bounds.size.y + num3));
	}

	private IEnumerator BounceDot()
	{
		chatNotifyDot.enabled = true;
		yield return Effects.Bounce(chatNotifyDot.transform);
		notificationRoutine = null;
	}

	private ChatBubble GetPooledBubble()
	{
		if (chatBubblePool.NotInUse == 0)
		{
			chatBubblePool.ReclaimOldest();
		}
		return chatBubblePool.Get<ChatBubble>();
	}

	private void OnFreeChatSubmit()
	{
		SendChat();
	}

	private void OnFreeChatChanged()
	{
		quickChatMenu.Clear();
		UpdateChatMode();
	}

	private void SendFreeChat()
	{
		string text = freeChatField.Text;
		if (UrlFinder.TryFindUrl(text.ToCharArray(), out var _, out var _))
		{
			Logger.Warning(string.Format("{0}() :: ABORTED, URL was found. Showing {1} instead!", "SendFreeChat", StringNames.FreeChatLinkWarning));
			AddChatWarning(DestroyableSingleton<TranslationController>.Instance.GetString(StringNames.FreeChatLinkWarning));
		}
		else
		{
			Logger.Debug("SendFreeChat () :: Sending message: '" + text + "'");
			PlayerControl.LocalPlayer.RpcSendChat(text);
		}
	}

	private void OpenQuickChat()
	{
		float num = targetScale * backgroundImage.sprite.bounds.size.x;
		float num2 = 3.5f;
		Camera uICamera = DestroyableSingleton<HudManager>.Instance.UICamera;
		float num3 = num + -0.39f + num2 + 0.1f;
		float num4 = CameraSafeArea.GetSafeOrthographicSize(uICamera) * 2f * uICamera.aspect;
		if (num3 > num4)
		{
			num2 -= Mathf.Clamp(num3 - num4, 0f, 100f);
		}
		quickChatMenu.Open(Mathf.Max(num2, 2.5f));
	}

	private void OnQuickChatOpened()
	{
		chatButton.gameObject.SetActive(value: false);
		ConsoleJoystick.SetMode_QuickChat();
		UpdateChatMode();
	}

	private void OnQuickChatClosed()
	{
		chatButton.gameObject.SetActive(value: true);
		UpdateChatMode();
	}

	private void OnQuickChatSubmit()
	{
		if (!quickChatMenu.CanSend)
		{
			quickChatField.AnimateInvalidSend();
		}
		else
		{
			SendChat();
		}
	}

	private void OnQuickChatClearPressed()
	{
		quickChatMenu.Clear();
	}

	private void OnQuickChatUndoPressed()
	{
		quickChatMenu.Undo();
	}

	private void OnQuickChatBuilderChanged(QuickChatPhraseBuilder builder)
	{
		quickChatField.UpdatePreview(builder);
		UpdateChatMode();
	}

	private void OnQuickChatButtonClicked()
	{
		if (!IsClosedOrClosing && !quickChatMenu.IsAnimating)
		{
			if (quickChatMenu.IsOpen)
			{
				quickChatMenu.Close();
			}
			else
			{
				OpenQuickChat();
			}
		}
	}

	private void SendQuickChat()
	{
		StringBuilder stringBuilder = new StringBuilder();
		stringBuilder.AppendLine("SendQuickChat() Sending message (see stack for more info)");
		quickChatMenu.BuildMessageLog(stringBuilder);
		Logger.Info(stringBuilder);
		PlayerControl.LocalPlayer.RpcSendQuickChat(quickChatMenu.BuildMessageData());
		quickChatMenu.Clear();
		quickChatField.Clear();
	}
}
