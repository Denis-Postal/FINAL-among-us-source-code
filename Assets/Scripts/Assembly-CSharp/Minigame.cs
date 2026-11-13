using System;
using System.Collections;
using UnityEngine;

public abstract class Minigame : MonoBehaviour
{
	protected enum CloseState
	{
		None,
		Waiting,
		Closing
	}

	public static Minigame Instance;

	public const float Depth = -50f;

	public TransitionType TransType;

	public Vector2 TargetPosition;

	protected PlayerTask MyTask;

	protected NormalPlayerTask MyNormTask;

	protected CloseState amClosing;

	protected bool amOpening;

	public AudioClip OpenSound;

	public AudioClip CloseSound;

	[NonSerialized]
	public SpecialInputHandler inputHandler;

	private bool multistageMinigameChecked;

	private MultistageMinigame multistageMinigameParent;

	private readonly Logger logger = new Logger(Logger.Category.Gameplay, "Minigame");

	private float timeOpened;

	public TaskTypes TaskType
	{
		get
		{
			if (!MyTask)
			{
				return TaskTypes.None;
			}
			return MyTask.TaskType;
		}
	}

	public Console Console { get; set; }

	protected int ConsoleId
	{
		get
		{
			if (!Console)
			{
				return 0;
			}
			return Console.ConsoleId;
		}
	}

	public bool HasMultistageMinigameParent => GetMultistageMinigameParentOnce() != null;

	public MultistageMinigame MultistageMinigameParent => GetMultistageMinigameParentOnce();

	public virtual bool SkipMultistageOverlayMenuSetup => false;

	public virtual void Begin(PlayerTask task)
	{
		Instance = this;
		MyTask = task;
		MyNormTask = task as NormalPlayerTask;
		timeOpened = Time.realtimeSinceStartup;
		if ((bool)PlayerControl.LocalPlayer)
		{
			if ((bool)MapBehaviour.Instance)
			{
				MapBehaviour.Instance.Close();
			}
			PlayerControl.LocalPlayer.MyPhysics.SetNormalizedVelocity(Vector2.zero);
		}
		logger.Info("Opening minigame " + GetType().Name);
		StartCoroutine(CoAnimateOpen());
		DestroyableSingleton<DebugAnalytics>.Instance.Analytics.MinigameOpened(PlayerControl.LocalPlayer.Data, TaskType);
	}

	protected IEnumerator CoStartClose(float duration = 0.75f)
	{
		if (amClosing == CloseState.None)
		{
			amClosing = CloseState.Waiting;
			yield return Effects.Wait(duration);
			Close();
		}
	}

	[Obsolete("Don't use, I just don't want to reselect the close button event handlers", true)]
	public void Close(bool allowMovement)
	{
		Close();
	}

	public virtual void ForceClose()
	{
		Close();
		UnityEngine.Object.Destroy(gameObject);
	}

	public virtual void Close()
	{
		if (amClosing != CloseState.Closing)
		{
			if ((bool)CloseSound && Constants.ShouldPlaySfx())
			{
				SoundManager.Instance.PlaySound(CloseSound, loop: false);
			}
			if (PlayerControl.LocalPlayer.Data.Role.TeamType == RoleTeamTypes.Crewmate)
			{
				GameManager.Instance.LogicMinigame.OnMinigameClose();
			}
			if ((bool)PlayerControl.LocalPlayer)
			{
				PlayerControl.HideCursorTemporarily();
			}
			amClosing = CloseState.Closing;
			logger.Info("Closing minigame " + GetType().Name);
			DestroyableSingleton<DebugAnalytics>.Instance.Analytics.MinigameClosed(PlayerControl.LocalPlayer.Data, TaskType, Time.realtimeSinceStartup - timeOpened, MyTask?.IsComplete ?? false);
			StartCoroutine(CoDestroySelf());
		}
		else
		{
			UnityEngine.Object.Destroy(gameObject);
		}
	}

	protected virtual IEnumerator CoAnimateOpen()
	{
		amOpening = true;
		if ((bool)OpenSound && Constants.ShouldPlaySfx())
		{
			SoundManager.Instance.PlaySound(OpenSound, loop: false);
		}
		float depth = transform.localPosition.z;
		switch (TransType)
		{
		case TransitionType.None:
			transform.localPosition = new Vector3(TargetPosition.x, TargetPosition.y, depth);
			break;
		case TransitionType.SlideBottom:
		{
			for (float timer = 0f; timer < 0.25f; timer += Time.deltaTime)
			{
				float t2 = timer / 0.25f;
				transform.localPosition = new Vector3(TargetPosition.x, Mathf.SmoothStep(-8f, TargetPosition.y, t2), depth);
				yield return null;
			}
			transform.localPosition = new Vector3(TargetPosition.x, TargetPosition.y, depth);
			break;
		}
		case TransitionType.Alpha:
		{
			SpriteRenderer[] rends = GetComponentsInChildren<SpriteRenderer>();
			for (float timer = 0f; timer < 0.25f; timer += Time.deltaTime)
			{
				float t = timer / 0.25f;
				for (int i = 0; i < rends.Length; i++)
				{
					rends[i].color = Color.Lerp(Palette.ClearWhite, Color.white, t);
				}
				yield return null;
			}
			for (int j = 0; j < rends.Length; j++)
			{
				rends[j].color = Color.white;
			}
			break;
		}
		}
		if (PlayerControl.LocalPlayer.Data.Role.TeamType == RoleTeamTypes.Crewmate)
		{
			GameManager.Instance.LogicMinigame.OnMinigameOpen();
		}
		amOpening = false;
	}

	protected virtual IEnumerator CoDestroySelf()
	{
		switch (TransType)
		{
		case TransitionType.SlideBottom:
		{
			for (float timer = 0f; timer < 0.25f; timer += Time.deltaTime)
			{
				float t2 = timer / 0.25f;
				transform.localPosition = new Vector3(0f, Mathf.SmoothStep(0f, -8f, t2), -50f);
				yield return null;
			}
			break;
		}
		case TransitionType.Alpha:
		{
			SpriteRenderer[] rends = GetComponentsInChildren<SpriteRenderer>();
			for (float timer = 0f; timer < 0.25f; timer += Time.deltaTime)
			{
				float t = timer / 0.25f;
				for (int i = 0; i < rends.Length; i++)
				{
					rends[i].color = Color.Lerp(Color.white, Palette.ClearWhite, t);
				}
				yield return null;
			}
			break;
		}
		}
		UnityEngine.Object.Destroy(gameObject);
	}

	public void SetupInput(bool disableCursor, bool suppressInGamePlayerList = false)
	{
		inputHandler = GetComponentInChildren<SpecialInputHandler>(includeInactive: true);
		if (inputHandler == null)
		{
			inputHandler = gameObject.AddComponent<SpecialInputHandler>();
		}
		if (disableCursor)
		{
			inputHandler.disableVirtualCursor = true;
		}
		if (suppressInGamePlayerList)
		{
			inputHandler.suppressInGamePlayerList = true;
		}
		ConsoleJoystick.SetMode_Task();
	}

	private MultistageMinigame GetMultistageMinigameParentOnce()
	{
		if (!multistageMinigameChecked)
		{
			multistageMinigameChecked = true;
			multistageMinigameParent = GetComponentInParent<MultistageMinigame>();
		}
		return multistageMinigameParent;
	}
}
