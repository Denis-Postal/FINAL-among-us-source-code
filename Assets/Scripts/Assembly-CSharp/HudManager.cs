using System;
using System.Collections;
using System.Linq;
using System.Text;
using AmongUs.Data;
using AmongUs.GameOptions;
using UnityEngine;

public class HudManager : DestroyableSingleton<HudManager>
{
	public FollowerCamera PlayerCam;

	public Camera UICamera;

	public MeetingHud MeetingPrefab;

	public KillButton KillButton;

	public AdminButton AdminButton;

	public SabotageButton SabotageButton;

	public VentButton ImpostorVentButton;

	public UseButton UseButton;

	public PetButton PetButton;

	public AbilityButton AbilityButton;

	public SecondaryAbilityButton SecondaryAbilityButton;

	public ReportButton ReportButton;

	public GameObject TaskStuff;

	public TaskPanelBehaviour TaskPanel;

	public CrewmatesKilledTracker CrewmatesKilled;

	public ChatController Chat;

	public DialogueBox Dialogue;

	public Transform TaskCompleteOverlay;

	private float taskDirtyTimer;

	public MeshRenderer ShadowQuad;

	public SpriteRenderer FullScreen;

	public PassiveButton MapButton;

	public GameObject MapButtonGlyph;

	public PassiveButton MatchInfoButton;

	public GameObject MatchInfoButtonGlyph;

	public KillOverlay KillOverlay;

	public IVirtualJoystick joystick;

	public VirtualJoystick joystickR;

	public MonoBehaviour[] Joysticks;

	public MonoBehaviour RightVJoystick;

	public Collider2D LeftStickDeadZone;

	public Collider2D RightStickDeadZone;

	public DiscussBehaviour discussEmblem;

	public ShhhBehaviour shhhEmblem;

	public IntroCutscene IntroPrefab;

	public OptionsMenuBehaviour GameMenu;

	public NotificationPopper Notifier;

	public RoomTracker roomTracker;

	public AudioClip TaskCompleteSound;

	public AudioClip TaskUpdateSound;

	public Transform consoleUIRoot;

	public GameObject[] consoleUIObjects;

	public GameObject menuNavigationPrompts;

	public GameObject GameLoadAnimation;

	public LobbyTimerExtensionUI LobbyTimerExtensionUI;

	[NonSerialized]
	public float consoleUIHorizontalShift;

	public AlertFlash AlertFlash;

	public DangerMeter DangerMeter;

	public GameObject SettingsButton;

	private StringBuilder tasksString = new StringBuilder();

	private DualshockLightManager.LightOverlayHandle lightFlashHandle;

	public Coroutine ReactorFlash { get; set; }

	public Coroutine OxyFlash { get; set; }

	public bool IsIntroDisplayed { get; private set; }

	public void Start()
	{
		if (ShadowQuad != null)
		{
			ShadowQuad.enabled = false;
		}
		Camera mainCamera = Camera.main;
		if (mainCamera != null)
		{
			Transform shadowCamera = mainCamera.transform.Find("ShadowCamera");
			if (shadowCamera != null)
			{
				shadowCamera.gameObject.SetActive(false);
			}
		}
		DelayedInitTouchType();
		DebugToolInstantiator.EnsureOverlay(); // debug menu (BackQuote) in every in-game scene
		MapButton.OnClick.AddListener(() =>
		{
			ToggleMapVisible(GameManager.Instance.GetMapOptions());
		});
	}

	public void OnGameStart()
	{
		Logger.GlobalInstance.Info("[HudManager] HudManager initializing on game start.");
		DelayedInitTouchType();
		LobbyTimerExtensionUI.HideAll();
	}

	public void ShowTaskComplete()
	{
		StartCoroutine(CoTaskComplete());
	}

	private IEnumerator CoTaskComplete()
	{
		if (Constants.ShouldPlaySfx())
		{
			SoundManager.Instance.PlaySound(TaskCompleteSound, loop: false);
		}
		TaskCompleteOverlay.gameObject.SetActive(value: true);
		yield return Effects.Slide2D(TaskCompleteOverlay, new Vector2(0f, -8f), Vector2.zero, 0.25f);
		for (float time = 0f; time < 0.75f; time += Time.deltaTime)
		{
			yield return null;
		}
		yield return Effects.Slide2D(TaskCompleteOverlay, Vector2.zero, new Vector2(0f, 8f), 0.25f);
		TaskCompleteOverlay.gameObject.SetActive(value: false);
	}

	public void SetJoystickSize(float size)
	{
		if (joystick != null && joystick is VirtualJoystick)
		{
			float num = Mathf.Lerp(0.65f, 1.1f, FloatRange.ReverseLerp(size, 0.5f, 1.5f));
			SetVirtualJoystickSize((VirtualJoystick)joystick, size, new Vector2(num, num));
			if (joystickR != null)
			{
				num = Mathf.Lerp(0.65f, 1.1f, FloatRange.ReverseLerp(size, 0.5f, 1.5f));
				SetVirtualJoystickSize(joystickR, size, new Vector2(num + 0.4f, num - 0.6f));
			}
		}
	}

	private void SetVirtualJoystickSize(VirtualJoystick jStick, float size, Vector2 dist)
	{
		jStick.transform.localScale = new Vector3(size, size, 1f);
		AspectPosition component = jStick.GetComponent<AspectPosition>();
		component.DistanceFromEdge = new Vector3(dist.x, dist.y, -10f);
		component.AdjustPosition();
	}

	public void SetTouchType(ControlTypes type)
	{
		if (joystick != null)
		{
			// fix4a: destroy the old joystick unconditionally (null-safe) so two keyboard joysticks never coexist
			MonoBehaviour oldJoystick = joystick as MonoBehaviour;
			if (oldJoystick != null)
			{
				UnityEngine.Object.Destroy(oldJoystick.gameObject);
			}
			joystick = null;
		}
		if (joystickR != null)
		{
			UnityEngine.Object.Destroy(joystickR.gameObject);
			joystickR = null;
		}
		MonoBehaviour monoBehaviour = null;
		monoBehaviour = UnityEngine.Object.Instantiate(Joysticks[(int)type]);
		if (monoBehaviour != null)
		{
			monoBehaviour.transform.SetParent(transform, worldPositionStays: false);
			joystick = monoBehaviour.GetComponent<IVirtualJoystick>();
		}
		GameOptionsManager.Instance.CurrentGameOptions.TryGetBool(BoolOptionNames.UseFlashlight, out var value);
		if (value && type == ControlTypes.VirtualJoystick)
		{
			MonoBehaviour monoBehaviour2 = null;
			monoBehaviour2 = UnityEngine.Object.Instantiate(RightVJoystick);
			if (monoBehaviour2 != null)
			{
				monoBehaviour2.transform.SetParent(transform, worldPositionStays: false);
				joystickR = monoBehaviour2.GetComponent<VirtualJoystick>();
				joystickR.ToggleVisuals(LobbyBehaviour.Instance == null);
				Logger.GlobalInstance.Info(string.Format("[{0}] Initializing Right Joystick for Flashlight. [Use Flashlight: {1}] [Lobby: {2}]", "HudManager", value, LobbyBehaviour.Instance != null));
			}
		}
		SetJoystickSize(DataManager.Settings.Input.TouchJoystickSize);
	}

	public void InitMap()
	{
		if (!MapBehaviour.Instance)
		{
			MapBehaviour.Instance = UnityEngine.Object.Instantiate(ShipStatus.Instance.MapPrefab, transform);
			MapBehaviour.Instance.gameObject.SetActive(value: false);
		}
	}

	public void DeselectMapButton()
	{
		MapButton.SelectButton(isSelected: false);
	}

	public void ToggleMapVisible(MapOptions options)
	{
		if ((bool)MapBehaviour.Instance && MapBehaviour.Instance.gameObject.activeSelf)
		{
			MapBehaviour.Instance.Close();
		}
		else if (!IsIntroDisplayed && (!MatchInfoGuide.Instance || !MatchInfoGuide.Instance.IsActive) && (bool)ShipStatus.Instance)
		{
			InitMap();
			MapBehaviour.Instance.Show(options);
			MapButton.SelectButton(isSelected: true);
		}
	}

	public void SetHudActive(PlayerControl localPlayer, RoleBehaviour role, bool isActive)
	{
		if (role != null && role.Ability != null && role.Ability.AbilityType != AbilityTypes.MeetingOnly && role.Ability.AbilityType != AbilityTypes.Limited)
		{
			AbilityButton.ToggleVisible(isActive);
		}
		else
		{
			AbilityButton.ToggleVisible(visible: false);
		}
		if (role != null && role.SecondaryAbility != null && role.SecondaryAbility.AbilityType != AbilityTypes.MeetingOnly && role.SecondaryAbility.AbilityType != AbilityTypes.Limited)
		{
			SecondaryAbilityButton.ToggleVisible(isActive);
		}
		else
		{
			SecondaryAbilityButton.ToggleVisible(visible: false);
		}
		if (isActive)
		{
			UseButton.Refresh();
			if (role != null && role.Ability != null && role.Ability.AbilityType != AbilityTypes.MeetingOnly && role.Ability.AbilityType != AbilityTypes.Limited)
			{
				AbilityButton.Refresh(role.Ability);
			}
			if (role != null && role.SecondaryAbility != null && role.SecondaryAbility.AbilityType != AbilityTypes.MeetingOnly && role.SecondaryAbility.AbilityType != AbilityTypes.Limited)
			{
				SecondaryAbilityButton.Refresh(role.SecondaryAbility);
			}
		}
		else
		{
			UseButton.ToggleVisible(visible: false);
			PetButton.ToggleVisible(visible: false);
		}
		bool flag = !(localPlayer.Data == null) && localPlayer.Data.IsDead;
		ReportButton.ToggleVisible(isActive && !flag && GameManager.Instance.CanReportBodies() && ShipStatus.Instance != null);
		KillButton.ToggleVisible(isActive && role.IsImpostor && role.CanUseKillButton && !flag);
		SabotageButton.ToggleVisible(isActive && role.IsImpostor);
		AdminButton.ToggleVisible(isActive && role.IsImpostor);
		ImpostorVentButton.ToggleVisible(isActive && !flag && role.IsImpostor && GameOptionsManager.Instance.CurrentGameOptions.GameMode != GameModes.HideNSeek);
		TaskPanel.gameObject.SetActive(isActive);
		roomTracker.gameObject.SetActive(isActive);
		if (joystick != null)
		{
			joystick.ToggleVisuals(isActive);
		}
		ToggleRightJoystick(isActive);
	}

	public void UpdateVoteTimerText(StringNames key, int value)
	{
		Chat.UpdateVoteTimerText(key, value);
	}

	public void SetHudActive(bool isActive)
	{
		SetHudActive(PlayerControl.LocalPlayer, PlayerControl.LocalPlayer.Data.Role, isActive);
	}

	public void ToggleRightJoystick(bool isActive)
	{
		if (joystickR != null)
		{
			joystickR.ToggleVisuals(isActive);
		}
	}

	public void ToggleMapButton(bool isActive)
	{
		MapButton.gameObject.SetActive(isActive);
	}

	public void ToggleMapButtonGlyph(bool isActive)
	{
		MapButtonGlyph.SetActive(isActive);
	}

	public void SetMapAndInfoButtonsEnabled(bool enabled)
	{
		if (MapButton != null)
		{
			MapButton.enabled = enabled;
		}
		if (MatchInfoButton != null)
		{
			MatchInfoButton.enabled = enabled;
		}
	}

	public void ToggleSettingsButton(bool isActive)
	{
		SettingsButton.SetActive(isActive);
	}

	public void Update()
	{
		if (consoleUIRoot.transform.localPosition.x != consoleUIHorizontalShift)
		{
			Vector3 localPosition = consoleUIRoot.transform.localPosition;
			localPosition.x = consoleUIHorizontalShift;
			consoleUIRoot.transform.localPosition = localPosition;
		}
		if (joystickR != null && LobbyBehaviour.Instance != null)
		{
			joystickR.ToggleVisuals(on: false);
		}
		taskDirtyTimer += Time.deltaTime;
		if (!(taskDirtyTimer > 0.25f))
		{
			return;
		}
		float num = taskDirtyTimer;
		taskDirtyTimer = 0f;
		if (!PlayerControl.LocalPlayer)
		{
			TaskPanel.SetTaskText(string.Empty);
			return;
		}
		NetworkedPlayerInfo data = PlayerControl.LocalPlayer.Data;
		if (data == null)
		{
			return;
		}
		bool flag = data.Role != null && data.Role.IsImpostor;
		tasksString.Clear();
		if (PlayerControl.LocalPlayer.myTasks == null || PlayerControl.LocalPlayer.myTasks.Count == 0)
		{
			tasksString.Append("None");
		}
		else
		{
			for (int i = 0; i < PlayerControl.LocalPlayer.myTasks.Count; i++)
			{
				PlayerTask playerTask = PlayerControl.LocalPlayer.myTasks[i];
				if ((bool)playerTask)
				{
					if (playerTask.TaskType == TaskTypes.FixComms && !flag)
					{
						tasksString.Clear();
						playerTask.AppendTaskText(tasksString);
						break;
					}
					playerTask.AppendTaskText(tasksString);
				}
			}
			if (data.Role != null)
			{
				data.Role.AppendTaskHint(tasksString);
			}
			if (GameOptionsManager.Instance.CurrentGameOptions.GameMode == GameModes.HideNSeek && ShipStatus.Instance.HideCountdown > 0f)
			{
				ShipStatus.Instance.HideCountdown -= num;
				tasksString.Append("\n\n" + (int)ShipStatus.Instance.HideCountdown);
			}
			tasksString.TrimEnd();
		}
		TaskPanel.SetTaskText(tasksString.ToString());
	}

	public IEnumerator ShowEmblem(bool shhh)
	{
		if (shhh)
		{
			shhhEmblem.gameObject.SetActive(value: true);
			yield return shhhEmblem.PlayAnimation();
			shhhEmblem.gameObject.SetActive(value: false);
		}
		else
		{
			discussEmblem.gameObject.SetActive(value: true);
			yield return discussEmblem.PlayAnimation();
			discussEmblem.gameObject.SetActive(value: false);
		}
	}

	public void StartReactorFlash()
	{
		if (ReactorFlash == null)
		{
			ReactorFlash = StartCoroutine(CoReactorFlash());
		}
	}

	public void StartOxyFlash()
	{
		if (OxyFlash == null)
		{
			OxyFlash = StartCoroutine(CoReactorFlash());
		}
	}

	public void ShowPopUp(string text)
	{
		Dialogue.Show(text);
	}

	public void StopReactorFlash()
	{
		if (ReactorFlash != null)
		{
			StopCoroutine(ReactorFlash);
			ReactorFlash = null;
			FullScreen.gameObject.SetActive(value: false);
			PlainShipRoom plainShipRoom = ShipStatus.Instance.AllRooms.FirstOrDefault((PlainShipRoom r) => r is IReactorShipRoom);
			if ((bool)plainShipRoom)
			{
				((IReactorShipRoom)plainShipRoom).StopMeltdown();
			}
			if (lightFlashHandle != null)
			{
				lightFlashHandle.Dispose();
				lightFlashHandle = null;
			}
		}
	}

	public void StopOxyFlash()
	{
		if (OxyFlash != null)
		{
			StopCoroutine(OxyFlash);
			FullScreen.gameObject.SetActive(value: false);
			OxyFlash = null;
			if (lightFlashHandle != null)
			{
				lightFlashHandle.Dispose();
				lightFlashHandle = null;
			}
		}
	}

	public IEnumerator CoFadeFullScreen(Color source, Color target, float duration = 0.2f, bool showLoader = false)
	{
		GameLoadAnimation.SetActive(showLoader);
		if (Chat.IsOpenOrOpening)
		{
			Chat.Close();
		}
		if (FullScreen.gameObject.activeSelf && FullScreen.color == target)
		{
			yield break;
		}
		FullScreen.gameObject.SetActive(value: true);
		for (float t = 0f; t < duration; t += Time.deltaTime)
		{
			if (!FullScreen)
			{
				yield break;
			}
			FullScreen.color = Color.Lerp(source, target, t / duration);
			yield return null;
		}
		if ((bool)FullScreen)
		{
			FullScreen.color = target;
			if (target.a < 0.05f)
			{
				FullScreen.gameObject.SetActive(value: false);
			}
		}
	}

	private IEnumerator CoReactorFlash()
	{
		WaitForSeconds wait = new WaitForSeconds(1f);
		bool light = false;
		FullScreen.color = new Color(1f, 0f, 0f, 95f / 255f);
		while (true)
		{
			FullScreen.gameObject.SetActive(!FullScreen.gameObject.activeSelf);
			SoundManager.Instance.PlaySound(ShipStatus.Instance.SabotageSound, loop: false);
			if (lightFlashHandle == null)
			{
				lightFlashHandle = DestroyableSingleton<DualshockLightManager>.Instance.AllocateLight();
				lightFlashHandle.color = new Color(1f, 0f, 0f, 1f);
				lightFlashHandle.intensity = 1f;
			}
			light = !light;
			lightFlashHandle.color.a = (light ? 1f : 0f);
			yield return wait;
		}
	}

	public IEnumerator CoShowIntro()
	{
		while (!ShipStatus.Instance)
		{
			yield return null;
		}
		IsIntroDisplayed = true;
		LobbyTimerExtensionUI.HideAll();
		SetMapAndInfoButtonsEnabled(enabled: false);
		DestroyableSingleton<HudManager>.Instance.FullScreen.transform.localPosition = new Vector3(0f, 0f, -250f);
		yield return DestroyableSingleton<HudManager>.Instance.ShowEmblem(shhh: true);
		IntroCutscene introCutscene = UnityEngine.Object.Instantiate(IntroPrefab, transform);
		yield return introCutscene.CoBegin();
		PlayerControl.LocalPlayer.SetKillTimer(10f);
		((SabotageSystemType)ShipStatus.Instance.Systems[SystemTypes.Sabotage]).SetInitialSabotageCooldown();
		if (ShipStatus.Instance.Systems.TryGetValue(SystemTypes.Doors, out var value) && value is IDoorSystem)
		{
			((IDoorSystem)value).SetInitialSabotageCooldown();
		}
		yield return ShipStatus.Instance.PrespawnStep();
		PlayerControl.LocalPlayer.AdjustLighting();
		yield return CoFadeFullScreen(Color.black, Color.clear);
		FullScreen.transform.localPosition = new Vector3(0f, 0f, -500f);
		IsIntroDisplayed = false;
		SetMapAndInfoButtonsEnabled(enabled: true);
		CrewmatesKilled.gameObject.SetActive(GameManager.Instance.ShowCrewmatesKilled());
		GameManager.Instance.StartGame();
	}

	public void HideGameLoader()
	{
		GameLoadAnimation.SetActive(value: false);
	}

	public void OpenMeetingRoom(PlayerControl reporter)
	{
		if (!MeetingHud.Instance)
		{
			Debug.Log("Opening meeting room: " + reporter);
			ShipStatus.Instance.RepairCriticalSabotages();
			MeetingHud.Instance = UnityEngine.Object.Instantiate(MeetingPrefab);
			MeetingHud.Instance.ServerStart(reporter.PlayerId);
			AmongUsClient.Instance.Spawn(MeetingHud.Instance);
			GameData.OnMeetingStart();
			Chat.OnMeetingStart();
		}
	}

	public void SetAlertOverlay(bool enabled)
	{
		AlertFlash.SetOverlay(enabled);
	}

	public override void OnDestroy()
	{
		base.OnDestroy();
		if (lightFlashHandle != null)
		{
			lightFlashHandle.Dispose();
			lightFlashHandle = null;
		}
	}

	public void NotifyOfDeath()
	{
		CrewmatesKilled.OnCrewmateKilled();
	}

	public void NotifyOfDisconnect(PlayerControl pc)
	{
		if (!pc.Data.Role.IsImpostor && CrewmatesKilled.isActiveAndEnabled)
		{
			CrewmatesKilled.OnCrewmateDisconnect();
		}
	}

	public void ToggleUseAndPetButton(IUsable useTarget, bool canPlayNormally, bool canPet)
	{
		if (!MapBehaviour.Instance || !MapBehaviour.Instance.gameObject.activeSelf)
		{
			if ((useTarget == null) & canPlayNormally & canPet)
			{
				UseButton.ToggleVisible(visible: false);
				PetButton.ToggleVisible(visible: true);
				PetButton.SetTarget();
				PetButton.SetEnabled();
			}
			else
			{
				UseButton.ToggleVisible(visible: true);
				PetButton.ToggleVisible(visible: false);
				UseButton.SetTarget(useTarget);
			}
		}
	}

	public void ShowLobbyTimer(int timeRemainingSeconds)
	{
		LobbyTimerExtensionUI.ShowLobbyTimer(timeRemainingSeconds);
	}

	public void ShowLobbyTimerPopup(int timeRemainingSeconds, int timeGrantedSeconds)
	{
		LobbyTimerExtensionUI.ShowLobbyTimerPopup(timeRemainingSeconds, timeGrantedSeconds);
	}

	public void OnLobbyTimerExtended()
	{
		LobbyTimerExtensionUI.HideAll();
	}

	private void DelayedInitTouchType()
	{
		StartCoroutine(CoDelayedInitTouchType());
	}

	private IEnumerator CoDelayedInitTouchType()
	{
		yield return new WaitForSeconds(0.3f);
		Logger.GlobalInstance.Info(string.Format("[{0}] InitTouchType : {1} Current Control Type: {2}", "HudManager", "ActiveInputManager", ActiveInputManager.currentControlType));
		if (ActiveInputManager.currentControlType == ActiveInputManager.InputType.Joystick)
		{
			Logger.GlobalInstance.Info(string.Format("[{0}] InitTouchType : {1}", "HudManager", ControlTypes.Controller));
			SetTouchType(ControlTypes.Controller);
		}
		else
		{
			Logger.GlobalInstance.Info(string.Format("[{0}] InitTouchType : {1}", "HudManager", DataManager.Settings.Input.InputMode));
			SetTouchType(DataManager.Settings.Input.InputMode);
		}
	}
}
