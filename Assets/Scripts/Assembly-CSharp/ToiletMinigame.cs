using System.Collections;
using Rewired;
using UnityEngine;

public class ToiletMinigame : Minigame
{
	public SpriteRenderer Needle;

	public SpriteRenderer Pipes;

	public const float StickDown = -0.75f;

	public FloatRange StickRange = new FloatRange(-0.85f, -0.4f);

	public Collider2D Stick;

	public SpriteRenderer Plunger;

	public Sprite PlungerUp;

	public Sprite PlungerDown;

	private float pressure;

	public Controller controller = new Controller();

	public float lastY;

	public float plungeScale = 0.5f;

	public AudioClip flushSound;

	public AudioClip[] plungeSounds;

	private AudioSource plungerSource;

	private float controllerStickPos = 1f;

	private const float controllerPlungeSpeed = 30f;

	public override void Begin(PlayerTask task)
	{
		plungerSource = SoundManager.Instance.GetNamedSfxSource("plungerSource");
		base.Begin(task);
		SetupInput(disableCursor: true);
	}

	public void Update()
	{
		if (amClosing != CloseState.None || pressure >= 1f)
		{
			return;
		}
		pressure -= Time.deltaTime * plungeScale / 2f;
		if (pressure < 0f)
		{
			pressure = 0f;
		}
		controller.Update();
		Vector3 localPosition = Stick.transform.localPosition;
		if (Controller.currentTouchType == Controller.TouchType.Joystick)
		{
			float axisRaw = ReInput.players.GetPlayer(0).GetAxisRaw(14);
			if (axisRaw < 0f)
			{
				if (controllerStickPos > 1f + axisRaw)
				{
					controllerStickPos = Mathf.Lerp(controllerStickPos, 1f + axisRaw, Time.deltaTime * 30f);
				}
			}
			else if (axisRaw > 0f)
			{
				if (controllerStickPos < axisRaw)
				{
					controllerStickPos = Mathf.Lerp(controllerStickPos, axisRaw, Time.deltaTime * 30f);
				}
			}
			else
			{
				controllerStickPos = Mathf.Lerp(controllerStickPos, 1f, Time.deltaTime);
			}
			localPosition.y = StickRange.Lerp(controllerStickPos);
			Stick.transform.localPosition = localPosition;
			if (lastY > localPosition.y)
			{
				pressure += (lastY - localPosition.y) * plungeScale;
				if (Constants.ShouldPlaySfx() && !plungerSource.isPlaying)
				{
					plungerSource.clip = plungeSounds.Random();
					plungerSource.Play();
					VibrationManager.Vibrate(0.3f, 0.3f, 0.2f, VibrationManager.VibrationFalloff.Linear);
				}
			}
			lastY = localPosition.y;
		}
		else
		{
			switch (controller.CheckDrag(Stick))
			{
			case DragState.TouchStart:
				lastY = localPosition.y;
				break;
			case DragState.Dragging:
				localPosition.y = StickRange.Clamp(StickRange.max + (controller.DragPosition.y - controller.DragStartPosition.y));
				Stick.transform.localPosition = localPosition;
				if (lastY > localPosition.y)
				{
					pressure += (lastY - localPosition.y) * plungeScale;
					if (Constants.ShouldPlaySfx() && !plungerSource.isPlaying)
					{
						plungerSource.clip = plungeSounds.Random();
						plungerSource.Play();
					}
				}
				lastY = localPosition.y;
				break;
			case DragState.Released:
				localPosition.y = Mathf.Lerp(localPosition.y, StickRange.max, Time.deltaTime);
				Stick.transform.localPosition = localPosition;
				break;
			}
		}
		Plunger.sprite = ((localPosition.y < -0.75f) ? PlungerDown : PlungerUp);
		Needle.transform.localEulerAngles = new Vector3(0f, 0f, Mathf.Lerp(0f, -230f, pressure));
		if (pressure >= 1f)
		{
			StartCoroutine(Finish());
		}
	}

	private IEnumerator Finish()
	{
		if (Constants.ShouldPlaySfx())
		{
			SoundManager.Instance.PlaySound(flushSound, loop: false);
		}
		VibrationManager.Vibrate(2.5f, 2.5f, 0f, VibrationManager.VibrationFalloff.None, flushSound);
		MyNormTask.NextStep();
		yield return Effects.All(Effects.Shake(Pipes.transform, 0.65f, 0.05f, taper: true), Effects.Rotate2D(Needle.transform, 230f, 0f, 0.6f));
		Close();
	}

	public override void Close()
	{
		SoundManager.Instance.StopNamedSound("plungerSource");
		base.Close();
	}
}
