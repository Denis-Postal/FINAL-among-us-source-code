using System.Collections;
using System.Collections.Generic;
using Rewired;
using UnityEngine;

public class VentCleaningMinigame : Minigame
{
	public FloatRange XSpan = new FloatRange(-1.15f, 1.15f);

	public FloatRange YSpan = new FloatRange(-1.15f, 1.15f);

	public ObjectPoolBehavior dirtPool;

	public GameObject ventLidClosed;

	public GameObject ventLidOpened;

	public UiElement backButton;

	public AudioClip VentOpenSound;

	public AudioClip ImpostorDiscoveredSound;

	public AudioClip CleanedSound;

	public SpriteRenderer[] SpiderWebs;

	public float cleaningDistanceThreshold;

	public Transform selectorObject;

	public SpriteRenderer selectorHand;

	private int numberOfDirts;

	private int numberOfDirtsCleanedUp;

	private bool ventOpen;

	private VentilationSystem ventSystem;

	private int VentId => ConsoleId;

	public override void Begin(PlayerTask task)
	{
		base.Begin(task);
		ventOpen = false;
		ventLidClosed.SetActive(value: true);
		ventLidOpened.SetActive(value: false);
		numberOfDirts = MyNormTask.Data[MyNormTask.MaxStep];
		numberOfDirtsCleanedUp = 0;
		ventSystem = ShipStatus.Instance.Systems[SystemTypes.Ventilation] as VentilationSystem;
		for (int i = 0; i < numberOfDirts; i++)
		{
			SpawnDirt();
		}
		SpiderWebs.ForEach((SpriteRenderer s) =>
		{
			s.enabled = false;
		});
		SpiderWebs.RandomSet(2).ForEach((SpriteRenderer s) =>
		{
			s.enabled = true;
		});
		if (!PlayerControl.LocalPlayer.Data.IsDead)
		{
			VentilationSystem.Update(VentilationSystem.Operation.StartCleaning, VentId);
		}
		selectorObject.gameObject.SetActive(value: false);
		PlayerControl.LocalPlayer.SetPlayerMaterialColors(selectorHand);
		SetupInput(disableCursor: false);
	}

	public override void Close()
	{
		if (amClosing != CloseState.Closing)
		{
			VentilationSystem.Update(VentilationSystem.Operation.StopCleaning, VentId);
		}
		base.Close();
	}

	public void OpenVent()
	{
		if (!ventOpen)
		{
			StartCoroutine(CoOpenVent());
		}
	}

	private void FixedUpdate()
	{
		if (amOpening || amClosing != CloseState.None)
		{
			return;
		}
		if (!PlayerControl.LocalPlayer.Data.IsDead && ventSystem != null && ventSystem.IsImpostorInsideVent(VentId))
		{
			if (Constants.ShouldPlaySfx())
			{
				SoundManager.Instance.PlaySound(ImpostorDiscoveredSound, loop: false, 0.8f);
			}
			VentilationSystem.Update(VentilationSystem.Operation.BootImpostors, VentId);
			Close();
		}
		else if (Controller.currentTouchType == Controller.TouchType.Joystick)
		{
			HandleJoystick();
		}
		else if (selectorObject.gameObject.activeSelf)
		{
			selectorObject.gameObject.SetActive(value: false);
		}
	}

	private void HandleJoystick()
	{
		Player player = ReInput.players.GetPlayer(0);
		bool button = player.GetButton(11);
		if (ventOpen && !selectorObject.gameObject.activeSelf)
		{
			selectorObject.gameObject.SetActive(value: true);
		}
		if (ventOpen)
		{
			Vector3 position = selectorObject.position;
			position.x = VirtualCursor.currentPosition.x;
			position.y = VirtualCursor.currentPosition.y;
			selectorObject.position = position;
			if (!player.GetButtonUp(11))
			{
				return;
			}
			float num = 0f;
			int num2 = -1;
			List<PoolableBehavior> activeChildren = dirtPool.activeChildren;
			for (int i = 0; i < activeChildren.Count; i++)
			{
				float sqrMagnitude = ((Vector2)(activeChildren[i].transform.localPosition - selectorObject.localPosition)).sqrMagnitude;
				if (sqrMagnitude <= cleaningDistanceThreshold && (num2 == -1 || sqrMagnitude < num))
				{
					num = sqrMagnitude;
					num2 = i;
				}
			}
			if (num2 != -1)
			{
				PoolableBehavior poolableBehavior = activeChildren[num2];
				poolableBehavior.StopAllCoroutines();
				CleanUp(poolableBehavior.GetComponent<VentDirt>());
			}
		}
		else if (button)
		{
			OpenVent();
		}
	}

	private IEnumerator CoOpenVent()
	{
		SoundManager.Instance.PlaySound(VentOpenSound, loop: false);
		yield return Effects.Lerp(0.2f, (float t) =>
		{
			ventLidClosed.transform.localScale = new Vector3(1f, Mathf.Lerp(1f, -0.5f, t), 1f);
			ventLidClosed.transform.localPosition = new Vector3(0f, Mathf.Lerp(0f, ventLidOpened.transform.localPosition.y, t), 3f);
		});
		ventLidClosed.SetActive(value: false);
		ventLidOpened.SetActive(value: true);
		ventOpen = true;
	}

	private void CleanUp(VentDirt ventDirt)
	{
		if (!ventOpen)
		{
			return;
		}
		if (Constants.ShouldPlaySfx())
		{
			SoundManager.Instance.PlaySound(CleanedSound, loop: false).pitch = FloatRange.Next(0.8f, 1.2f);
		}
		if (MyNormTask.IsComplete)
		{
			return;
		}
		StartCoroutine(ventDirt.CoDisappear());
		numberOfDirtsCleanedUp++;
		if (numberOfDirtsCleanedUp < numberOfDirts)
		{
			return;
		}
		MyNormTask.NextStep();
		StartCoroutine(CoStartClose());
		foreach (VentDirt activeChild in dirtPool.activeChildren)
		{
			if (!(activeChild == ventDirt))
			{
				StartCoroutine(activeChild.CoDisappear());
			}
		}
	}

	private void SpawnDirt()
	{
		VentDirt dirt = dirtPool.Get<VentDirt>();
		dirt.transform.localPosition = new Vector3(XSpan.Next(), YSpan.Next(), 4f);
		dirt.transform.localEulerAngles = new Vector3(0f, 0f, Random.Range(0, 360));
		dirt.GetComponent<ButtonBehavior>().OnClick.AddListener(() =>
		{
			CleanUp(dirt);
		});
	}
}
