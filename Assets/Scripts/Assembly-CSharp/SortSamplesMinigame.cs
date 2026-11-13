using Object = UnityEngine.Object;
using System;
using System.Collections;
using System.Collections.Generic;
using Cpp2ILInjected;
using UnityEngine;

public class SortSamplesMinigame : Minigame
{
	private sealed class _003CCoWaitBeforeClose_003Ed__18 : IEnumerator<object>, IEnumerator, IDisposable
	{
		public int _003C_003E1__state;

		private object _003C_003E2__current;

		public SortSamplesMinigame _003C_003E4__this;

		object IEnumerator<object>.Current => _003C_003E2__current;

		object IEnumerator.Current => _003C_003E2__current;

		public _003CCoWaitBeforeClose_003Ed__18(int _003C_003E1__state)
		{
			((IDisposable)this).Dispose();
			this._003C_003E1__state = _003C_003E1__state;
		}

		void IDisposable.Dispose()
		{
		}

		private bool MoveNext()
		{
			if (_003C_003E1__state == 0)
			{
				_003C_003E1__state = -1;
				_003C_003E2__current = Effects.Wait(1.5f);
				_003C_003E1__state = 1;
				return true;
			}
			if (_003C_003E1__state == 1)
			{
				_003C_003E1__state = -1;
				_003C_003E4__this.transform.parent.GetComponent<MultistageMinigame>().Close();
			}
			return false;
		}

		bool IEnumerator.MoveNext()
		{
			//ILSpy generated this explicit interface implementation from .override directive in MoveNext
			return this.MoveNext();
		}

		void IEnumerator.Reset()
		{
			Cpp2ILHelpers.NoteDecompilerIssue("Method not found @1803A88A0");
			NotSupportedException ex = new NotSupportedException();
			Cpp2ILHelpers.NoteDecompilerIssue("Method not found @1803A88A0");
			throw ex;
		}
	}

	[SerializeField]
	private SortableSample[] sortableSamples;

	[SerializeField]
	private Collider2D mushroomJar;

	[SerializeField]
	private Transform mushroomJarDropSpot;

	[SerializeField]
	private Collider2D crystalJar;

	[SerializeField]
	private Transform crystalJarDropSpot;

	[SerializeField]
	private Transform hand;

	[SerializeField]
	private SpriteRenderer handOpenSprite;

	[SerializeField]
	private SpriteRenderer handClosedSprite;

	private readonly Controller controller= new Controller();

	private SortableSample currentSample;

	private int samplesSorted;

	private bool IsComplete => samplesSorted >= sortableSamples.Length;

	public override bool SkipMultistageOverlayMenuSetup => true;

	private void Update()
	{
		if (MyNormTask.IsComplete)
		{
			return;
		}
		controller.Update();
		SortableSample[] array = sortableSamples;
		foreach (SortableSample sortableSample in array)
		{
			switch (controller.CheckDrag(sortableSample.Collider))
			{
			case DragState.TouchStart:
				sortableSample.StartPickUp();
				break;
			case DragState.Holding:
			case DragState.Dragging:
			{
				currentSample = sortableSample;
				Vector3 localPosition = sortableSample.transform.localPosition;
				Vector2 vector = controller.DragPosition - (Vector2)transform.position;
				localPosition.x = vector.x;
				localPosition.y = vector.y;
				sortableSample.UpdatePosition(localPosition);
				UpdateCursorVisibility();
				break;
			}
			case DragState.Released:
				currentSample = null;
				if (sortableSample.IsMushroom && mushroomJar.OverlapPoint(sortableSample.transform.position))
				{
					samplesSorted++;
					sortableSample.DropIntoContainer(mushroomJarDropSpot);
				}
				else if (!sortableSample.IsMushroom && crystalJar.OverlapPoint(sortableSample.transform.position))
				{
					samplesSorted++;
					sortableSample.DropIntoContainer(crystalJarDropSpot);
				}
				else
				{
					sortableSample.SnapBackToOriginalPosition();
				}
				UpdateCursorVisibility();
				break;
			}
		}
		hand.position = VirtualCursor.currentPosition;
		hand.SetLocalZ(-0.5f);
		if (IsComplete)
		{
			UpdateCursorVisibility();
			MyNormTask.NextStep();
			StartCoroutine(CoWaitBeforeClose());
		}
	}

	public override void Begin(PlayerTask task)
	{
		base.Begin(task);
		SetupInput(disableCursor: false);
		ActiveInputManager.CurrentInputSourceChanged = (Action)Delegate.Combine(ActiveInputManager.CurrentInputSourceChanged, new Action(OnInputMethodChanged));
		UpdateCursorVisibility();
		SpriteRenderer[] componentsInChildren = hand.GetComponentsInChildren<SpriteRenderer>();
		foreach (SpriteRenderer playerMaterialColors in componentsInChildren)
		{
			PlayerControl.LocalPlayer.SetPlayerMaterialColors(playerMaterialColors);
		}
	}

	public override void Close()
	{
		base.Close();
		ActiveInputManager.CurrentInputSourceChanged = (Action)Delegate.Remove(ActiveInputManager.CurrentInputSourceChanged, new Action(OnInputMethodChanged));
	}

	private IEnumerator CoWaitBeforeClose()
	{
		yield return Effects.Wait(1.5f);
		transform.parent.GetComponent<MultistageMinigame>().Close();
	}

	private void OnInputMethodChanged()
	{
		UpdateCursorVisibility();
	}

	private void UpdateCursorVisibility()
	{
		hand.gameObject.SetActive(!IsComplete && ActiveInputManager.currentControlType == ActiveInputManager.InputType.Joystick);
		handClosedSprite.enabled = currentSample != null;
		handOpenSprite.enabled = currentSample == null;
	}

	public SortSamplesMinigame()
	{ }
}
