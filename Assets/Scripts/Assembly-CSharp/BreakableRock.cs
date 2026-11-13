using System;
using System.Collections;
using UnityEngine;

public class BreakableRock : MonoBehaviour
{
	private const float FALL_DURATION = 0.3f;

	[SerializeField]
	private SpriteRenderer spriteRenderer;

	[SerializeField]
	private Sprite[] rockSprites;

	[SerializeField]
	private PassiveButton passiveButton;

	[SerializeField]
	private AnimationCurve animCurve;

	[SerializeField]
	private CollectableOre myOre;

	[SerializeField]
	private Collider2D col;

	private Action<CollectableOre, Vector3> powerMeterAction;

	private Controller controller;

	public bool CanInteract { get; private set; } = true;

	private void Update()
	{
		if (CanInteract && controller.CheckDrag(col) == DragState.TouchStart)
		{
			TouchRock();
		}
	}

	public void Initialize(Action<CollectableOre, Vector3> powerAction, Controller control)
	{
		controller = control;
		powerMeterAction = (Action<CollectableOre, Vector3>)Delegate.Combine(powerMeterAction, powerAction);
		spriteRenderer.sprite = rockSprites.Random();
	}

	private void TouchRock()
	{
		if (CanInteract && !IsUsingController())
		{
			powerMeterAction?.Invoke(myOre, controller.HoverPosition);
		}
	}

	public CollectableOre GetOre()
	{
		return myOre;
	}

	public void SendPower()
	{
		StartCoroutine(FallRockAnim());
	}

	public void SetInput(bool isEnabled)
	{
		CanInteract = isEnabled;
	}

	private IEnumerator FallRockAnim()
	{
		Vector3 position = transform.position;
		float num = ((UnityEngine.Random.Range(0, 2) == 0) ? UnityEngine.Random.Range(-0.5f, -1.5f) : UnityEngine.Random.Range(0.5f, 1.5f));
		position.x += num;
		StartCoroutine(Effects.Sequence(Effects.Wait(0.15f), Effects.ColorFade(spriteRenderer, Color.white, Color.clear, 0.15f)));
		yield return Effects.CurvePositionY(transform, animCurve, 0.3f, num);
		gameObject.SetActive(value: false);
	}

	private bool IsUsingController()
	{
		return ActiveInputManager.currentControlType == ActiveInputManager.InputType.Joystick;
	}
}
