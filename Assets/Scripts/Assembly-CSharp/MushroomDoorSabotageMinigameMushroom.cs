using System;
using System.Collections;
using UnityEngine;

public class MushroomDoorSabotageMinigameMushroom : MonoBehaviour
{
	public enum State
	{
		None,
		Visible,
		Invisible,
		Appearing,
		Disappearing,
		Whacked
	}

	[SerializeField]
	private PassiveButton button;

	[SerializeField]
	private SpriteRenderer sprite;

	[SerializeField]
	private ParticleSystem whackedEffect;

	[SerializeField]
	[Header("Audio")]
	private AudioClip mushroomAppearSFX;

	[SerializeField]
	private AudioClip mushroomDisappearSFX;

	[SerializeField]
	private AudioClip mushroomHitSFX;

	private float timeInState;

	public State CurrentState { get; private set; }

	public bool IsTimeInStateComplete => timeInState <= 0f;

	public event Action<MushroomDoorSabotageMinigameMushroom> OnClick;

	private void Start()
	{
		button.OnClick.AddListener(OnMushroomClick);
	}

	public void SetState(State state, float maxTimeInState)
	{
		if (CurrentState == State.Whacked)
		{
			return;
		}
		if (CurrentState == State.None)
		{
			CurrentState = state;
			timeInState = maxTimeInState;
			button.enabled = false;
			transform.localScale = Vector3.zero;
			if (CurrentState == State.Visible)
			{
				StartCoroutine(CoShowMushroom());
			}
			return;
		}
		switch (state)
		{
		case State.Visible:
			StartCoroutine(CoShowMushroom());
			break;
		case State.Invisible:
			StartCoroutine(CoHideMushroom());
			break;
		}
		timeInState = maxTimeInState;
	}

	public void DeductTimeInState(float deltaTime)
	{
		timeInState -= deltaTime;
	}

	private IEnumerator CoShowMushroom()
	{
		if (Constants.ShouldPlaySfx())
		{
			SoundManager.Instance.PlaySoundImmediate(mushroomAppearSFX, loop: false);
		}
		CurrentState = State.Appearing;
		yield return Effects.ScaleIn(transform, 0f, 1f, 0.6f);
		button.enabled = true;
		CurrentState = State.Visible;
	}

	private IEnumerator CoHideMushroom()
	{
		if (Constants.ShouldPlaySfx())
		{
			SoundManager.Instance.PlaySoundImmediate(mushroomDisappearSFX, loop: false);
		}
		CurrentState = State.Disappearing;
		yield return Effects.ScaleIn(transform, 1f, 0f, 0.3f);
		button.enabled = false;
		CurrentState = State.Invisible;
	}

	private void OnMushroomClick()
	{
		if (CurrentState != State.Whacked)
		{
			CurrentState = State.Whacked;
			StopAllCoroutines();
			StartCoroutine(CoAnimateWhackMushroom());
			OnClick?.Invoke(this);
		}
	}

	private IEnumerator CoAnimateWhackMushroom()
	{
		if (Constants.ShouldPlaySfx())
		{
			SoundManager.Instance.PlaySoundImmediate(mushroomHitSFX, loop: false);
		}
		whackedEffect.Play();
		button.SetButtonEnableState(enabled: false);
		sprite.gameObject.SetActive(value: true);
		yield return Effects.All(Effects.ScaleIn(transform, transform.localScale.x, 1.2f, 0.3f), Effects.ColorFade(sprite, Palette.White, Palette.ClearWhite, 0.2f));
	}
}
