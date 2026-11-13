using System.Collections;
using System.Collections.Generic;
using Rewired;
using UnityEngine;

public class CrankGeneratorMinigame : Minigame
{
	private static readonly string SHADER_Y_PERCENT = "_PercentY";

	private static readonly string CRANK_MOTOR_SFX = "crankMotorLoop";

	[Tooltip("Indicates how much spinning its needed to complete task")]
	[SerializeField]
	private float targetProgress = 2000f;

	[SerializeField]
	[Tooltip("How fast does machine loses power when not spinning")]
	private float progressDecay = 250f;

	[Tooltip("CrankAudioPlays targetprogress/lightbulbRate")]
	[SerializeField]
	private int crankAudioRate;

	[SerializeField]
	private SpriteRenderer powerBarSpriteRenderer;

	[SerializeField]
	private Collider2D crank;

	[SerializeField]
	private Transform checkpoint;

	[SerializeField]
	private SpriteRenderer lightbulb;

	[SerializeField]
	private SpriteRenderer miniLights;

	[SerializeField]
	private AudioClip[] turnAudioClips;

	[SerializeField]
	private AudioClip motorAudioClip;

	[Tooltip("Multiplier to make cranking easier/harder on gamepad")]
	private float gamepadAngleProgressMul = 0.75f;

	private readonly Controller controller = new Controller();

	private Player gamepad;

	private Vector2? prevGamepadAngle;

	private float progress;

	private float currentAngle;

	private bool lightbulbEffect;

	private int crankAudioDiv;

	private AudioSource motorAudioSource;

	private List<AudioClip> randomAudioClips;

	private bool finishCrank;

	private void Update()
	{
		if (finishCrank)
		{
			return;
		}
		controller.Update();
		switch (controller.CheckDrag(crank))
		{
		case DragState.TouchStart:
			currentAngle = Vector3.SignedAngle(checkpoint.transform.up, crank.transform.up, Vector3.forward);
			break;
		case DragState.Dragging:
		{
			Vector3 up = (controller.DragPosition - (Vector2)crank.transform.position).normalized;
			crank.transform.up = up;
			float num = Vector3.SignedAngle(checkpoint.transform.up, crank.transform.up, Vector3.forward);
			if (num < 0f)
			{
				num += 360f;
			}
			float num2 = currentAngle - num;
			if (num2 >= 0f && num2 < 50f)
			{
				progress += num2;
			}
			currentAngle = num;
			break;
		}
		}
		UpdateGamepadInput();
		progress = Mathf.Clamp(progress - progressDecay * Time.deltaTime, 0f, targetProgress);
		float num3 = Mathf.Clamp(progress / targetProgress, 0f, 1f);
		float value = Mathf.Lerp(0f, 1f, 1f - num3);
		powerBarSpriteRenderer.material.SetFloat(SHADER_Y_PERCENT, value);
		if (Constants.ShouldPlaySfx())
		{
			motorAudioSource.volume = num3;
			motorAudioSource.pitch = Mathf.Floor(num3 * 100f) / 100f;
		}
		if (Constants.ShouldPlaySfx())
		{
			if (progress < targetProgress / (float)crankAudioRate * (float)(crankAudioDiv - 1))
			{
				crankAudioDiv = Mathf.Clamp(crankAudioDiv - 1, 1, crankAudioRate);
			}
			if (progress >= targetProgress / (float)crankAudioRate * (float)crankAudioDiv && crankAudioDiv < crankAudioRate)
			{
				crankAudioDiv = Mathf.Clamp(crankAudioDiv + 1, 1, crankAudioRate);
				SoundManager.Instance.PlaySound(GetRandomNonRepeatSound(), loop: false);
			}
		}
		if (progress >= targetProgress)
		{
			TurnOnLights();
			finishCrank = true;
			StartCoroutine(WaitComplete());
		}
	}

	private IEnumerator WaitComplete()
	{
		yield return Effects.Wait(0.8f);
		MyNormTask.NextStep();
		if (MyNormTask.IsComplete)
		{
			StartCoroutine(CoStartClose());
		}
	}

	public override void Begin(PlayerTask task)
	{
		base.Begin(task);
		SetupInput(disableCursor: true);
		gamepad = ReInput.players.GetPlayer(0);
		progress = 0f;
		powerBarSpriteRenderer.material.SetFloat(SHADER_Y_PERCENT, progress);
		crankAudioDiv = 1;
		finishCrank = false;
		motorAudioSource = SoundManager.Instance.GetNamedSfxSource(CRANK_MOTOR_SFX);
		motorAudioSource.clip = motorAudioClip;
		motorAudioSource.loop = true;
		motorAudioSource.pitch = 0f;
		motorAudioSource.volume = 0f;
		motorAudioSource.Play();
		randomAudioClips = new List<AudioClip>();
	}

	public override void Close()
	{
		finishCrank = true;
		base.Close();
		SoundManager.Instance.StopNamedSound(CRANK_MOTOR_SFX);
	}

	private void UpdateGamepadInput()
	{
		if (gamepad == null)
		{
			return;
		}
		Vector2 normalized = gamepad.GetAxis2DRaw(13, 14).normalized;
		if (normalized.magnitude != 0f)
		{
			if (!prevGamepadAngle.HasValue)
			{
				prevGamepadAngle = normalized;
				return;
			}
			float value = Vector2.SignedAngle(normalized, prevGamepadAngle.Value);
			value = Mathf.Clamp(value, 0f, 50f) * gamepadAngleProgressMul;
			progress += value;
			VibrationManager.Vibrate(0.3f, 0.3f, 0.02f);
			crank.transform.up = normalized;
			prevGamepadAngle = normalized;
		}
	}

	private void TurnOnLights()
	{
		lightbulb.gameObject.SetActive(value: true);
		miniLights.gameObject.SetActive(value: true);
	}

	private AudioClip GetRandomNonRepeatSound()
	{
		randomAudioClips = new List<AudioClip>(turnAudioClips);
		AudioClip audioClip = randomAudioClips.Random();
		randomAudioClips.Remove(audioClip);
		return audioClip;
	}
}
