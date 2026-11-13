using System;
using System.Collections;
using Rewired;
using UnityEngine;

public class CollectableVegetable : MonoBehaviour
{
	public delegate void CollectableVegetableHandler(CollectableVegetable vegetable);

	private const string PULL_SFX_NAME = "_PULL_SFX";

	private const string VIBRATION_NAME = "CollectVegetableStretch";

	[SerializeField]
	[Header("Design")]
	private float secondsToHoldBeforeCollecting = 2f;

	[Header("Vegetables")]
	[SerializeField]
	private GameObject carrot;

	[SerializeField]
	private GameObject cauliflower;

	[SerializeField]
	private GameObject chard;

	[SerializeField]
	private GameObject curls;

	[Header("Navigation")]
	[SerializeField]
	private CollectableVegetable neighborUp;

	[SerializeField]
	private CollectableVegetable neighborDown;

	[SerializeField]
	private CollectableVegetable neighborLeft;

	[SerializeField]
	private CollectableVegetable neighborRight;

	[Header("Visuals")]
	[SerializeField]
	private GameObject vegetableContainer;

	[SerializeField]
	private Collider2D collider;

	[SerializeField]
	private GameObject selectionGlyph;

	[SerializeField]
	private GameObject selectionCursor;

	[Header("Audio")]
	[SerializeField]
	private AudioClip pullSfx;

	[SerializeField]
	private AudioClip[] pickSfx;

	private CollectVegetablesMinigame.Vegetable vegetableType;

	private GameObject currentVegetable;

	private readonly Controller controller = new Controller();

	private Player player;

	private bool selected;

	private float stretchStartTime;

	private int veggieIndex;

	private bool stretching;

	private bool collected;

	private Coroutine shrinkCoroutine;

	public int Index => veggieIndex;

	public string SfxName => gameObject.name + "_PULL_SFX";

	public event CollectableVegetableHandler OnTouchedEvent;

	public event CollectableVegetableHandler OnCollectedEvent;

	public event CollectableVegetableHandler OnNavigationEvent;

	private void OnEnable()
	{
		ActiveInputManager.CurrentInputSourceChanged = (Action)Delegate.Combine(ActiveInputManager.CurrentInputSourceChanged, new Action(OnInputMethodChanged));
	}

	private void OnDisable()
	{
		ActiveInputManager.CurrentInputSourceChanged = (Action)Delegate.Remove(ActiveInputManager.CurrentInputSourceChanged, new Action(OnInputMethodChanged));
	}

	private void Update()
	{
		if (ActiveInputManager.currentControlType == ActiveInputManager.InputType.Joystick)
		{
			if (!selected)
			{
				return;
			}
			if (player.GetButtonUp(14))
			{
				TryNavigate(neighborUp);
			}
			else if (player.GetNegativeButtonUp(14))
			{
				TryNavigate(neighborDown);
			}
			else if (player.GetButtonUp(13))
			{
				TryNavigate(neighborRight);
			}
			else if (player.GetNegativeButtonUp(13))
			{
				TryNavigate(neighborLeft);
			}
			else if (!collected)
			{
				if (player.GetAxisRaw(17) > 0.5f && !stretching)
				{
					StartStretch();
				}
				else if (player.GetButtonUp(17) && stretching)
				{
					TryResetStretch();
				}
			}
		}
		else if (!collected)
		{
			controller.Update();
			switch (controller.CheckDrag(collider))
			{
			case DragState.TouchStart:
				OnTouchedEvent?.Invoke(this);
				StartStretch();
				break;
			case DragState.Released:
				TryResetStretch();
				break;
			}
		}
	}

	public void Initialize(CollectVegetablesMinigame.Vegetable vegetable, int index)
	{
		player = ReInput.players.GetPlayer(0);
		veggieIndex = index;
		vegetableType = vegetable;
		carrot.SetActive(value: false);
		cauliflower.SetActive(value: false);
		chard.SetActive(value: false);
		curls.SetActive(value: false);
		currentVegetable = GetVegetable(vegetableType);
		currentVegetable.SetActive(value: true);
	}

	public void SetSelection(bool isSelected)
	{
		if (selected && !isSelected)
		{
			TryResetStretch();
		}
		selected = isSelected;
		UpdateSelectionCursor();
	}

	private GameObject GetVegetable(CollectVegetablesMinigame.Vegetable vegetableType)
	{
		switch (vegetableType)
		{
		default:
			Debug.LogError("[CollectableVegetable] Vegetable has not been configured. Returning null.");
			return null;
		case CollectVegetablesMinigame.Vegetable.Carrot:
			return carrot;
		case CollectVegetablesMinigame.Vegetable.Cauliflower:
			return cauliflower;
		case CollectVegetablesMinigame.Vegetable.Chard:
			return chard;
		case CollectVegetablesMinigame.Vegetable.Curls:
			return curls;
		}
	}

	private void UpdateSelectionCursor()
	{
		selectionCursor.SetActive(selected && ActiveInputManager.currentControlType == ActiveInputManager.InputType.Joystick);
		selectionGlyph.SetActive(!collected);
	}

	private void OnInputMethodChanged()
	{
		UpdateSelectionCursor();
	}

	private void TryNavigate(CollectableVegetable neighbor)
	{
		if (neighbor != null)
		{
			OnNavigationEvent?.Invoke(neighbor);
		}
	}

	private void StartStretch()
	{
		StopStretch();
		stretching = true;
		stretchStartTime = Time.time;
		shrinkCoroutine = StartCoroutine(CoStretchUp());
	}

	private void StopStretch()
	{
		stretching = false;
		SoundManager.Instance.StopNamedSound(SfxName);
		VibrationManager.CancelVibration("CollectVegetableStretch");
		if (shrinkCoroutine != null)
		{
			StopCoroutine(shrinkCoroutine);
			shrinkCoroutine = null;
		}
	}

	private void TryResetStretch()
	{
		if (Time.time - stretchStartTime < secondsToHoldBeforeCollecting)
		{
			StopStretch();
			currentVegetable.transform.localScale = Vector3.one;
		}
	}

	private IEnumerator CoStretchUp()
	{
		if (Constants.ShouldPlaySfx())
		{
			SoundManager.Instance.PlayNamedSound(SfxName, pullSfx, loop: true, SoundManager.Instance.SfxChannel);
		}
		VibrationManager.Vibrate(0.3f, 0.3f, 1f, VibrationManager.VibrationFalloff.None, null, loopClip: false, "CollectVegetableStretch");
		for (float timer = 0f; timer < 1f; timer += Time.deltaTime)
		{
			float t = timer / 1f;
			currentVegetable.transform.localScale = new Vector3(1f, Mathf.Lerp(0.9f, 1.4f, t), 1f);
			yield return null;
		}
		SoundManager.Instance.StopNamedSound(SfxName);
		if (Constants.ShouldPlaySfx())
		{
			SoundManager.Instance.PlaySoundImmediate(pickSfx.Random(), loop: false);
		}
		vegetableContainer.transform.localPosition = new Vector3(vegetableContainer.transform.localPosition.x, vegetableContainer.transform.localPosition.y + 0.1f, vegetableContainer.transform.localPosition.z);
		for (float timer = 0f; timer < 0.1f; timer += Time.deltaTime)
		{
			float t2 = timer / 0.1f;
			currentVegetable.transform.localScale = new Vector3(Mathf.Lerp(0.8f, 1.1f, t2), Mathf.Lerp(1.1f, 0.95f, t2), 1f);
			yield return null;
		}
		vegetableContainer.transform.localPosition = new Vector3(vegetableContainer.transform.localPosition.x, vegetableContainer.transform.localPosition.y + 0.2f, vegetableContainer.transform.localPosition.z);
		for (float timer = 0f; timer < 0.1f; timer += Time.deltaTime)
		{
			float t3 = timer / 0.1f;
			currentVegetable.transform.localScale = new Vector3(Mathf.Lerp(1.1f, 1f, t3), Mathf.Lerp(0.95f, 1f, t3), 1f);
			yield return null;
		}
		yield return CoCollect();
	}

	private IEnumerator CoCollect()
	{
		collected = true;
		UpdateSelectionCursor();
		Vector3 localPosition = currentVegetable.transform.localPosition;
		Vector3 dest = localPosition + currentVegetable.transform.up.Mul(new Vector3(0f, 1.5f, 0f));
		yield return Effects.All(Effects.Slide3D(currentVegetable.transform, localPosition, dest, 0.5f), Effects.ScaleIn(currentVegetable.transform, 1f, 0f, 0.5f));
		OnCollectedEvent?.Invoke(this);
	}
}
