using System.Collections;
using Rewired;
using UnityEngine;

public class IncubateEggMinigame : Minigame
{
	private enum State
	{
		Empty,
		EggPlaced,
		EggHatched,
		Complete
	}

	private const int STATE_INDEX = 1;

	[SerializeField]
	private PassiveButton incubatorButton;

	[SerializeField]
	private SpriteRenderer egg;

	[SerializeField]
	private float secondsToHatchEgg = 5f;

	[SerializeField]
	private GameObject incubatorLight;

	[SerializeField]
	[Header("Critter")]
	private SpriteRenderer critter;

	[SerializeField]
	private IncubateEggMinigameEmote happyEmote;

	[SerializeField]
	private IncubateEggMinigameEmote thirstyEmote;

	[SerializeField]
	private IncubateEggMinigameEmote hungryEmote;

	[SerializeField]
	[Header("Food")]
	private PassiveButton foodBowl;

	[SerializeField]
	private GameObject food;

	[SerializeField]
	private PassiveButton waterBottle;

	[SerializeField]
	private GameObject waterFull;

	[Header("Timer")]
	[SerializeField]
	private FungleTimer timer;

	[Header("Glyphs")]
	[SerializeField]
	private GameObject emptyGlyphs;

	[SerializeField]
	private GameObject hatchedGlyphs;

	[Header("Audio")]
	[SerializeField]
	private AudioClip foodPlacedSfx;

	[SerializeField]
	private AudioClip waterPlacedSfx;

	[SerializeField]
	private AudioClip eggPlacedSfx;

	[SerializeField]
	private AudioClip eggHatchSfx;

	[SerializeField]
	private AudioClip timerEndSfx;

	private Player player;

	private float lastEmoteTime;

	private State CurrentState
	{
		get
		{
			return (State)MyNormTask.Data[1];
		}
		set
		{
			MyNormTask.Data[1] = (byte)value;
		}
	}

	private void Update()
	{
		UpdateEmotes();
		if (MyNormTask.IsComplete || MyNormTask.Data == null || MyNormTask.Data.Length <= 1)
		{
			return;
		}
		if (CurrentState == State.Empty)
		{
			if (player.GetButtonDown(11))
			{
				PutEgg();
			}
		}
		else if (CurrentState == State.EggPlaced)
		{
			timer.SetValue(MyNormTask.TaskTimer);
			if (MyNormTask.TaskTimer <= 0f)
			{
				if (Constants.ShouldPlaySfx())
				{
					SoundManager.Instance.PlaySoundImmediate(timerEndSfx, loop: false);
				}
				CurrentState = State.EggHatched;
				StartCoroutine(CoHatchEgg());
			}
		}
		else
		{
			if (CurrentState != State.EggHatched)
			{
				return;
			}
			if (player.GetButtonDown(21))
			{
				FillWaterBottle();
			}
			if (player.GetButtonDown(24))
			{
				FillFoodBowl();
			}
			if (food.activeSelf && waterFull.activeSelf)
			{
				CurrentState = State.Complete;
				MyNormTask.NextStep();
				StartCoroutine(Effects.ActionAfterDelay(0.75f, () =>
				{
					transform.parent.GetComponent<MultistageMinigame>().Close();
				}));
			}
		}
	}

	private void UpdateEmotes()
	{
		if (food.activeSelf && waterFull.activeSelf && !happyEmote.IsVisible)
		{
			thirstyEmote.SetVisible(visible: false);
			hungryEmote.SetVisible(visible: false);
			happyEmote.SetVisible(visible: true);
		}
		else if (!food.activeSelf && !waterFull.activeSelf)
		{
			if (thirstyEmote.IsVisible == hungryEmote.IsVisible)
			{
				thirstyEmote.SetVisible(visible: false);
				hungryEmote.SetVisible(visible: true);
				lastEmoteTime = Time.time;
			}
			if (Time.time > lastEmoteTime + 3f)
			{
				thirstyEmote.SetVisible(!thirstyEmote.IsVisible);
				hungryEmote.SetVisible(!hungryEmote.IsVisible);
				lastEmoteTime = Time.time;
			}
		}
		else
		{
			hungryEmote.SetVisible(!food.activeSelf);
			thirstyEmote.SetVisible(!waterFull.activeSelf);
		}
	}

	public override void Begin(PlayerTask task)
	{
		base.Begin(task);
		SetupInput(disableCursor: true);
		player = ReInput.players.GetPlayer(0);
		if (MyNormTask.Data == null || MyNormTask.Data.Length <= 1)
		{
			MyNormTask.Data = new byte[2]
			{
				(byte)((MyNormTask.Data != null) ? MyNormTask.Data[0] : 0),
				0
			};
		}
		critter.gameObject.SetActive(value: false);
		food.SetActive(value: false);
		waterFull.SetActive(value: false);
		incubatorLight.SetActive(value: false);
		egg.gameObject.SetActive(value: false);
		emptyGlyphs.SetActive(value: false);
		hatchedGlyphs.SetActive(value: false);
		UpdateEmotes();
		switch (CurrentState)
		{
		case State.Empty:
			incubatorButton.OnClick.AddListener(PutEgg);
			emptyGlyphs.SetActive(value: true);
			timer.Hide();
			break;
		case State.EggPlaced:
			SetEggPlaced();
			break;
		case State.EggHatched:
			critter.gameObject.SetActive(value: true);
			ActivateCritterFoodAndAnimate();
			break;
		}
	}

	private void PutEgg()
	{
		MyNormTask.TaskTimer = secondsToHatchEgg;
		MyNormTask.TimerStarted = NormalPlayerTask.TimerState.Started;
		MyNormTask.ShowTaskTimer = true;
		CurrentState = State.EggPlaced;
		emptyGlyphs.SetActive(value: false);
		incubatorButton.SetButtonEnableState(enabled: false);
		if (Constants.ShouldPlaySfx())
		{
			SoundManager.Instance.PlaySoundImmediate(eggPlacedSfx, loop: false);
		}
		SetEggPlaced();
		StartCoroutine(Effects.ActionAfterDelay(0.75f, () =>
		{
			transform.parent.GetComponent<MultistageMinigame>().Close();
		}));
	}

	private void SetEggPlaced()
	{
		egg.gameObject.SetActive(value: true);
		incubatorLight.SetActive(value: true);
		emptyGlyphs.SetActive(value: false);
		hatchedGlyphs.SetActive(value: false);
		timer.Show();
		timer.SetValue(MyNormTask.TaskTimer);
	}

	private IEnumerator CoHatchEgg()
	{
		if (Constants.ShouldPlaySfx())
		{
			SoundManager.Instance.PlaySoundImmediate(eggHatchSfx, loop: false);
		}
		VibrationManager.Vibrate(0.7f, 0.7f, 0.1f);
		yield return Effects.ColorFade(egg, Color.white, Palette.ClearWhite, 0.5f);
		critter.gameObject.SetActive(value: true);
		yield return Effects.ColorFade(critter, Palette.ClearWhite, Color.white, 0.5f);
		ActivateCritterFoodAndAnimate();
	}

	private void ActivateCritterFoodAndAnimate()
	{
		timer.Hide();
		incubatorLight.SetActive(value: false);
		emptyGlyphs.SetActive(value: false);
		hatchedGlyphs.SetActive(value: true);
		UpdateEmotes();
		foodBowl.OnClick.AddListener(FillFoodBowl);
		waterBottle.OnClick.AddListener(FillWaterBottle);
		StartCoroutine(CoAnimateCritter());
	}

	private void FillFoodBowl()
	{
		if (!food.activeSelf)
		{
			food.SetActive(value: true);
			foodBowl.SetButtonEnableState(enabled: false);
			if (Constants.ShouldPlaySfx())
			{
				SoundManager.Instance.PlaySoundImmediate(foodPlacedSfx, loop: false);
			}
			VibrationManager.Vibrate(0.3f, 0.3f, 0.1f);
		}
	}

	private void FillWaterBottle()
	{
		if (!waterFull.activeSelf)
		{
			waterFull.SetActive(value: true);
			waterBottle.SetButtonEnableState(enabled: false);
			if (Constants.ShouldPlaySfx())
			{
				SoundManager.Instance.PlaySoundImmediate(waterPlacedSfx, loop: false);
			}
			VibrationManager.Vibrate(0.3f, 0.3f, 0.1f);
		}
	}

	private IEnumerator CoAnimateCritter()
	{
		while (true)
		{
			yield return Effects.Bounce(critter.transform);
		}
	}
}
