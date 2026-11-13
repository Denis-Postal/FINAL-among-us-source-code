using System;
using System.Collections;
using Cpp2ILInjected;
using Rewired;
using UnityEngine;
using UnityEngine.Audio;

public class LiftWeightsMinigame : Minigame
{
	private enum State
	{
		Idle,
		Lifting,
		Dropping
	}

	private const string BAR_FILL_SFX = "liftWeightsBarFillLoop";

	[SerializeField]
	private Transform barbell;

	[SerializeField]
	private Collider2D actionButton;

	[SerializeField]
	private Transform top;

	[SerializeField]
	private Transform bottom;

	[SerializeField]
	private AnimationCurve liftSpeedCurve;

	[SerializeField]
	private AnimationCurve dropSpeedCurve;

	[SerializeField]
	private float secondsToLift = 1f;

	[SerializeField]
	private SpriteRenderer[] counters;

	[SerializeField]
	private Transform fillBarParent;

	[SerializeField]
	private SpriteRenderer fillBar;

	[SerializeField]
	private Transform validIndicator;

	[SerializeField]
	private FloatRange fillBarScaleRange;

	[SerializeField]
	private FloatRange validFillPercentRange;

	[SerializeField]
	private Sprite buffSprite;

	[SerializeField]
	private Sprite weakSprite;

	[SerializeField]
	private SpriteRenderer buffButton;

	[SerializeField]
	private SpriteRenderer hands;

	[SerializeField]
	private AudioClip startLiftingSound;

	[SerializeField]
	private AudioClip barfillSound;

	[SerializeField]
	private AudioClip completeRepSound;

	[SerializeField]
	private AudioClip completeAllRepsSound;

	[SerializeField]
	private AudioClip failRepSound;

	private readonly Controller controller= new Controller();

	private Player player;

	private bool hasInput;

	private State state;

	private float currentBarFillPercent;

	private float liftStartTime;

	private float dropStartTime;

	private float secondsToDrop;

	private AudioSource barfillAudioSource;

	private unsafe void Update()
{
		if (MyNormTask.IsComplete)
		{
			return;
		}
		bool flag = GetTouchInput() || GetControllerInput();
		if (!hasInput && flag)
		{
			StartLifting();
		}
		else if (hasInput == flag && flag)
		{
			ContinueLifting();
		}
		else if (hasInput && !flag)
		{
			EndLifting();
		}
		hasInput = flag;
		if (state == State.Dropping)
		{
			float num = currentBarFillPercent;
			num -= dropSpeedCurve.Evaluate(num);
			SetBarFill(num);
			UpdateBarbellPosition();
			if (num <= 0.01f)
			{
				state = State.Idle;
			}
		}
	}

	public unsafe override void Begin(PlayerTask task)
{
		base.Begin(task);
		SetupInput(disableCursor: true);
		player = ReInput.players.GetPlayer(0);
		state = State.Idle;
		barbell.transform.localPosition = bottom.localPosition;
		SetBarFill(0f);
		UpdateBarbellPosition();
		InitializeValidIndicator();
		MyNormTask.MaxStep = counters.Length;
		MyNormTask.ShowTaskStep = true;
		for (int i = 0; i < MyNormTask.TaskStep && i < counters.Length; i++)
		{
			counters[i].color = Color.green;
		}
		PlayerControl.LocalPlayer.SetPlayerMaterialColors(hands);
		barfillAudioSource = SoundManager.Instance.GetNamedSfxSource("liftWeightsBarFillLoop");
		barfillAudioSource.clip = barfillSound;
		barfillAudioSource.loop = true;
		barfillAudioSource.pitch = 0f;
		barfillAudioSource.volume = 0f;
	}

	private bool GetTouchInput()
{
		controller.Update();
		switch (controller.CheckDrag(actionButton))
		{
		case DragState.TouchStart:
		case DragState.Holding:
		case DragState.Dragging:
			return true;
		default:
			return false;
		}
	}

	public bool GetControllerInput()
{
		if (player.GetButton(21))
		{
			return player.GetButton(24);
		}
		return false;
	}

	private unsafe void StartLifting()
{
		if (state == State.Idle)
		{
			state = State.Lifting;
			liftStartTime = Time.time;
			fillBar.color = Color.white;
			if (Constants.ShouldPlaySfx())
			{
				SoundManager.Instance.PlaySound(startLiftingSound, loop: false);
				barfillAudioSource.volume = 1f;
				barfillAudioSource.Play();
			}
		}
	}

	private void ContinueLifting()
	{
		if (state == State.Lifting)
		{
			float num = Mathf.Clamp01((Time.time - liftStartTime) / secondsToLift);
			SetBarFill(num);
			UpdateBarbellPosition();
			barfillAudioSource.pitch = num;
			if (num >= 1f)
			{
				barfillAudioSource.Stop();
			}
			else
			{
				VibrationManager.Vibrate(0.3f, 0.3f, 0.02f);
			}
		}
	}

	private unsafe void EndLifting()
{
		if (state != State.Lifting)
		{
			return;
		}
		if (validFillPercentRange.Contains(currentBarFillPercent))
		{
			counters[MyNormTask.taskStep].color = Color.green;
			if (Constants.ShouldPlaySfx())
			{
				SoundManager.Instance.PlaySound(completeRepSound, loop: false);
			}
			StartCoroutine(Effects.Bloop(0f, counters[MyNormTask.taskStep].transform, counters[MyNormTask.taskStep].transform.localScale.x));
			MyNormTask.NextStep();
			VibrationManager.Vibrate(0.7f, 0.7f, 0.2f);
			if (MyNormTask.IsComplete)
			{
				if (Constants.ShouldPlaySfx())
				{
					StartCoroutine(Effects.Sequence(Effects.Wait(0.1f), Effects.Action(() =>
					{
						SoundManager.Instance.PlaySound(completeAllRepsSound, loop: false);
					}), CoStartClose()));
				}
				else
				{
					StartCoroutine(CoStartClose());
				}
			}
		}
		else
		{
			fillBar.color = Color.red;
			if (Constants.ShouldPlaySfx())
			{
				SoundManager.Instance.PlaySound(failRepSound, loop: false);
			}
		}
		barfillAudioSource.Stop();
		barfillAudioSource.volume = 0f;
		state = State.Dropping;
	}

	private unsafe void SetBarFill(float t)
{
		currentBarFillPercent = t;
		fillBarParent.localScale = new Vector3(fillBarScaleRange.Lerp(t), fillBarParent.localScale.y, 1f);
		if (validFillPercentRange.Contains(currentBarFillPercent))
		{
			if (buffButton.sprite != buffSprite)
			{
				buffButton.sprite = buffSprite;
			}
		}
		else if (buffButton.sprite != weakSprite)
		{
			buffButton.sprite = weakSprite;
		}
	}

	private unsafe void UpdateBarbellPosition()
{
		float time = Mathf.Clamp01(fillBarParent.localScale.x / (validFillPercentRange.max * fillBarScaleRange.Width));
		barbell.transform.localPosition = Vector3.Lerp(bottom.transform.localPosition, top.transform.localPosition, liftSpeedCurve.Evaluate(time));
	}

	private void OnValidate()
	{
		InitializeValidIndicator();
	}

	private unsafe void InitializeValidIndicator()
{
		Vector3 localPosition = validIndicator.transform.localPosition;
		localPosition.x = validFillPercentRange.min * fillBarScaleRange.Width;
		validIndicator.transform.localPosition = localPosition;
		validIndicator.localScale = new Vector3(validFillPercentRange.Width * fillBarScaleRange.Width, validIndicator.localScale.y, 1f);
	}

	public override void Close()
	{
		barfillAudioSource.Stop();
		base.Close();
	}

	public LiftWeightsMinigame()
	{ }

	private void _003CEndLifting_003Eb__39_0()
	{
		SoundManager instance = SoundManager.Instance;
		AudioMixerGroup audioMixer = default;
		AudioSource audioSource = instance.PlaySound(completeAllRepsSound, loop: false, 1f, audioMixer);
	}
}
