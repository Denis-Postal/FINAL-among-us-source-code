using System;
using System.Collections;
using System.Collections.Generic;
using Rewired;
using UnityEngine;

public class MineOreMinigame : Minigame
{
	private const string BAR_FILL_SFX = "mineOrePowerBarLoop";

	private const int ORE_NUMBER = 3;

	private const int MAX_GOOD_ROCKS = 2;

	private const int MAX_PERFECT_ROCKS = 4;

	[Tooltip("Speed of power meter when breaking rocks")]
	[SerializeField]
	private float powerBarSpeed = 1f;

	[Tooltip("angle of pickaxe while animating breaking a rock")]
	[SerializeField]
	private float pikeaxeFirstAngle = -90f;

	[Tooltip("angle of pickaxe while animating breaking a rock")]
	[SerializeField]
	private float pikeaxeSecondAngle = 50f;

	[SerializeField]
	[Tooltip("time of pickeaxe animation in seconds")]
	private float pikeaxeAnimTime = 0.3f;

	[SerializeField]
	[Tooltip("Good Zone percentage max value")]
	private float goodPercentage = 0.5f;

	[SerializeField]
	[Tooltip("Great Zone percentage max value")]
	private float greatPercentage = 0.8f;

	[SerializeField]
	private Collider2D pickaxe;

	[SerializeField]
	private GameObject pickaxeSprite;

	[SerializeField]
	private GameObject pickControllerHelper;

	[SerializeField]
	private PowerBarMining powerBarMining;

	[SerializeField]
	private List<CollectableOre> ores;

	[Header("Audio Files")]
	[SerializeField]
	private AudioClip[] rockHitWeak;

	[SerializeField]
	private AudioClip[] rockHitStrong;

	[SerializeField]
	private AudioClip[] rockRelease;

	[SerializeField]
	private AudioClip pickaxeSwing;

	[SerializeField]
	private AudioClip powerBarLoop;

	private AudioSource powerBarAudioSource;

	private int currentOreCount;

	private CollectableOre currentOre;

	private Controller controller = new Controller();

	private bool isMining;

	private Player gamepad;

	private void Update()
	{
		if (isMining || powerBarMining.isActiveAndEnabled)
		{
			if (VirtualCursor.instance != null)
			{
				VirtualCursor.instance.SetWorldPosition(pickaxe.transform.position);
			}
			return;
		}
		controller.Update();
		if (!IsUsingController())
		{
			return;
		}
		pickaxe.transform.position = VirtualCursor.currentPosition;
		pickaxe.transform.SetLocalZ(-0.5f);
		if (!gamepad.GetButtonDown(21))
		{
			return;
		}
		List<Collider2D> list = new List<Collider2D>();
		if (Physics2D.OverlapCircle(VirtualCursor.currentPosition, 0.2f, default(ContactFilter2D).NoFilter(), list) <= 0)
		{
			return;
		}
		foreach (Collider2D item in list)
		{
			BreakableRock component = item.GetComponent<BreakableRock>();
			if (component != null && component.CanInteract)
			{
				CallPowerMeter(component.GetOre(), VirtualCursor.currentPosition);
				break;
			}
		}
	}

	public override void Begin(PlayerTask task)
	{
		base.Begin(task);
		SetupInput(disableCursor: false);
		pickaxe.gameObject.SetActive(IsUsingController());
		pickControllerHelper.SetActive(IsUsingController());
		ActiveInputManager.CurrentInputSourceChanged = (Action)Delegate.Combine(ActiveInputManager.CurrentInputSourceChanged, new Action(OnInputChanged));
		gamepad = ReInput.players.GetPlayer(0);
		currentOreCount = 0;
		SpawnRocksAndOres();
		powerBarAudioSource = SoundManager.Instance.GetNamedSfxSource("mineOrePowerBarLoop");
		powerBarAudioSource.clip = powerBarLoop;
		powerBarAudioSource.loop = true;
		powerBarAudioSource.pitch = 0f;
		powerBarAudioSource.volume = 0f;
		foreach (CollectableOre ore in ores)
		{
			ore.Initialize(OreCollected, CallPowerMeter, controller, RockReleaseAudio);
		}
		SetRockOreInput(isEnabled: true);
	}

	public override void Close()
	{
		MyNormTask.Data = new byte[ores.Count * 2];
		for (int i = 0; i < ores.Count; i++)
		{
			MyNormTask.Data[i] = (byte)ores[i].GetNumberRocksBroken();
		}
		for (int j = ores.Count; j < ores.Count * 2; j++)
		{
			if (!ores[j - ores.Count].GetIsRock())
			{
				MyNormTask.Data[j] = 0;
			}
			else
			{
				MyNormTask.Data[j] = 1;
			}
		}
		powerBarAudioSource.Stop();
		base.Close();
		ActiveInputManager.CurrentInputSourceChanged = (Action)Delegate.Remove(ActiveInputManager.CurrentInputSourceChanged, new Action(OnInputChanged));
	}

	private void OnInputChanged()
	{
		if (!isMining)
		{
			pickaxe.gameObject.SetActive(IsUsingController());
			pickControllerHelper.SetActive(IsUsingController());
		}
	}

	private void SpawnRocksAndOres()
	{
		List<CollectableOre> list = new List<CollectableOre>(ores);
		if (MyNormTask.Data != null && MyNormTask.Data.Length == ores.Count * 2)
		{
			int num = 0;
			for (int i = 0; i < ores.Count; i++)
			{
				int num2 = MyNormTask.Data[i];
				ores[i].SetNumRocks(num2);
				bool flag = MyNormTask.Data[i + ores.Count] == 0;
				if (num2 >= 4)
				{
					if (flag)
					{
						currentOreCount++;
					}
					else
					{
						ores[i].SetAsRock();
					}
					ores[i].gameObject.SetActive(value: false);
					continue;
				}
				ores[i].RotateObjects();
				if (!flag)
				{
					ores[i].SetAsRock();
					continue;
				}
				ores[i].SetOreSprite(num);
				num++;
				num = Mathf.Clamp(num, 0, 2);
			}
		}
		else
		{
			for (int j = 0; j < 3; j++)
			{
				int index = list.RandomIdx();
				CollectableOre collectableOre = list[index];
				collectableOre.SetNumRocks(0);
				collectableOre.RotateObjects();
				collectableOre.SetOreSprite(j);
				list.Remove(collectableOre);
			}
			for (int k = 0; k < list.Count; k++)
			{
				CollectableOre collectableOre2 = list[k];
				collectableOre2.SetNumRocks(0);
				collectableOre2.SetAsRock();
				collectableOre2.RotateObjects();
				collectableOre2.transform.SetParent(transform);
			}
		}
	}

	private void OreCollected()
	{
		currentOreCount++;
		if (currentOreCount >= 3)
		{
			MyNormTask.NextStep();
			if (MyNormTask.IsComplete)
			{
				StartCoroutine(CoStartClose());
			}
		}
	}

	private void CallPowerMeter(CollectableOre ore, Vector3 clickPos)
	{
		if (Constants.ShouldPlaySfx())
		{
			powerBarAudioSource.volume = 1f;
			powerBarAudioSource.Play();
		}
		currentOre = ore;
		SetRockOreInput(isEnabled: false);
		powerBarMining.gameObject.SetActive(value: true);
		powerBarMining.Initialize(powerBarSpeed, HitRock, clickPos, UpdateAudioPitch);
		pickaxe.gameObject.SetActive(value: true);
		pickaxe.transform.position = new Vector3(clickPos.x, clickPos.y, pickaxe.transform.position.z);
		StartCoroutine(Effects.Rotate2D(pickaxeSprite.transform, 0f, pikeaxeFirstAngle, pikeaxeAnimTime));
	}

	private void UpdateAudioPitch(float power)
	{
		if (Constants.ShouldPlaySfx())
		{
			powerBarAudioSource.pitch = 1f + power;
		}
	}

	private void HitRock(float power, Vector3 clickPos)
	{
		AudioClip clipToPlay;
		List<BreakableRock> rocksToBreak;
		if (power <= goodPercentage)
		{
			clipToPlay = rockHitWeak.Random();
			rocksToBreak = GetRocks(clickPos, 1);
		}
		else if (power <= greatPercentage)
		{
			clipToPlay = rockHitStrong.Random();
			rocksToBreak = GetRocks(clickPos, 2);
		}
		else
		{
			clipToPlay = rockHitStrong.Random();
			rocksToBreak = GetRocks(clickPos, 4);
		}
		isMining = true;
		VibrationManager.Vibrate(power * 2f, power * 2f, 0.1f);
		if (Constants.ShouldPlaySfx())
		{
			SoundManager.Instance.PlaySound(pickaxeSwing, loop: false);
		}
		powerBarAudioSource.Stop();
		StartCoroutine(Effects.Sequence(Effects.Rotate2D(pickaxeSprite.transform, pikeaxeFirstAngle, pikeaxeSecondAngle, pikeaxeAnimTime), Effects.Action(() =>
		{
			powerBarMining.gameObject.SetActive(value: false);
			if (!IsUsingController())
			{
				pickaxe.gameObject.SetActive(value: false);
			}
			else if (VirtualCursor.instance != null)
			{
				VirtualCursor.instance.SetWorldPosition(pickaxe.transform.position);
			}
			foreach (BreakableRock item in rocksToBreak)
			{
				item.SendPower();
				if ((bool)currentOre)
				{
					currentOre.BrokenRock();
				}
			}
			if (Constants.ShouldPlaySfx())
			{
				SoundManager.Instance.PlaySound(clipToPlay, loop: false);
			}
			currentOre = null;
			StartCoroutine(CoWaitBreakAnimations());
		})));
	}

	private IEnumerator CoWaitBreakAnimations()
	{
		yield return Effects.Wait(0.5f);
		isMining = false;
		SetRockOreInput(isEnabled: true);
	}

	private List<BreakableRock> GetRocks(Vector3 startPoint, int numRocks)
	{
		List<BreakableRock> list = new List<BreakableRock>();
		BreakableRock[] rocks = currentOre.GetRocks();
		foreach (BreakableRock breakableRock in rocks)
		{
			if (breakableRock.gameObject.activeInHierarchy)
			{
				list.Add(breakableRock);
			}
		}
		if (numRocks >= list.Count)
		{
			return new List<BreakableRock>(list);
		}
		List<BreakableRock> list2 = new List<BreakableRock>();
		BreakableRock closestRock = GetClosestRock(startPoint, list);
		if ((bool)closestRock)
		{
			list2.Add(closestRock);
			list.Remove(closestRock);
		}
		if (numRocks == 1 || list.Count <= 0)
		{
			return list2;
		}
		closestRock = GetClosestRock(startPoint, list);
		if ((bool)closestRock)
		{
			list2.Add(closestRock);
		}
		return list2;
	}

	private BreakableRock GetClosestRock(Vector3 startPoint, List<BreakableRock> potentialRocks)
	{
		float num = 10000f;
		BreakableRock result = null;
		foreach (BreakableRock potentialRock in potentialRocks)
		{
			float num2 = Vector3.Distance(potentialRock.transform.position, startPoint);
			if (num2 < num)
			{
				result = potentialRock;
				num = num2;
			}
		}
		return result;
	}

	private bool IsUsingController()
	{
		return ActiveInputManager.currentControlType == ActiveInputManager.InputType.Joystick;
	}

	private void SetRockOreInput(bool isEnabled)
	{
		foreach (CollectableOre ore in ores)
		{
			BreakableRock[] rocks = ore.GetRocks();
			for (int i = 0; i < rocks.Length; i++)
			{
				rocks[i].SetInput(isEnabled);
			}
		}
	}

	private void RockReleaseAudio()
	{
		if (Constants.ShouldPlaySfx())
		{
			SoundManager.Instance.PlaySound(rockRelease.Random(), loop: false);
		}
	}
}
