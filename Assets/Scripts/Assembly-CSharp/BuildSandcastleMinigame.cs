using System;
using System.Collections;
using Cpp2ILInjected;
using Rewired;
using UnityEngine;
using UnityEngine.Audio;

public class BuildSandcastleMinigame : Minigame
{
	private const float CLOSE_ENOUGH_TO_TOP = 0.8f;

	[SerializeField]
	private float liftLerpSpeed = 5f;

	[SerializeField]
	private float fallGravity = 0.5f;

	[SerializeField]
	private Transform startPosition;

	[SerializeField]
	private Transform endPosition;

	[SerializeField]
	private Collider2D bucketCollider;

	[SerializeField]
	private SpriteRenderer bucketSprite;

	[SerializeField]
	private SpriteRenderer bucketBackSprite;

	[SerializeField]
	private GameObject bucketGlyph;

	[SerializeField]
	private AudioClip grabSound;

	[SerializeField]
	private AudioClip completeSound;

	private readonly Controller controller= new Controller();

	private Player player;

	private bool hasInput;

	private float fallVelocity;

	private float targetBucketY;

	private float trueBucketY;

	private unsafe void Update()
{
		if (MyNormTask.IsComplete)
		{
			return;
		}
		bool flag = hasInput;
		if (bucketCollider.enabled)
		{
			if (ActiveInputManager.currentControlType == ActiveInputManager.InputType.Joystick)
			{
				UpdateJoystickInput();
			}
			else
			{
				UpdateDefaultInput();
			}
		}
		if (flag && !hasInput)
		{
			fallVelocity = 0f;
		}
		if (hasInput && !flag && Constants.ShouldPlaySfx())
		{
			SoundManager.Instance.PlaySound(grabSound, loop: false);
		}
		if (trueBucketY >= 0.8f)
		{
			Success();
			return;
		}
		if (!hasInput && trueBucketY > 0f)
		{
			fallVelocity += fallGravity;
			SetBucketY(trueBucketY - fallVelocity * Time.deltaTime, instant: true);
		}
		UpdateBucketPosition();
	}

	public override void Begin(PlayerTask task)
	{
		base.Begin(task);
		SetupInput(disableCursor: true);
		player = ReInput.players.GetPlayer(0);
	}

	private void UpdateDefaultInput()
{
		hasInput = false;
		controller.Update();
		DragState dragState = controller.CheckDrag(bucketCollider);
		if ((uint)(dragState - 1) <= 2u)
		{
			hasInput = true;
			float num = endPosition.position.y - controller.DragPosition.y;
			float newValue = 1f - Mathf.Clamp01(num / Vector2.Distance(startPosition.position, endPosition.position));
			SetBucketY(newValue, instant: true);
		}
	}

	private void UpdateJoystickInput()
	{
		hasInput = false;
		if (player.GetAxisRaw(14) > 0.01f)
		{
			hasInput = true;
			SetBucketY(player.GetAxisRaw(14), instant: false);
		}
	}

	private void SetBucketY(float newValue, bool instant)
	{
		targetBucketY = Mathf.Clamp01(newValue);
		if (instant)
		{
			trueBucketY = targetBucketY;
		}
		else
		{
			trueBucketY = Mathf.Lerp(trueBucketY, targetBucketY, Time.deltaTime * liftLerpSpeed);
		}
	}

	private unsafe void UpdateBucketPosition()
{
		bucketCollider.transform.localPosition = Vector3.Lerp(startPosition.localPosition, endPosition.localPosition, trueBucketY);
	}

	private unsafe void Success()
{
		bucketCollider.enabled = false;
		bucketGlyph.SetActive(value: false);
		StartCoroutine(Effects.Sequence(Effects.All(Effects.ColorFade(bucketSprite, Color.white, Color.clear, 0.5f), Effects.ColorFade(bucketBackSprite, Color.white, Color.clear, 0.5f)), CoStartClose()));
		MyNormTask.NextStep();
		if (Constants.ShouldPlaySfx())
		{
			SoundManager.Instance.PlaySound(completeSound, loop: false);
		}
	}

	public BuildSandcastleMinigame()
	{ }
}
